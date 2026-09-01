using Marquee.Application.DTOs.Review;

namespace Marquee.Application.Interfaces.Services;

public interface IReviewService
{
    Task<IReadOnlyList<ReviewListDto>> GetByMediaIdAsync(int mediaId, int? currentUserId, CancellationToken cancellationToken = default);
    Task<ReviewListDto?> GetDtoByIdAsync(int reviewId, int? currentUserId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ReviewListDto>> GetRecentAsync(int? currentUserId, int count, CancellationToken cancellationToken = default);
    Task<ReviewListDto> CreateAsync(int userId, int mediaId, string content, bool containsSpoilers, CancellationToken cancellationToken = default);
    Task UpdateAsync(int userId, int reviewId, string content, bool containsSpoilers, CancellationToken cancellationToken = default);
    Task DeleteAsync(int userId, int reviewId, CancellationToken cancellationToken = default);
}