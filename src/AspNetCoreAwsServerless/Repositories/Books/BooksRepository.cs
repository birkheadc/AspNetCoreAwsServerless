using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Utils.Id;

namespace AspNetCoreAwsServerless.Repositories.Books;

public class BooksRepository(IDynamoDBContext context, ILogger<BooksRepository> logger)
  : IBooksRepository
{
  private readonly IDynamoDBContext _context = context;
  private readonly ILogger<BooksRepository> _logger = logger;

  public async Task Delete(Id<Book> id)
  {
    _logger.LogInformation("Delete Book Id: ${id}", id);
    throw new NotImplementedException();
  }

  public async Task<Book> Get(Id<Book> id)
  {
    _logger.LogInformation("Get Book Id: ${id}", id);
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<Book>> GetAll()
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
      _logger.LogError("Failed to fetch all books. {exception}", exception);
      throw new InternalServerErrorException("");
    }
  }

  public async Task<Book> Put(Book book)
  {
    try
    {
      await _context.SaveAsync(book);
      return book;
    }
    catch (Exception exception)
    {
      _logger.LogError("Failed to put book {book}. {exception}", book, exception);
      throw new InternalServerErrorException("");
    }
  }
}
