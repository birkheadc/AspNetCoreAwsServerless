using Amazon.DynamoDBv2.DataModel;
using AspNetCoreAwsServerless.Entities.Roles;
using AspNetCoreAwsServerless.Utils.Id;

namespace AspNetCoreAwsServerless.Entities.Users;

[DynamoDBTable("Users")]
public record User
{
  [DynamoDBHashKey]
  [DynamoDBProperty(typeof(IdPropertyConverter<User>))]
  public required Id<User> Id { get; set; }
  public required string EmailAddress { get; set; }
  public required UserProfile Profile { get; set; }
  public UserRole[] Roles { get; set; } = [];
}
