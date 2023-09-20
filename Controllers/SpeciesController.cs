using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NytteveksterApi.Contexts;
using NytteveksterApi.Models;

namespace NytteveksterApi.Controllers;
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class SpeciesController : ControllerBase
{
  private readonly NytteveksterContext context;

  public SpeciesController(NytteveksterContext _context)
  {
    context = _context;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<Species>>> GetSpecies()
  {
    return Ok(await context.Species.ToListAsync());
  }

}