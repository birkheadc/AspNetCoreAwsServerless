using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using AspNetCoreAwsServerless.Config.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Paginated;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.Extensions.Options;

namespace AspNetCoreAwsServerless.Repositories.Books;

public class BooksRepository(
  IOptions<BooksOptions> config,
  IDynamoDBContext context,
  ILogger<BooksRepository> logger
) : IBooksRepository
{
  private readonly IDynamoDBContext _context = context;
  private readonly ILogger<BooksRepository> _logger = logger;
  private readonly BooksOptions _config = config.Value;

  public async Task<ApiResult> Delete(Id<Book> id)
  {
    _logger.LogInformation("Delete Book Id: ${id}", id);
    try
    {
      await _context.DeleteAsync<Book>(id);
      _logger.LogInformation("Deleted book Id: ${id} (Book may or may not have been present!)", id);
      return ApiResult.Success();
    }
    catch (Exception exception)
    {
      _logger.LogWarning("Failed to delete book Id: {id}. {exception}", id, exception);
      return ApiResultErrors.InternalServerError;
    }
  }

  public async Task<ApiResult<Book>> Get(Id<Book> id)
  {
    _logger.LogInformation("Get book Id: ${id}", id);
    try
    {
      Book? book = await _context.LoadAsync<Book>(id);
      if (book is null)
      {
        _logger.LogWarning("Failed to get book Id: {id}. Id not present in database.", id);
        return ApiResultErrors.NotFound;
      }
      _logger.LogInformation("Found book Id: {id}", id);
      return book;
    }
    catch (Exception exception)
    {
      _logger.LogWarning("Failed to get book Id: {id}. {exception}", id, exception);
      return ApiResultErrors.InternalServerError;
    }
  }

  public async Task<ApiResult<List<Book>>> GetAll()
  {
    _logger.LogInformation("Getting all books...");
    try
    {
      List<ScanCondition> conditions = [];

      List<Book> books = await _context.ScanAsync<Book>(conditions).GetRemainingAsync();
      _logger.LogInformation("Found all books. Count: {count}", books.Count);

      return books;
    }
    catch (Exception exception)
    {
      _logger.LogCritical("Failed to get all books. {exception}", exception);
      return ApiResultErrors.InternalServerError;
    }
  }

  public async Task<ApiResult<Paginated<Book>>> GetPage(string? paginationToken = null)
  {
    _logger.LogInformation(
      "Getting a page of books. PaginationToken: {paginationToken}",
      paginationToken ?? "null"
    );
    try
    {
      ScanOperationConfig scan =
        new() { Limit = _config.PageSize, PaginationToken = paginationToken };

      AsyncSearch<Book> result = _context.FromScanAsync<Book>(scan);

      Paginated<Book> books =
        new() { Values = await result.GetNextSetAsync(), PaginationToken = result.PaginationToken };

      _logger.LogInformation(
        "Found page of books. Count: {count}, PaginationToken: {paginationToken}",
        books.Values.Count,
        books.PaginationToken != "{}" ? books.PaginationToken : null
      );

      return books;
    }
    catch (Exception exception)
    {
      _logger.LogError("Failed to get page of books. {exception}", exception);
      return ApiResultErrors.InternalServerError;
    }
  }

  public async Task<ApiResult<Book>> Put(Book book)
  {
    _logger.LogInformation("Putting book ${id}...", book.Id);
    try
    {
      await _context.SaveAsync(book);
      _logger.LogInformation("Successfully put book ${id}", book.Id);
      return ApiResult.Success(book);
    }
    catch (Exception exception)
    {
      _logger.LogCritical("Failed to put book {book}. {exception}", book, exception);
      return ApiResultErrors.InternalServerError;
    }
  }

  public async Task<ApiResult> PutMany(List<Book> books)
  {
    _logger.LogInformation("Attempting to put {num} books", books.Count);
    try
    {
      BatchWrite<Book> batchWrite = _context.CreateBatchWrite<Book>();
      batchWrite.AddPutItems(books);

      await batchWrite.ExecuteAsync();

      _logger.LogInformation("Successfully put {num} books", books.Count);
      return ApiResult.Success();
    }
    catch (Exception exception)
    {
      _logger.LogCritical("Failed to create many books. {exception}", exception);
      return ApiResultErrors.InternalServerError;
    }
  }
}
