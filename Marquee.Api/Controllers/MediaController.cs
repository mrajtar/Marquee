using System.ComponentModel.DataAnnotations;
using Marquee.Application.Interfaces.Repositories;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marquee.Controllers;
[ApiController]
[Route("api/media")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Media>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var media = await _mediaService.GetAllAsync(cancellationToken);
        return Ok(media);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Media), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var media = await _mediaService.GetByIdAsync(id, cancellationToken);
        
        if (media == null)
            return NotFound(new {message = $"Media with ID {id} not found"});
        
        return Ok(media);
    }

    [HttpGet("{id:int}/details")]
    [ProducesResponseType(typeof(Media), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdWithDetails(int id, CancellationToken cancellationToken)
    {
        var media = await _mediaService.GetByIdWithDetails(id, cancellationToken);

        if (media == null)
            return NotFound(new { message = $"Media with ID {id} not found" });

        return Ok(media);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyList<Media>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search([FromQuery] string searchTerm, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest(new { message = "Search term is required" });
        
        var results = await _mediaService.SearchAsync(searchTerm, cancellationToken);
        
        return Ok(results);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Media), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] Media media, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _mediaService.AddAsync(media, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new {ex.Message});
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] Media media, CancellationToken cancellationToken)
    {
        if (id != media.Id)
            return BadRequest(new { message = "ID mismatch" });
        
        try
        {
            await _mediaService.UpdateAsync(media, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new  { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
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