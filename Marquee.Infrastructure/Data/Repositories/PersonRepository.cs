using Marquee.Application.Interfaces.Repositories;
using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marquee.Infrastructure.Data.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly MarqueeDbContext _context;
    
    public PersonRepository(MarqueeDbContext context)
    {
        _context = context;
    }
    public async Task<Person?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.People
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Person?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.People
            .Include(p => p.MediaPeople)
                .ThenInclude(mp => mp.Media)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Person>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.People.AsNoTracking().OrderBy(p => p.Name).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Person>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return [];
        
        searchTerm = searchTerm.Trim();
        
        return await _context.People
            .AsNoTracking()
            .Where(p => p.Name.Contains(searchTerm))
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Person person, CancellationToken cancellationToken = default)
    {
        await _context.People.AddAsync(person, cancellationToken);
    }

    public void Update(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);
        _context.People.Update(person);
    }

    public void Delete(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);
        _context.People.Remove(person);
    }
}