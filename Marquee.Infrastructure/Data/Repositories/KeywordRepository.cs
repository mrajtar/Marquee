using Marquee.Application.Interfaces.Repositories;
using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marquee.Infrastructure.Data.Repositories;

public class KeywordRepository : IKeywordRepository
{
    private readonly MarqueeDbContext _context;

    public KeywordRepository(MarqueeDbContext context)
    {
        _context = context;
    }

    public async Task<Keyword?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Keywords
            .FirstOrDefaultAsync(k => k.Id == id, cancellationToken);
    }

    public async Task<Keyword?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Keywords
            .Include(k => k.MediaKeywords)
            .ThenInclude(mk => mk.Media)
            .FirstOrDefaultAsync(k => k.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Keyword>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Keywords
            .AsNoTracking()
            .OrderBy(k => k.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Keyword>> SearchAsync(string searchTerm,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return [];
        searchTerm = searchTerm.Trim();
        return await _context.Keywords
            .AsNoTracking()
            .Where(k => k.Name.Contains(searchTerm))
            .OrderBy(k => k.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Keyword keyword, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(keyword);
        await _context.Keywords.AddAsync(keyword, cancellationToken);
    }

    public void Update(Keyword keyword)
    {
        ArgumentNullException.ThrowIfNull(keyword);
        _context.Keywords.Update(keyword);
    }

    public void Delete(Keyword keyword)
    {
        ArgumentNullException.ThrowIfNull(keyword);
        _context.Keywords.Remove(keyword);
    }
}