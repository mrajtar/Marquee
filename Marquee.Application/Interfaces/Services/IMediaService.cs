using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Services;

public interface IMediaService
{
    Task<Media?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Media?> GetByIdWithDetails(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Media>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Media>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<Media> AddAsync(Media media, CancellationToken cancellationToken = default);
    Task UpdateAsync(Media media, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}