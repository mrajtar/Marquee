using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Repositories;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(int id);
    Task<IReadOnlyList<Review>> GetForMediaAsync(int mediaId);
    Task<IReadOnlyList<Review>> GetForUserAsync(int userId);
    Task AddAsync(Review review);
    void Update(Review review);
    void Delete(Review review);
}