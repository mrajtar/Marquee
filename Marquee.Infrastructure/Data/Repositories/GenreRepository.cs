using Marquee.Application.Interfaces.Repositories;
using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marquee.Infrastructure.Data.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly MarqueeDbContext _context;
    
    public GenreRepository(MarqueeDbContext context)
    {
        _context =  context;
    }
    
    public async Task<Genre?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Genres
            .FirstOrDefaultAsync( g => g.Id == id, cancellationToken);
    }

    public async Task<Genre?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Genres
            .Include(g => g.MediaGenres)
                .ThenInclude(mg => mg.Media)
            .FirstOrDefaultAsync( g => g.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Genre>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Genres
            .AsNoTracking()
            .OrderBy(g => g.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Genre>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return [];

        searchTerm = searchTerm.Trim();

        return await _context.Genres
            .AsNoTracking()
            .Where(g => g.Name.Contains(searchTerm))
            .OrderBy(g => g.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Genre genre, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(genre);

        await _context.Genres.AddAsync( genre, cancellationToken);
    }

    public void Update(Genre genre)
    {
        ArgumentNullException.ThrowIfNull(genre);

        _context.Genres.Update(genre);
    }

    public void Delete(Genre genre)
    {
        ArgumentNullException.ThrowIfNull(genre);

        _context.Genres.Remove(genre);
    }
}