using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Utils.Id;

namespace AspNetCoreAwsServerless.Repositories.Books;

public class BooksRepository : IBooksRepository
{
  public Task Delete(Id<Book> id)
  {
    throw new NotImplementedException();
  }

  public Task<Book> Get(Id<Book> id)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<Book>> GetAll()
  {
    throw new NotImplementedException();
  }

  public Task<Book> Put(Book book)
  {
    throw new NotImplementedException();
  }
}
