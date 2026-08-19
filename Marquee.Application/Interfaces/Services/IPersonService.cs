using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Services;

public interface IPersonService
{
    Task<Person?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Person?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Person>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Person>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<Person> AddAsync(Person person, CancellationToken cancellationToken = default);
    Task UpdateAsync(Person person, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}