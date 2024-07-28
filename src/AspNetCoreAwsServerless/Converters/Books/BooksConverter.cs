using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Utils.Id;

namespace AspNetCoreAwsServerless.Converters.Books;

public class BooksConverter : IBooksConverter
{
  public Book ToEntity(BookCreateDto dto)
  {
    Id<Book> id = new();
    Book book =
      new()
      {
        Id = id,
        Title = dto.Title,
        Author = dto.Author,
        Pages = dto.Pages
      };
    return book;
  }

  public Book ToEntity(BookPutDto dto)
  {
    Book book =
      new()
      {
        Id = new(dto.Id),
        Title = dto.Title,
        Author = dto.Author,
        Pages = dto.Pages
      };
    return book;
  }

  public Book ToEntity(BookPatchDto dto, Book book)
  {
    Book newBook =
      new()
      {
        Id = book.Id,
        Title = dto.Title ?? book.Title,
        Author = dto.Author ?? book.Author,
        Pages = dto.Pages ?? book.Pages
      };
    return newBook;
  }

  public BookDto ToDto(Book book)
  {
    BookDto dto =
      new()
      {
        Id = book.Id.ToString(),
        Title = book.Title,
        Author = book.Author,
        Pages = book.Pages
      };
    return dto;
  }

  public List<BookDto> ToDto(List<Book> books)
  {
    List<BookDto> dtos = [];
    foreach (Book book in books)
    {
      dtos.Add(ToDto(book));
    }
    return dtos;
  }
}
