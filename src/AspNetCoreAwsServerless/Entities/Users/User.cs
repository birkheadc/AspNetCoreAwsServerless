using Amazon.DynamoDBv2.DataModel;
using AspNetCoreAwsServerless.Utils.Id;

namespace AspNetCoreAwsServerless.Entities.Users;

[DynamoDBTable("Users")]
public class User
{
  [DynamoDBHashKey]
  [DynamoDBProperty(typeof(IdPropertyConverter<User>))]
  public required Id<User> Id { get; init; }
  public required string EmailAddress { get; init; }
  public string? DisplayName { get; init; }
}
