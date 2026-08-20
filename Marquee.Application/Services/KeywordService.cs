using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Repositories;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;

namespace Marquee.Application.Services;

public class KeywordService : IKeywordService
{
    private readonly IKeywordRepository _keywordRepository;
    private readonly IUnitOfWork _unitOfWork;

    public KeywordService(IKeywordRepository keywordRepository, IUnitOfWork unitOfWork)
    {
        _keywordRepository = keywordRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Keyword?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _keywordRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Keyword?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _keywordRepository.GetByIdWithDetailsAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<Keyword>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _keywordRepository.GetAllAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Keyword>> SearchAsync(string searchTerm,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return [];
        return await _keywordRepository.SearchAsync(searchTerm, cancellationToken);
    }

    public async Task<Keyword> AddAsync(Keyword keyword, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(keyword);
        await _keywordRepository.AddAsync(keyword, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return keyword;
    }

    public async Task UpdateAsync(Keyword keyword, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(keyword);
        var existingKeyword = await _keywordRepository.GetByIdAsync(keyword.Id, cancellationToken);
        if (existingKeyword is null)
        {
            throw new KeyNotFoundException($"Keyword with ID {keyword.Id} was not found.");
        }
        
        existingKeyword.Name = keyword.Name;
        existingKeyword.TmdbId = keyword.TmdbId;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var keyword = await _keywordRepository.GetByIdAsync(id, cancellationToken);
        if (keyword is null)
        {
            throw new KeyNotFoundException($"Keyword with ID {id} was not found.");
        }

        _keywordRepository.Delete(keyword);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}