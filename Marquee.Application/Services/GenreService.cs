using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Repositories;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;

namespace Marquee.Application.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GenreService(IGenreRepository genreRepository, IUnitOfWork unitOfWork)
    {
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Genre?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _genreRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Genre?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _genreRepository.GetByIdWithDetailsAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<Genre>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _genreRepository.GetAllAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Genre>> SearchAsync(string searchTerm,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return [];
        return await _genreRepository.SearchAsync(searchTerm, cancellationToken);
    }

    public async Task<Genre> AddAsync(Genre genre, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(genre);
        await _genreRepository.AddAsync(genre, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return genre;
    }

    public async Task UpdateAsync(Genre genre, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(genre);
        var existingGenre = await _genreRepository.GetByIdAsync(genre.Id, cancellationToken);
        if (existingGenre is null)
        {
            throw new KeyNotFoundException($"Genre with ID {genre.Id} was not found.");
        }

        existingGenre.Name = genre.Name;
        existingGenre.TmdbId = genre.TmdbId;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var genre = await _genreRepository.GetByIdAsync(id, cancellationToken);
        if (genre is null)
        {
            throw new KeyNotFoundException($"Genre with ID {id} was not found.");
        }

        _genreRepository.Delete(genre);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}