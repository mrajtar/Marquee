using Marquee.Application.Interfaces.Repositories;
using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marquee.Infrastructure.Data.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly MarqueeDbContext _context;

    public ReviewRepository(MarqueeDbContext context)
    {
        _context = context;
    }
    
    public async Task<Review?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Review?> GetByIdWitUserAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Review?> GetByUserAndMediaAsync(int userId, int mediaId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews.FirstOrDefaultAsync(r => r.UserId  == userId && r.MediaId == mediaId, cancellationToken);
    }

    public async Task<IReadOnlyList<Review>> GetByMediaIdAsync(int mediaId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews.AsNoTracking().Include(r => r.User).Where(r => r.MediaId == mediaId).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Review review, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(review);
        await _context.Reviews.AddAsync(review, cancellationToken);
    }

    public void Delete(Review review)
    {
        ArgumentNullException.ThrowIfNull(review);
        _context.Reviews.Remove(review);
    }
}