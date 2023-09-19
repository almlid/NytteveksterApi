namespace NytteveksterApi.Models;

public class Type
{
  public int Id { get; set; }
  public required string Name { get; set; }
  public string? Description { get; set; }
  public required string ImagePath { get; set; }
  public ICollection<Species>? Species { get; set; }
}