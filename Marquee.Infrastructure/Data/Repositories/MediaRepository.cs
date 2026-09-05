using Marquee.Application.DTOs.Media;
using Marquee.Application.Interfaces.Repositories;
using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marquee.Infrastructure.Data.Repositories;

public class MediaRepository : IMediaRepository
{
    private readonly MarqueeDbContext _context;

    public MediaRepository(MarqueeDbContext context)
    {
        _context = context;
    }

    public async Task<Media?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Media
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Media?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Media
            .Include(m => m.MediaGenres)
            .ThenInclude(mg => mg.Genre)
            .Include(m => m.MediaKeywords)
            .ThenInclude(mk => mk.Keyword)
            .Include(m => m.MediaPeople)
            .ThenInclude(mp => mp.Person)
            .Include(m => m.MediaCountries)
            .ThenInclude(mc => mc.Country)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Media>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Media
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<MediaListDto>> GetAllAsListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Media
            .AsNoTracking()
            .OrderBy(m => m.Title)
            .Select(m => new MediaListDto
            {
                Id = m.Id,
                Title = m.Title,
                PosterUrl = m.PosterUrl,
                BackdropUrl = m.BackdropUrl,
                ReleaseDate = m.ReleaseDate,
                AverageRating = m.Ratings
                    .Select(r => (double?)r.Value)
                    .Average(),
                RatingCount = m.Ratings.Count,
                MediaType = m.Type
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Media>> SearchAsync(string searchTerm,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return [];

        searchTerm = searchTerm.Trim();

        return await _context.Media
            .AsNoTracking()
            .Where(m => m.Title.Contains(searchTerm))
            .OrderBy(m => m.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<(double? AverageRating, int RatingCount)> GetRatingSummaryAsync(int mediaId,
        CancellationToken cancellationToken = default)
    {
        var ratings = _context.Ratings.Where(r => r.MediaId == mediaId);
        var count = await ratings.CountAsync(cancellationToken);

        if (count is 0)
            return (null, 0);

        var average = await ratings.AverageAsync(r => (double)r.Value, cancellationToken);
        return (average, count);
    }

    public async Task<int> GetReviewCountAsync(int mediaId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews.CountAsync(r => r.MediaId == mediaId, cancellationToken);
    }

    public async Task<IReadOnlyList<Media>> GetTrendingAsync(int count, CancellationToken cancellationToken = default)
    {
        return await _context.Media
            .AsNoTracking()
            .OrderByDescending(m => m.TrendingScore)
            .ThenByDescending(m => m.ReleaseDate)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Media?> GetFeaturedAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Media
            .AsNoTracking()
            .Where(m => m.BackdropUrl != null)
            .OrderByDescending(m => m.TrendingScore)
            .ThenByDescending(m => m.ReleaseDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Media media, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(media);
        await _context.Media.AddAsync(media, cancellationToken);
    }

    public void Update(Media media)
    {
        ArgumentNullException.ThrowIfNull(media);
        _context.Media.Update(media);
    }

    public void Delete(Media media)
    {
        ArgumentNullException.ThrowIfNull(media);
        _context.Media.Remove(media);
    }
}