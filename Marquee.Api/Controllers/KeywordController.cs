using AutoMapper;
using Marquee.Application.DTOs.Keyword;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marquee.Controllers;

[ApiController]
[Route("api/keywords")]
public class KeywordController : ControllerBase
{
    private readonly IKeywordService _keywordService;
    private readonly IMapper _mapper;

    public KeywordController(IKeywordService keywordService, IMapper mapper)
    {
        _keywordService = keywordService;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<KeywordListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var keywords = await _keywordService.GetAllAsync(cancellationToken);
        var result = _mapper.Map<List<KeywordListDto>>(keywords);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(KeywordDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var keyword = await _keywordService.GetByIdAsync(id, cancellationToken);
        if (keyword is null)
        {
            return NotFound(new { message = $"Keyword with ID {id} not found." });
        }

        var result = _mapper.Map<KeywordDetailsDto>(keyword);
        return Ok(result);
    }

    [HttpGet("search")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<KeywordListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search([FromQuery] string searchTerm, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest(new { message = "Search term is required." });
        }

        var keywords = await _keywordService.SearchAsync(searchTerm, cancellationToken);
        var result = _mapper.Map<List<KeywordListDto>>(keywords);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(KeywordDetailsDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Add([FromBody] CreateKeywordDto dto, CancellationToken cancellationToken)
    {
        var keyword = _mapper.Map<Keyword>(dto);
        try
        {
            var created = await _keywordService.AddAsync(keyword, cancellationToken);
            var result = _mapper.Map<KeywordDetailsDto>(created);
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
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateKeywordDto dto,
        CancellationToken cancellationToken)
    {
        var keyword = _mapper.Map<Keyword>(dto);
        keyword.Id = id;
        try
        {
            await _keywordService.UpdateAsync(keyword, cancellationToken);
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
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _keywordService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}