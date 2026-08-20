using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Repositories;

public interface IKeywordRepository
{
    Task<Keyword?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Keyword?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Keyword>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Keyword>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task AddAsync(Keyword keyword, CancellationToken cancellationToken = default);
    void Update(Keyword keyword);
    void Delete(Keyword keyword);
}