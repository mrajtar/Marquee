using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Services;

public interface IKeywordService
{
    Task<Keyword?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Keyword?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Keyword>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Keyword>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<Keyword> AddAsync(Keyword keyword, CancellationToken cancellationToken = default);
    Task UpdateAsync(Keyword keyword, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}