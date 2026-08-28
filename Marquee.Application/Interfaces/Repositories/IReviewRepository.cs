using Marquee.Application.DTOs.Review;
using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Repositories;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Review?> GetByIdWitUserAsync(int id, CancellationToken cancellationToken = default);
    Task<Review?> GetByUserAndMediaAsync(int userId, int mediaId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ReviewListDto>> GetByMediaIdAsync(int mediaId, int? currentUserId, CancellationToken cancellationToken = default);
    Task<ReviewListDto?> GetDtoByIdAsync(int reviewId, int? currentUserId, CancellationToken cancellationToken = default);
    Task AddAsync(Review review, CancellationToken cancellationToken = default);
    void Delete(Review review);
}