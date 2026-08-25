using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Services;

public interface IRatingService
{
    Task<Rating?> GetAsync(int userId, int mediaId, CancellationToken cancellationToken);
    Task<Rating> SetAsync(int userId, int mediaId, int value, CancellationToken cancellationToken);
    Task DeleteAsync(int userId, int mediaId, CancellationToken cancellationToken);
}