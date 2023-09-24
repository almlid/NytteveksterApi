namespace NytteveksterApi.Dtos;

public class ListResponseDTO<T>
{
  public int Count { get; set; }
  public required List<T> Results { get; set; }
}