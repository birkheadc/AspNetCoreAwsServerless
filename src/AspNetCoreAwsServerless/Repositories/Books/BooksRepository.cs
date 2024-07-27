using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Utils.Id;

namespace AspNetCoreAwsServerless.Repositories.Books;

public class BooksRepository(IDynamoDBContext context) : IBooksRepository
{
  private readonly IDynamoDBContext _context = context;

  public async Task Delete(Id<Book> id)
  {
    throw new NotImplementedException();
  }

  public async Task<Book> Get(Id<Book> id)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<Book>> GetAll()
  {
    try
    {
      List<ScanCondition> conditions = [];
      return await _context.ScanAsync<Book>(conditions).GetRemainingAsync();
    }
    catch (Exception exception)
    {
      // Todo: Logging
      Console.WriteLine($"Failed to fetch all books. {exception}");
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
      // Todo: Logging
      Console.WriteLine($"Failed to put book. {book.Id} {exception}");
      throw new InternalServerErrorException("");
    }
  }
}
