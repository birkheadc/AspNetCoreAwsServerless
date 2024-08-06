namespace AspNetCoreAwsServerless.Dtos.Sums;

public record SumCreateDto
{
  public required int[] Values { get; init; }
}
