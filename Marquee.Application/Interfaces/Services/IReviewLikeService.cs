namespace Marquee.Application.Interfaces.Services;

public interface IReviewLikeService
{
    Task<bool> IsLikedAsync (int userId, int reviewId, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(int reviewId, CancellationToken cancellationToken = default);
    Task LikeAsync(int userId, int reviewId, CancellationToken cancellationToken = default);
    Task UnlikeAsync(int userId, int reviewId, CancellationToken cancellationToken = default);
}