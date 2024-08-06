using System.ComponentModel.DataAnnotations;
using AspNetCoreAwsServerless.Attributes.IsValidGuid;

namespace AspNetCoreAwsServerless.Dtos.Books;

public record BookDto
{
  [Required]
  [IsValidGuid]
  public required string Id { get; init; }

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
