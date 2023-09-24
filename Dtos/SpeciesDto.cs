namespace NytteveksterApi.Dtos;

public class SpeciesDto
{
  public int Id { get; set; }
  public string CommonName { get; set; }
  public string ScientificName { get; set; }
  public string ImagePath { get; set; }
  public string Description { get; set; }
  public TypeSimpleDto Type { get; set; }
  public SpeciesAvailabilityDto Availability { get; set; }
}