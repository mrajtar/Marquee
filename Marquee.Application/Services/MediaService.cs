using System.Globalization;
using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Repositories;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;

namespace Marquee.Application.Services;

public class MediaService : IMediaService
{
    private readonly IMediaRepository _mediaRepository;
    private readonly IUnitOfWork  _unitOfWork;

    public MediaService(IMediaRepository mediaRepository, IUnitOfWork unitOfWork)
    {
        _mediaRepository = mediaRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Media?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _mediaRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Media?> GetByIdWithDetails(int id, CancellationToken cancellationToken = default)
    {
        return await _mediaRepository.GetByIdWithDetailsAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<Media>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _mediaRepository.GetAllAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Media>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return [];
        
        return await _mediaRepository.SearchAsync(searchTerm, cancellationToken);
    }

    public async Task<Media> AddAsync(Media media, IReadOnlyCollection<int> genreIds, IReadOnlyCollection<int> keywordIds, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(media);
        
        AddGenres(media, genreIds);
        AddKeywords(media, keywordIds);
        
        await _mediaRepository.AddAsync(media, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return media;
    }

    public async Task UpdateAsync(Media media, IReadOnlyCollection<int> genreIds, IReadOnlyCollection<int> keywordIds, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(media);
        
        var existingMedia = await _mediaRepository.GetByIdWithDetailsAsync(media.Id, cancellationToken);
        if  (existingMedia == null)
            throw new KeyNotFoundException($"Media with id {media.Id} was not found.");
        
        existingMedia.Title = media.Title;
        existingMedia.OriginalTitle = media.OriginalTitle;
        existingMedia.Overview = media.Overview;
        existingMedia.PosterUrl = media.PosterUrl;
        existingMedia.BackdropUrl = media.BackdropUrl;
        existingMedia.Status = media.Status;
        existingMedia.ReleaseDate = media.ReleaseDate;
        existingMedia.TmdbId = media.TmdbId;
        existingMedia.ImdbId = media.ImdbId;
        
        existingMedia.MediaGenres.Clear();
        existingMedia.MediaKeywords.Clear();
        AddGenres(existingMedia, genreIds);
        AddKeywords(existingMedia, keywordIds);
        
        _mediaRepository.Update(existingMedia);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var media = await _mediaRepository.GetByIdAsync(id, cancellationToken);
        if (media == null)
            throw new KeyNotFoundException($"Media with id {id} was not found.");
        
        _mediaRepository.Delete(media);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    private static void AddGenres(Media media, IEnumerable<int> genreIds)
    {
        foreach (var genreId in genreIds.Distinct())
        {
            media.MediaGenres.Add(new MediaGenre
            {
                MediaId = media.Id,
                GenreId = genreId
            });
        }
    }

    private static void AddKeywords(Media media, IEnumerable<int> keywordIds)
    {
        foreach (var keywordId in keywordIds.Distinct())
        {
            media.MediaKeywords.Add(new MediaKeyword
            {
                MediaId = media.Id,
                KeywordId = keywordId
            });
        }
    }
}