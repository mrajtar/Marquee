using AutoMapper;
using Marquee.Application.DTOs.Person;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Marquee.Controllers;

[ApiController]
[Route("api/people")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;
    private readonly IMapper _mapper;

    public PersonController(IPersonService personService, IMapper mapper)
    {
        _personService = personService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PersonListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var people = await _personService.GetAllAsync(cancellationToken);
        var result = _mapper.Map<List<PersonListDto>>(people);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PersonDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var person = await _personService.GetByIdWithDetailsAsync(id, cancellationToken);
        if (person == null) return NotFound(new { message = $"Person with id {id} not found." });
        var result = _mapper.Map<PersonDetailsDto>(person);
        return Ok(result);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyList<PersonListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search([FromQuery] string searchTerm, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return BadRequest(new { message = "Search term is required." });
        var people = await _personService.SearchAsync(searchTerm, cancellationToken);
        var result = _mapper.Map<List<PersonListDto>>(people);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PersonDetailsDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] CreatePersonDto dto, CancellationToken cancellationToken)
    {
        var person = _mapper.Map<Person>(dto);
        try
        {
            var created = await _personService.AddAsync(person, cancellationToken);
            var result = _mapper.Map<PersonDetailsDto>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePersonDto dto, CancellationToken cancellationToken)
    {
        var person = _mapper.Map<Person>(dto);
        person.Id = id;
        try
        {
            await _personService.UpdateAsync(person, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _personService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}