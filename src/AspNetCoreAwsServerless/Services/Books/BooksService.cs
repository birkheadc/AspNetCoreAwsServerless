using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Repositories.Books;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Paginated;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Services.Books;

public class BooksService(IBooksRepository booksRepository, IBooksConverter booksConverter)
  : IBooksService
{
  private readonly IBooksRepository _booksRepository = booksRepository;
  private readonly IBooksConverter _booksConverter = booksConverter;

  public async Task<ApiResult<Book>> Create(BookCreateDto dto)
  {
    Book book = _booksConverter.ToEntity(dto);
    return await _booksRepository.Put(book);
  }

  public async Task<ApiResult> CreateMany(BookCreateManyDto dto)
  {
    List<Book> books = [];
    foreach (BookCreateDto book in dto.Books)
    {
      books.Add(_booksConverter.ToEntity(book));
    }
    return await _booksRepository.PutMany(books);
  }

  public async Task<ApiResult> Delete(Id<Book> id)
  {
    return await _booksRepository.Delete(id);
  }

  public async Task<ApiResult<Book>> Get(Id<Book> id)
  {
    return await _booksRepository.Get(id);
  }

  public async Task<ApiResult<List<Book>>> GetAll()
  {
    return await _booksRepository.GetAll();
  }

  public async Task<ApiResult<Paginated<Book>>> GetPage(string? paginationToken)
  {
    return await _booksRepository.GetPage(paginationToken);
  }

  public async Task<ApiResult<Book>> Patch(Id<Book> id, BookPatchDto dto)
  {
    ApiResult<Book> result = await _booksRepository.Get(id);
    if (result.IsFailure)
    {
      return result;
    }
    Book newBook = _booksConverter.ToEntity(dto, result.Value);
    return await _booksRepository.Put(newBook);
  }

  public async Task<ApiResult<Book>> Put(BookPutDto dto)
  {
    Book book = _booksConverter.ToEntity(dto);
    return await _booksRepository.Put(book);
  }
}
