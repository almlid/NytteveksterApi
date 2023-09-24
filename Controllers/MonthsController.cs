using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NytteveksterApi.Contexts;
using NytteveksterApi.Dtos;

namespace NytteveksterApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class MonthsController : ControllerBase
{
  private readonly NytteveksterContext context;
  private readonly IHttpContextAccessor httpContextAccessor;
  private readonly string apiVersion;

  public MonthsController(NytteveksterContext _context, IHttpContextAccessor _httpContextAccessor)
  {
    context = _context;
    httpContextAccessor = _httpContextAccessor;
    apiVersion = _httpContextAccessor.HttpContext?.GetRequestedApiVersion()?.ToString() ?? "1.0";
  }

  [HttpGet]
  public async Task<ActionResult<ListResponseDTO<MonthlyAvailabilityDto>>> GetMonths()
  {
    try
    {
      var allSpeciesAvailability = await context.SpeciesAvailability.ToListAsync();
      if (allSpeciesAvailability == null)
      {
        return NotFound();
      }

      ICollection<string> Months = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
      List<MonthlyAvailabilityDto> monthDtoList = new List<MonthlyAvailabilityDto>();

      foreach (var month in Months)
      {
        var monthDto = new MonthlyAvailabilityDto
        {
          Month = month,
          SpeciesCount = allSpeciesAvailability
                .Count(sa => (bool)sa
                  .GetType()
                  .GetProperty(month)
                  .GetValue(sa)),
          SpeciesUrl = $"/api/v{apiVersion}/species/month/{month}"
        };

        monthDtoList.Add(monthDto);
      }

      var dto = new ListResponseDTO<MonthlyAvailabilityDto>
      {
        Count = monthDtoList.Count,
        Results = monthDtoList
      };

      return Ok(dto);

    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

}

