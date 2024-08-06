using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Utils.Paginated;

namespace AspNetCoreAwsServerless.Converters.Books;

public interface IBooksConverter
{
  public Book ToEntity(BookCreateDto dto);
  public Book ToEntity(BookPutDto dto);
  public Book ToEntity(BookPatchDto dto, Book book);
  public BookDto ToDto(Book book);
  public List<BookDto> ToDto(List<Book> books);
  public Paginated<BookDto> ToDto(Paginated<Book> books);
}
