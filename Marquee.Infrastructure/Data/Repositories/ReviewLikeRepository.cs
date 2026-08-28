using Marquee.Application.Interfaces.Repositories;
using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marquee.Infrastructure.Data.Repositories;

public class ReviewLikeRepository : IReviewLikeRepository
{
    private readonly MarqueeDbContext _context;
    
    public ReviewLikeRepository(MarqueeDbContext context)
    {
        _context = context;
    }
    
    public async Task<ReviewLike?> GetAsync(int userId, int reviewId, CancellationToken cancellationToken = default)
    {
        return await _context.ReviewLikes.FirstOrDefaultAsync(x => x.UserId == userId && x.ReviewId == reviewId, cancellationToken);
    }

    public async Task<int> GetCountAsync(int reviewId, CancellationToken cancellationToken = default)
    {
        return await _context.ReviewLikes.CountAsync(x => x.ReviewId == reviewId, cancellationToken);
    }

    public async Task<bool> ExistsAsync(int userId, int reviewId, CancellationToken cancellationToken = default)
    {
        return await _context.ReviewLikes.AnyAsync(x => x.UserId == userId && x.ReviewId == reviewId, cancellationToken);
    }

    public async Task AddAsync(ReviewLike reviewLike, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(reviewLike);
        await _context.ReviewLikes.AddAsync(reviewLike, cancellationToken);
    }

    public void Delete(ReviewLike reviewLike)
    {
        ArgumentNullException.ThrowIfNull(reviewLike);
        _context.ReviewLikes.Remove(reviewLike);
    }
}