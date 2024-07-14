namespace AspNetCoreAwsServerless.Dtos.Books;

public record BookCreateDto
{
  public required string Title { get; init; }
  public required string Author { get; init; }
  public required int Pages { get; init; }
}
