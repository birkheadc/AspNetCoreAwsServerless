namespace AspNetCoreAwsServerless.Dtos.Books;

public record BookDto
{
  public required string Id { get; init; }

  public required string Title { get; init; }

  public required string Author { get; init; }

  public required int Pages { get; init; }
}
