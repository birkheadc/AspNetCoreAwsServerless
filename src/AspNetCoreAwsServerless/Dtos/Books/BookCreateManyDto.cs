namespace AspNetCoreAwsServerless.Dtos.Books;

public class BookCreateManyDto
{
  public required List<BookCreateDto> Books { get; init; }
}
