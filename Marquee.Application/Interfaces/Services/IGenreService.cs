using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Services;

public interface IGenreService
{
    Task<Genre?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Genre?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Genre>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Genre>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<Genre> AddAsync( Genre genre, CancellationToken cancellationToken = default);
    Task UpdateAsync(Genre genre, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}