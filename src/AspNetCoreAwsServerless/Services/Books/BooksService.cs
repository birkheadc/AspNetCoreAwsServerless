using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Repositories.Books;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Services.Books;

public class BooksService(IBooksRepository repository, IBooksConverter converter) : IBooksService
{
  private readonly IBooksRepository _repository = repository;
  private readonly IBooksConverter _converter = converter;

  public async Task<ApiResult<Book>> Create(BookCreateDto dto)
  {
    Book book = _converter.ToEntity(dto);
    return await _repository.Put(book);
  }

  public async Task<ApiResult> CreateMany(BookCreateManyDto dto)
  {
    List<Book> books = [];
    foreach (BookCreateDto book in dto.Books)
    {
      books.Add(_converter.ToEntity(book));
    }
    return await _repository.PutMany(books);
  }

  public async Task<ApiResult> Delete(Id<Book> id)
  {
    return await _repository.Delete(id);
  }

  public async Task<ApiResult<Book>> Get(Id<Book> id)
  {
    return await _repository.Get(id);
  }

  public async Task<ApiResult<List<Book>>> GetAll()
  {
    return await _repository.GetAll();
  }

  public async Task<ApiResult<Book>> Patch(Id<Book> id, BookPatchDto dto)
  {
    ApiResult<Book> result = await _repository.Get(id);
    if (result.IsFailure)
    {
      return result;
    }
    Book newBook = _converter.ToEntity(dto, result.Value);
    return await _repository.Put(newBook);
  }

  public async Task<ApiResult<Book>> Put(BookPutDto dto)
  {
    Book book = _converter.ToEntity(dto);
    return await _repository.Put(book);
  }
}
