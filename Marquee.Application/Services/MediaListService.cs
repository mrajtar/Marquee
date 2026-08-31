using Marquee.Application.Exceptions;
using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Repositories;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;

namespace Marquee.Application.Services;

public class MediaListService : IMediaListService
{
    private readonly IMediaListRepository _mediaListRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediaRepository _mediaRepository;

    public MediaListService(IMediaListRepository mediaListRepository, ICurrentUserService currentUserService ,IUnitOfWork unitOfWork, IMediaRepository mediaRepository)
    {
        _mediaListRepository = mediaListRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
        _mediaRepository = mediaRepository;
    }

    public async Task<IReadOnlyList<MediaList>> GetUserListsAsync(int userId,
        CancellationToken cancellationToken = default)
    {
        var lists = await _mediaListRepository.GetByUserIdAsync(userId, cancellationToken);
        if (_currentUserService.UserId == userId || _currentUserService.IsAdmin)
            return lists;
        return [.. lists.Where(l => l.IsPublic)];
    }

    public async Task<MediaList?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var list = await _mediaListRepository.GetByIdAsync(id, cancellationToken);
        return list is not null && CanViewList(list) ? list : null;
    }

    public async Task<MediaList?> GetByIdWithItemsAsync(int id, CancellationToken cancellationToken = default)
    {
        var list = await _mediaListRepository.GetByIdWithItemsAsync(id, cancellationToken);
        return list is not null && CanViewList(list) ? list : null;
    }

    public async Task<MediaList> CreateAsync(MediaList mediaList, CancellationToken cancellationToken = default)
    {
        if (!_currentUserService.IsAuthenticated)
            throw new UnauthorizedAccessException("Authentication required.");
        if (string.IsNullOrWhiteSpace(mediaList.Name))
            throw new ArgumentException("List name is required.", nameof(mediaList));

        mediaList.UserId = _currentUserService.UserId!.Value;
        mediaList.CreatedAt = DateTime.UtcNow;
        
        if (await _mediaListRepository.ExistsAsync(mediaList.UserId, mediaList.Name, cancellationToken))
            throw new InvalidOperationException("A list with this name already exists.");
        
        await _mediaListRepository.AddAsync(mediaList, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return mediaList;
    }

    public async Task UpdateAsync(MediaList mediaList, CancellationToken cancellationToken = default)
    {
        var existing = await _mediaListRepository.GetByIdAsync(mediaList.Id, cancellationToken);
        if (existing is null)
            throw new KeyNotFoundException($"Media list with ID {mediaList.Id} not found.");

        EnsureOwner(existing);
        
        if (!string.Equals(existing.Name, mediaList.Name, StringComparison.OrdinalIgnoreCase) &&
            await _mediaListRepository.ExistsAsync(existing.UserId, mediaList.Name, cancellationToken))
        {
            throw new InvalidOperationException("A list with this name already exists.");
        }

        existing.Name = mediaList.Name;
        existing.Description = mediaList.Description;
        existing.IsPublic = mediaList.IsPublic;
        existing.UpdatedAt = DateTime.UtcNow;

        _mediaListRepository.Update(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var list = await _mediaListRepository.GetByIdAsync(id, cancellationToken);
        if (list is null)
            throw new KeyNotFoundException($"Media list with ID {id} not found.");

        EnsureOwner(list);
        
        _mediaListRepository.Delete(list);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task AddItemAsync(int mediaListId, int mediaId, CancellationToken cancellationToken = default)
    {
        var list = await _mediaListRepository.GetByIdAsync(mediaListId, cancellationToken);
        var media = await _mediaRepository.GetByIdAsync(mediaId, cancellationToken);
        
        if (media is null)
            throw new KeyNotFoundException($"Media with ID {mediaId} not found.");
        
        if (list is null)
            throw new KeyNotFoundException($"Media list with ID {mediaListId} not found.");

        EnsureOwner(list);
        
        var existingItem = await _mediaListRepository.GetItemAsync(mediaListId, mediaId, cancellationToken);
        if (existingItem is not null)
            return;

        var item = new MediaListItem
        {
            MediaListId = mediaListId,
            MediaId = mediaId,
            AddedAt = DateTime.UtcNow
        };
        await _mediaListRepository.AddItemAsync(item, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveItemAsync(int mediaListId, int mediaId, CancellationToken cancellationToken = default)
    {
        var list = await _mediaListRepository.GetByIdAsync(mediaListId, cancellationToken);
        if (list is null)
            throw new KeyNotFoundException($"Media list with ID {mediaListId} not found.");
        EnsureOwner(list);
        var item = await _mediaListRepository.GetItemAsync(mediaListId, mediaId, cancellationToken);
        if (item is null)
            throw new KeyNotFoundException("Item not found in list.");

        _mediaListRepository.RemoveItem(item);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }



    private bool CanViewList(MediaList mediaList)
    {
        if (mediaList.IsPublic) return true;
        if (_currentUserService.IsAdmin) return true;
        return mediaList.UserId == _currentUserService.UserId;
    }

    private void EnsureOwner(MediaList mediaList)
    {
        if (!_currentUserService.IsAuthenticated)
            throw new UnauthorizedAccessException("Authentication required.");
        if (mediaList.UserId != _currentUserService.UserId && !_currentUserService.IsAdmin)
        {
            throw new ForbiddenAccessException("You do not own this list.");
        }
    }
}