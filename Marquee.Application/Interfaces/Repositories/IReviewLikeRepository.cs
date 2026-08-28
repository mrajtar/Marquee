using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Repositories;

public interface IReviewLikeRepository
{
    Task<ReviewLike?> GetAsync(int userId, int reviewId, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(int reviewId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int userId, int reviewId, CancellationToken cancellationToken = default);
    Task AddAsync(ReviewLike reviewLike, CancellationToken cancellationToken = default);
    void Delete(ReviewLike reviewLike);
}