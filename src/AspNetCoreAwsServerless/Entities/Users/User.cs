using Amazon.DynamoDBv2.DataModel;
using AspNetCoreAwsServerless.Utils.Id;

namespace AspNetCoreAwsServerless.Entities.Users;

[DynamoDBTable("Users")]
public class User
{
  [DynamoDBHashKey]
  [DynamoDBProperty(typeof(IdPropertyConverter<User>))]
  public required Id<User> Id { get; set; }
  public required string EmailAddress { get; set; }
  public string? DisplayName { get; set; }
}
