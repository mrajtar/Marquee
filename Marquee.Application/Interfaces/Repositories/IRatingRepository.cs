using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Repositories;

public interface IRatingRepository
{
    Task<Rating?> GetAsync(int userId, int mediaId, CancellationToken cancellationToken);
    Task AddAsync(Rating rating, CancellationToken cancellationToken);
    void Delete(Rating rating);
}