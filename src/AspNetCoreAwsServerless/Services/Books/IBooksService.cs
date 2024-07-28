using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Services.Books;

public interface IBooksService
{
  public Task<ApiResult<Book>> Get(Id<Book> id);
  public Task<ApiResult<List<Book>>> GetAll();
  public Task<ApiResult<Book>> Create(BookCreateDto dto);
  public Task<ApiResult<Book>> Put(BookPutDto dto);
  public Task<ApiResult<Book>> Patch(Id<Book> id, BookPatchDto dto);
  public Task<ApiResult> Delete(Id<Book> id);
}
