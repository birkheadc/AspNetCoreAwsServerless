using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Utils.Id;

namespace AspNetCoreAwsServerless.Services.Books;

public interface IBooksService
{
  public Task<Book> Get(Id<Book> id);
  public Task<IEnumerable<Book>> GetAll();
  public Task<Book> Create(BookCreateDto dto);
  public Task<Book> Put(BookPutDto dto);
  public Task<Book> Patch(Id<Book> id, BookPatchDto dto);
  public Task Delete(Id<Book> id);
}
