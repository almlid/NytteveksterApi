using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NytteveksterApi.Contexts;
using NytteveksterApi.Models;
using NytteveksterApi.Dtos;
using System.Globalization;

namespace NytteveksterApi.Controllers;
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class SpeciesController : ControllerBase
{
  private readonly NytteveksterContext context;
  private readonly IHttpContextAccessor httpContextAccessor;
  private readonly string apiVersion;


  public SpeciesController(NytteveksterContext _context, IHttpContextAccessor _httpContextAccessor)
  {
    context = _context;
    httpContextAccessor = _httpContextAccessor;
    apiVersion = _httpContextAccessor.HttpContext?.GetRequestedApiVersion()?.ToString() ?? "1.0";
  }


  [HttpGet]
  public async Task<ActionResult<ListResponseDTO<SpeciesSimpleDto>>> GetSpecies()
  {
    try
    {
      List<Species> speciesList = await context.Species
            .Include(t => t.Type)
            .Include(a => a.Availability)
            .ToListAsync();

      var speciesDtoList = speciesList.Select(s => new SpeciesSimpleDto
      {
        Id = s.Id,
        CommonName = s.CommonName,
        Url = $"/api/v{apiVersion}/species/{s.Id}"

      }).ToList();

      var dto = new ListResponseDTO<SpeciesSimpleDto>
      {
        Count = speciesList.Count,
        Results = speciesDtoList

      };
      return Ok(dto);

    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<SpeciesDto>> GetById(int id)
  {
    try
    {
      Species? species = await context.Species.Include(t => t.Type).Include(a => a.Availability).FirstOrDefaultAsync(s => s.Id == id);

      if (species != null)
      {
        var dto = new SpeciesDto
        {
          Id = species.Id,
          CommonName = species.CommonName,
          Description = species.Description,
          ImagePath = species.ImagePath,
          ScientificName = species.ScientificName,

          Type = new TypeSimpleDto()
          {
            Id = species.Type.Id,
            Name = species.Type.Name,
            Url = $"/api/v{apiVersion}/types/{species.Type.Id}",
          },

          Availability = new SpeciesAvailabilityDto()
          {
            January = species.Availability.January,
            February = species.Availability.February,
            March = species.Availability.March,
            April = species.Availability.April,
            May = species.Availability.May,
            June = species.Availability.June,
            July = species.Availability.July,
            August = species.Availability.August,
            September = species.Availability.September,
            October = species.Availability.October,
            November = species.Availability.November,
            December = species.Availability.December
          },
        };
        return Ok(dto);
      }
      return NotFound();
    }
    catch (Exception e)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
    }
  }


  [HttpGet("Type/{type}")]
  public async Task<ActionResult<ListResponseDTO<SpeciesSimpleDto>>> GetByType(string? type)
  {

    try
    {
      if (type != null)
      {
        List<Species> speciesList = await context.Species
              .Where(s => s.Type.Name.ToLower().Contains(type.ToLower()))
              .Include(t => t.Type)
              .Include(a => a.Availability).ToListAsync();

        var speciesDtoList = speciesList.Select(s => new SpeciesSimpleDto
        {
          Id = s.Id,
          CommonName = s.CommonName,
          Url = $"/api/v{apiVersion}/species/{s.Id}"

        }).ToList();

        var dto = new ListResponseDTO<SpeciesSimpleDto>
        {
          Count = speciesList.Count,
          Results = speciesDtoList

        };
        return Ok(dto);

      }
      return NotFound();
    }
    catch
    {
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpGet("Month/{month}")]
  public async Task<ActionResult<ListResponseDTO<SpeciesSimpleDto>>> GetByMonth(string? month)
  {
    try
    {
      if (month != null)
      {
        string validMonth = CultureInfo.CurrentCulture.DateTimeFormat
              .GetMonthName(DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month);

        var speciesList = context.Species
              .Include(t => t.Type)
              .Include(a => a.Availability)
              .AsEnumerable()
              .Where(s => (bool)s.Availability
                .GetType()
                .GetProperty(validMonth)
                .GetValue(s.Availability))
              .ToList();

        var speciesDtoList = speciesList.Select(s => new SpeciesSimpleDto
        {
          Id = s.Id,
          CommonName = s.CommonName,
          Url = $"/api/v{apiVersion}/species/{s.Id}"
        }).ToList();

        var dto = new ListResponseDTO<SpeciesSimpleDto>
        {
          Count = speciesList.Count,
          Results = speciesDtoList
        };
        return Ok(dto);
      }
      return NotFound();
    }
    catch (FormatException ex)
    {
      return BadRequest("Invalid month format.");
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
    }
  }


}