using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Repositories;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;

namespace Marquee.Application.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PersonService(IPersonRepository personRepository, IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Person?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _personRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Person?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _personRepository.GetByIdWithDetailsAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<Person>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _personRepository.GetAllAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Person>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return [];

        return await _personRepository.SearchAsync(searchTerm, cancellationToken);
    }

    public async Task<Person> AddAsync(Person person, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(person);
        
        if (string.IsNullOrWhiteSpace(person.Name))
            throw new ArgumentException("Person name is required", nameof(person));
        
        await _personRepository.AddAsync(person, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return person;
    }

    public async Task UpdateAsync(Person person, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(person);
        
        var existingPerson = await _personRepository.GetByIdAsync(person.Id,  cancellationToken);
        
        if (existingPerson == null)
            throw new KeyNotFoundException($"Person with id {person.Id} not found");
        
        existingPerson.Name = person.Name;
        existingPerson.ProfileImageUrl = person.ProfileImageUrl;
        existingPerson.BirthDate = person.BirthDate;
        existingPerson.DeathDate = person.DeathDate;
        existingPerson.Biography = person.Biography;
        existingPerson.TmdbId = person.TmdbId;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var person = await GetByIdAsync(id, cancellationToken);
        if (person == null)
            throw new KeyNotFoundException($"Person with id {id} not found");
        
        _personRepository.Delete(person);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}