namespace AspNetCoreAwsServerless.Dtos.Example;

public record ExampleDto
{
  public required string DisplayName { get; init; }

  public required string Password { get; init; }

  public required string SecretCode { get; init; }
}
