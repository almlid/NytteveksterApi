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
public class TypesController : ControllerBase
{
  private readonly NytteveksterContext context;
  private readonly IHttpContextAccessor httpContextAccessor;
  private readonly string apiVersion;

  public TypesController(NytteveksterContext _context, IHttpContextAccessor _httpContextAccessor)
  {
    context = _context;
    httpContextAccessor = _httpContextAccessor;
    apiVersion = _httpContextAccessor.HttpContext?.GetRequestedApiVersion()?.ToString() ?? "1.0";
  }

  [HttpGet]
  public async Task<ActionResult<ListResponseDTO<TypeSimpleDto>>> GetTypes()
  {
    try
    {
      var typeDtoList = await context.Types.Select(t => new TypeSimpleDto
      {
        Id = t.Id,
        Name = t.Name,
        Url = $"/api/v{apiVersion}/types/{t.Id}"
      }).ToListAsync();

      if (typeDtoList == null)
      {
        return NotFound();
      }

      var dto = new ListResponseDTO<TypeSimpleDto>
      {
        Count = typeDtoList.Count,
        Results = typeDtoList
      };
      return Ok(dto);

    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<TypeDto>> GetById(int id)
  {
    try
    {
      var type = await context.Types.Include(t => t.Species).FirstOrDefaultAsync(t => t.Id == id);

      if (type != null)
      {
        var dto = new TypeDto
        {

          Id = type.Id,
          Name = type.Name,
          Description = type.Description,
          ImagePath = type.ImagePath,
          SpeciesCount = type.Species?.Count ?? 0,
          SpeciesUrl = $"/api/v{apiVersion}/species/type/{type.Name}",
        };
        return Ok(dto);

      }
      return NotFound();

    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }
}
