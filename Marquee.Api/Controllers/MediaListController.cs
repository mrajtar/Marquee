using AutoMapper;
using Marquee.Application.DTOs.MediaList;
using Marquee.Application.Exceptions;
using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marquee.Controllers;

[ApiController]
[Route("api/lists")]
public class MediaListController : ControllerBase
{
    private readonly IMediaListService _mediaListService;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public MediaListController(IMediaListService mediaListService, IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _mediaListService = mediaListService;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IReadOnlyList<MediaListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyLists([FromQuery] int? mediaId, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId is null)
            return Unauthorized();
        
        var result = await _mediaListService.GetUserListsAsync(userId.Value, mediaId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(MediaListDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var list = await _mediaListService.GetByIdWithItemsAsync(id, cancellationToken);
        if (list is null) return NotFound();
        var result = _mapper.Map<MediaListDetailsDto>(list);
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(MediaListDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Add([FromBody] CreateMediaListDto dto, CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated) 
            return Unauthorized();
        
        var mediaList = _mapper.Map<MediaList>(dto);
        
        try
        {
            var created = await _mediaListService.CreateAsync(mediaList, cancellationToken);
            var result = _mapper.Map<MediaListDto>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMediaListDto dto, CancellationToken cancellationToken)
    {
        var mediaList = _mapper.Map<MediaList>(dto);
        mediaList.Id = id;
        
        try
        {
            await _mediaListService.UpdateAsync(mediaList, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (ForbiddenAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _mediaListService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (ForbiddenAccessException)
        {
            return Forbid();
        }
    }

    [HttpPost("{id:int}/items")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddItem(int id, [FromBody] AddMediaListItemDto dto, CancellationToken cancellationToken)
    {
        try
        {
            await _mediaListService.AddItemAsync(id, dto.MediaId, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (ForbiddenAccessException)
        {
            return Forbid();
        }
    }

    [HttpDelete("{id:int}/items/{mediaId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveItem(int id, int mediaId, CancellationToken cancellationToken)
    {
        try
        {
            await _mediaListService.RemoveItemAsync(id, mediaId, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (ForbiddenAccessException)
        {
            return Forbid();
        }
    }
}