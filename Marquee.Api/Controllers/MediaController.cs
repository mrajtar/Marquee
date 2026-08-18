using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Marquee.Application.DTOs.Media;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Marquee.Controllers;
[ApiController]
[Route("api/media")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;
    private readonly IMapper _mapper;

    public MediaController(IMediaService mediaService, IMapper mapper)
    {
        _mediaService = mediaService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MediaListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var media = await _mediaService.GetAllAsync(cancellationToken);
        var result = _mapper.Map<IReadOnlyList<MediaListDto>>(media);
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MediaListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var media = await _mediaService.GetByIdAsync(id, cancellationToken);
        
        if (media == null)
            return NotFound(new {message = $"Media with ID {id} not found"});
        
        var result = _mapper.Map<MediaListDto>(media);
        return Ok(result);
    }

    [HttpGet("{id:int}/details")]
    [ProducesResponseType(typeof(MediaDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdWithDetails(int id, CancellationToken cancellationToken)
    {
        var media = await _mediaService.GetByIdWithDetails(id, cancellationToken);

        if (media == null)
            return NotFound(new { message = $"Media with ID {id} not found" });

        var result = _mapper.Map<MediaDetailsDto>(media);
        return Ok(result);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyList<MediaListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search([FromQuery] string searchTerm, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest(new { message = "Search term is required" });
        
        var media = await _mediaService.SearchAsync(searchTerm, cancellationToken);
        
        var result = _mapper.Map<IReadOnlyList<MediaListDto>>(media);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MediaDetailsDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] CreateMediaDto dto, CancellationToken cancellationToken)
    {
        var media = _mapper.Map<Media>(dto);
        var created = await _mediaService.AddAsync(
            media,
            dto.GenreIds,
            dto.KeywordIds,
            cancellationToken);
        var result = _mapper.Map<MediaDetailsDto>(created);
        return CreatedAtAction(nameof(GetByIdWithDetails), new { id = created.Id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMediaDto dto, CancellationToken cancellationToken)
    {
        var media = _mapper.Map<Media>(dto);
        media.Id = id;
        
        try
        {
            await _mediaService.UpdateAsync(
                media,
                dto.GenreIds,
                dto.KeywordIds,
                cancellationToken);
            
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new  { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _mediaService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}