using AutoMapper;
using Marquee.Application.DTOs.Genre;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Marquee.Controllers;

[ApiController]
[Route("api/genres")]
public class GenreController : ControllerBase
{
    private readonly IGenreService _genreService;
    private readonly IMapper _mapper;

    public GenreController(IGenreService genreService, IMapper mapper)
    {
        _genreService = genreService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<GenreListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var genres = await _genreService.GetAllAsync(cancellationToken);
        var result = _mapper.Map<List<GenreListDto>>(genres);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GenreDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var genre = await _genreService.GetByIdAsync(id, cancellationToken);
        if (genre is null)
            return NotFound(new { message = $"Genre with ID {id} not found." });

        var result = _mapper.Map<GenreDetailsDto>(genre);
        return Ok(result);
    }
    
    [HttpGet("{id:int}/details")]
    [ProducesResponseType(typeof(GenreDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdWithDetails(int id, CancellationToken cancellationToken)
    {
        var genre = await _genreService.GetByIdWithDetailsAsync(id, cancellationToken);
        if (genre == null)
            return NotFound(new { message = $"Genre with ID {id} not found" });
        
        var result = _mapper.Map<GenreDetailsDto>(genre);
        return Ok(result);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyList<GenreListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search([FromQuery] string searchTerm, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest(new { message = "Search term is required." });

        var genres = await _genreService.SearchAsync(searchTerm, cancellationToken);
        var result = _mapper.Map<List<GenreListDto>>(genres);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GenreDetailsDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Add([FromBody] CreateGenreDto dto, CancellationToken cancellationToken)
    {
        var genre = _mapper.Map<Genre>(dto);
        try
        {
            var created = await _genreService.AddAsync(genre, cancellationToken);
            var result = _mapper.Map<GenreDetailsDto>(created);
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateGenreDto dto, CancellationToken cancellationToken)
    {
        var genre = _mapper.Map<Genre>(dto);
        genre.Id = id;
        try
        {
            await _genreService.UpdateAsync(genre, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _genreService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}