using System.ComponentModel.DataAnnotations;

namespace AspNetCoreAwsServerless.Dtos.Books;

public record BookCreateDto
{
  [Required]
  [MinLength(1)]
  public required string Title { get; init; }

  [Required]
  [MinLength(1)]
  public required string Author { get; init; }

  [Required]
  [Range(1, int.MaxValue)]
  public required int Pages { get; init; }
}
