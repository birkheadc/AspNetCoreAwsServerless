namespace AspNetCoreAwsServerless.Dtos.Session;

public record LoginDto
{
  public required string Code { get; init; }
  public required string RedirectUri { get; init; }
}
