using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Repositories;

public interface IRatingRepository
{
    Task<Rating?> GetAsync(int userId, int mediaId);
    Task<IReadOnlyList<Rating>> GetAllAsync(int mediaId);
    Task<IReadOnlyList<Rating>> GetForUserAsync(int userId);
    Task AddAsync(Rating rating);
    void Update(Rating rating);
    void Delete(Rating rating);
}