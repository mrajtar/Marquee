using Marquee.Application.DTOs.MediaList;
using Marquee.Application.Interfaces.Repositories;
using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marquee.Infrastructure.Data.Repositories;

public class MediaListRepository : IMediaListRepository
{
    private readonly MarqueeDbContext _context;
    
    public MediaListRepository(MarqueeDbContext context)
    {
        _context = context;
    }
    
    public async Task<MediaList?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.MediaLists.FirstOrDefaultAsync(ml => ml.Id == id, cancellationToken);
    }

    public async Task<MediaList?> GetByIdWithItemsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.MediaLists
            .Include(ml => ml.Items)
                .ThenInclude(mli => mli.Media)
            .FirstOrDefaultAsync(ml => ml.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(int userId, string name, CancellationToken cancellationToken = default)
    {
        return await _context.MediaLists
            .AnyAsync(ml => ml.UserId == userId && ml.Name == name, cancellationToken);
    }

    public async Task AddAsync(MediaList mediaList, CancellationToken cancellationToken = default)
    {
        await _context.MediaLists.AddAsync(mediaList, cancellationToken);
    }

    public void Update(MediaList mediaList)
    {
        _context.MediaLists.Update(mediaList);
    }

    public void Delete(MediaList mediaList)
    {
        _context.MediaLists.Remove(mediaList);
    }

    public async Task AddItemAsync(MediaListItem item, CancellationToken cancellationToken = default)
    {
        await _context.MediaListItems.AddAsync(item, cancellationToken);
    }

    public void RemoveItem(MediaListItem item)
    {
        _context.MediaListItems.Remove(item);
    }

    public async Task<MediaListItem?> GetItemAsync(int mediaListId, int mediaId, CancellationToken cancellationToken = default)
    {
        return await _context.MediaListItems
            .FirstOrDefaultAsync(mli => mli.MediaListId == mediaListId && mli.MediaId == mediaId, cancellationToken);
    }

    public async Task<IReadOnlyList<MediaListDto>> GetUserListDtosAsync(int userId, int? mediaId, CancellationToken cancellationToken = default)
    {
        return await _context.MediaLists
            .AsNoTracking()
            .Where(ml => ml.UserId == userId)
            .OrderBy(ml => ml.CreatedAt)
            .Select(ml => new MediaListDto
            {
                Id = ml.Id,
                Name = ml.Name,
                Description = ml.Description,
                IsPublic = ml.IsPublic,
                CreatedAt = ml.CreatedAt,
                UpdatedAt = ml.UpdatedAt,
                ItemCount = ml.Items.Count(),
                ContainsMedia = mediaId.HasValue && ml.Items.Any(i => i.MediaId == mediaId.Value)
            })
            .ToListAsync(cancellationToken);
    }
}