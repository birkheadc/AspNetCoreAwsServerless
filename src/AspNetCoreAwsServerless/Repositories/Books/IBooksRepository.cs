using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Repositories.Books;

public interface IBooksRepository
{
  public Task<ApiResult<Book>> Get(Id<Book> id);
  public Task<ApiResult<IEnumerable<Book>>> GetAll();
  public Task<ApiResult<Book>> Put(Book book);
  public Task<ApiResult> Delete(Id<Book> id);
}
