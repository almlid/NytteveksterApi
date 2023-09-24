namespace NytteveksterApi.Dtos;

public class TypeDto
{
  public int Id { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public string ImagePath { get; set; }
  public int SpeciesCount { get; set; }
  public string SpeciesUrl { get; set; }
}