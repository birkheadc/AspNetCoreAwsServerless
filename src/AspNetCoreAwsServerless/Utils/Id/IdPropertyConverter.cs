using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace AspNetCoreAwsServerless.Utils.Id;

public class IdPropertyConverter<T> : IPropertyConverter
{
  public object FromEntry(DynamoDBEntry entry)
  {
    Primitive primitive =
      entry as Primitive
      ?? throw new InternalServerErrorException("Entry as primitive was null I guess");
    if (
      primitive == null
      || primitive.Value is not String
      || string.IsNullOrEmpty((string)primitive.Value)
    )
      throw new InternalServerErrorException("The other thing happened");

    Id<T> id = new((string)primitive.Value);
    return id;
  }

  public DynamoDBEntry ToEntry(object value)
  {
    return new Primitive() { Value = value.ToString() };
  }
}
