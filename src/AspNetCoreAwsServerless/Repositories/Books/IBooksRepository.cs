using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Utils.Id;

namespace AspNetCoreAwsServerless.Repositories.Books;

public interface IBooksRepository
{
  public Task<Book> Get(Id<Book> id);
  public Task<IEnumerable<Book>> GetAll();
  public Task<Book> Put(Book book);
  public Task Delete(Id<Book> id);
}
