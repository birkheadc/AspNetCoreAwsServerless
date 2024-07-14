using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;

namespace AspNetCoreAwsServerless.Converters.Books;

public interface IBooksConverter
{
  public Book ToEntity(BookCreateDto dto);
  public Book ToEntity(BookPutDto dto);
  public Book ToEntity(BookPatchDto dto, Book book);
  public BookDto ToDto(Book book);
}
