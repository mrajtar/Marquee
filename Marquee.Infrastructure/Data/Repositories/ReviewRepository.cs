using Marquee.Application.DTOs.Review;
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

    public async Task<IReadOnlyList<ReviewListDto>> GetByMediaIdAsync(int mediaId, int? currentUserId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews
            .AsNoTracking()
            .Where(r => r.MediaId == mediaId)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ReviewListDto
            {
                Id = r.Id,
                MediaId = r.MediaId,
                Username = r.User.UserName!,
                DisplayName = r.User.DisplayName,
                ProfileImageUrl = r.User.ProfileImageUrl,
                Content = r.Content,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                ContainsSpoilers = r.ContainsSpoilers,
                LikeCount = r.Likes.Count(),
                LikedByCurrentUser = currentUserId.HasValue ? r.Likes.Any(x => x.UserId == currentUserId.Value) : null
            }).ToListAsync(cancellationToken);
    }

    public async Task<ReviewListDto?> GetDtoByIdAsync(int reviewId, int? currentUserId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews
            .AsNoTracking()
            .Where(r => r.Id == reviewId)
            .Select(r => new ReviewListDto
            {
                Id = r.Id,
                MediaId = r.MediaId,
                Username = r.User.UserName!,
                DisplayName = r.User.DisplayName,
                ProfileImageUrl = r.User.ProfileImageUrl,
                Content = r.Content,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                ContainsSpoilers = r.ContainsSpoilers,
                LikeCount = r.Likes.Count(),
                LikedByCurrentUser = currentUserId.HasValue ? r.Likes.Any(x => x.UserId == currentUserId.Value) : null
            }).FirstOrDefaultAsync(cancellationToken);
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