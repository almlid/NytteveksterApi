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
}
