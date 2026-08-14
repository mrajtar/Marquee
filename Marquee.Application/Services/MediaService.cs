using System.Globalization;
using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Repositories;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;

namespace Marquee.Application.Services;

public class MediaService : IMediaService
{
    private readonly IMediaRepository _mediaRepository;
    private readonly IUnitOfWork  _unitOfWork;

    public MediaService(IMediaRepository mediaRepository, IUnitOfWork unitOfWork)
    {
        _mediaRepository = mediaRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Media?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _mediaRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Media?> GetByIdWithDetails(int id, CancellationToken cancellationToken = default)
    {
        return await _mediaRepository.GetByIdWithDetailsAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<Media>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _mediaRepository.GetAllAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Media>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return [];
        
        return await _mediaRepository.SearchAsync(searchTerm, cancellationToken);
    }

    public async Task<Media> AddAsync(Media media, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(media);
        
        await _mediaRepository.AddAsync(media, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return media;
    }

    public async Task UpdateAsync(Media media, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(media);
        
        _mediaRepository.Update(media);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var media = await _mediaRepository.GetByIdAsync(id, cancellationToken);
        if (media == null)
            throw new KeyNotFoundException($"Media wit id {id} was not found");
        
        _mediaRepository.Delete(media);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}