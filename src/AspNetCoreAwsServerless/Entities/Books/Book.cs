using AspNetCoreAwsServerless.Utils.Id;

namespace AspNetCoreAwsServerless.Entities.Books;

public class Book
{
  public required Id<Book> Id { get; init; }
  public required string Title { get; init; }
  public required string Author { get; init; }
  public required int Pages { get; init; }
}
