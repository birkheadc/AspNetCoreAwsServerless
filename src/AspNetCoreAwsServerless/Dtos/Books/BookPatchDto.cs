using System.ComponentModel.DataAnnotations;

namespace AspNetCoreAwsServerless.Dtos.Books;

public record BookPatchDto
{
  [MinLength(1)]
  public string? Title { get; init; }

  [MinLength(1)]
  public string? Author { get; init; }

  [Range(1, int.MaxValue)]
  public int? Pages { get; init; }
}
