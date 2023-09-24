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

}