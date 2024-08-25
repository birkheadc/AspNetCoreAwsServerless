using System.ComponentModel.DataAnnotations;

namespace AspNetCoreAwsServerless.Dtos.Books;

public record BookPatchDto
{
  public string? Title { get; init; }

  public string? Author { get; init; }

  public int? Pages { get; init; }
}
