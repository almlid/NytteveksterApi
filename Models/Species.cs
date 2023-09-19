
namespace NytteveksterApi.Models;

public class Species
{
  public int Id { get; set; }
  public required string CommonName { get; set; }
  public string? ScientificName { get; set; }
  public required Type Type { get; set; }
  public SpeciesAvailability? Availability { get; set; }
  public string? Description { get; set; }
  public required string ImagePath { get; set; }
}