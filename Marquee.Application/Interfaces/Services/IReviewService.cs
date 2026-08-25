using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Services;

public interface IReviewService
{
    Task<IReadOnlyList<Review>> GetByMediaIdAsync(int mediaId, CancellationToken cancellationToken = default);
    Task<Review> CreateAsync(int userId, int mediaId, string content, CancellationToken cancellationToken = default);
    Task UpdateAsync(int userId, int reviewId, string content, CancellationToken cancellationToken = default);
    Task DeleteAsync( int userId, int reviewId, CancellationToken cancellationToken = default);
}