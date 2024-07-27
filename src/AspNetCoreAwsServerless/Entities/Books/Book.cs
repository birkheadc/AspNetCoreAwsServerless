using Amazon.DynamoDBv2.DataModel;
using AspNetCoreAwsServerless.Utils.Id;
using Swashbuckle.AspNetCore.Annotations;

namespace AspNetCoreAwsServerless.Entities.Books;

[DynamoDBTable("Books")]
public class Book
{
  [DynamoDBHashKey]
  [DynamoDBProperty(typeof(IdPropertyConverter<Book>))]
  public required Id<Book> Id { get; init; }

  public required string Title { get; init; }
  public required string Author { get; init; }
  public required int Pages { get; init; }
}
