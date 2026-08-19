using Marquee.Domain.Entities;

namespace Marquee.Application.Interfaces.Repositories;

public interface IPersonRepository
{
    Task<Person?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Person?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Person>> GetAllAsync(CancellationToken  cancellationToken = default);
    Task<IReadOnlyList<Person>> SearchAsync(string searchTerm, CancellationToken  cancellationToken = default);
    Task AddAsync(Person person, CancellationToken cancellationToken = default);
    void Update(Person person);
    void Delete(Person person);
}