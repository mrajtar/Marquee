using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Repositories;

public interface IGenreRepository
{
    Task<Genre?> GetByIdAsync (int id, CancellationToken cancellationToken = default);
    Task<Genre?> GetByIdWithDetailsAsync (int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Genre>> GetAllAsync (CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Genre>> SearchAsync  (string searchTerm, CancellationToken cancellationToken = default);
    Task AddAsync (Genre genre, CancellationToken cancellationToken = default);
    void Update(Genre genre);
    void Delete(Genre genre);
}