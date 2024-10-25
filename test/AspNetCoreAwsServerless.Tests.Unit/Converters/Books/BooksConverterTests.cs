using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Utils.Id;
using FluentAssertions;
using Moq;

namespace AspNetCoreAwsServerless.Tests.Unit.Converters.Books;

public class BooksConverterTests
{
  private readonly BooksConverter _converter;

  public BooksConverterTests()
  {
    _converter = new();
  }

  [Fact]
  public void ToEntity_WhenBookCreateDto_ReturnsCorrectEntity()
  {
    BookCreateDto dto =
      new()
      {
        Title = "New Book Title",
        Author = "New Author",
        Pages = 125
      };

    Book expected =
      new()
      {
        Id = It.IsAny<Id<Book>>(),
        Title = dto.Title,
        Author = dto.Author,
        Pages = dto.Pages
      };

    Book actual = _converter.ToEntity(dto);

    actual.Should().BeEquivalentTo(expected, options => options.Excluding(x => x.Id));
  }

  [Fact]
  public void ToEntity_WhenBookPutDto_ReturnsCorrectEntity()
  {
    Id<Book> id = new();
    BookPutDto dto =
      new()
      {
        Id = id.ToString(),
        Title = "New Book Title",
        Author = "New Author",
        Pages = 125
      };

    Book expected =
      new()
      {
        Id = id,
        Title = dto.Title,
        Author = dto.Author,
        Pages = dto.Pages
      };

    Book actual = _converter.ToEntity(dto);

    actual.Should().BeEquivalentTo(expected);
  }

  [Fact]
  public void ToEntity_WhenBookPatchDto_ReturnsCorrectEntity()
  {
    Id<Book> id = new();
    BookPatchDto dto = new() { Title = "New Book Title", };

    Book oldBook =
      new()
      {
        Id = id,
        Title = "Old Book Title",
        Author = "Author",
        Pages = 135
      };

    Book expected =
      new()
      {
        Id = oldBook.Id,
        Title = dto.Title,
        Author = oldBook.Author,
        Pages = oldBook.Pages
      };

    Book actual = _converter.ToEntity(dto, oldBook);

    actual.Should().BeEquivalentTo(expected);
  }
}
