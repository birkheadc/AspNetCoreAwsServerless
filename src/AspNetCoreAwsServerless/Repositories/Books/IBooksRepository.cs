using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Paginated;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Repositories.Books;

public interface IBooksRepository
{
  public Task<ApiResult<Book>> Get(Id<Book> id);
  public Task<ApiResult<List<Book>>> GetAll();
  public Task<ApiResult<Paginated<Book>>> GetPage(string? paginationToken = null);
  public Task<ApiResult<Book>> Put(Book book);
  public Task<ApiResult> PutMany(List<Book> books);
  public Task<ApiResult> Delete(Id<Book> id);
}
