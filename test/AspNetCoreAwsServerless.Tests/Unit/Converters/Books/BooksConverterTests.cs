using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Utils.Id;
using Moq;
using Xunit;

namespace AspNetCoreAwsServerless.Tests.Unit.Converters.Books;

public class BooksConverterTests
{
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

    BooksConverter converter = new();
    Book actual = converter.ToEntity(dto);

    Assert.Equal(expected.Title, actual.Title);
    Assert.Equal(expected.Author, expected.Author);
    Assert.Equal(expected.Pages, actual.Pages);
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

    BooksConverter converter = new();
    Book actual = converter.ToEntity(dto);

    Assert.Equal(expected.Id, actual.Id);
    Assert.Equal(expected.Title, actual.Title);
    Assert.Equal(expected.Author, expected.Author);
    Assert.Equal(expected.Pages, actual.Pages);
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

    BooksConverter converter = new();
    Book actual = converter.ToEntity(dto, oldBook);

    Assert.Equivalent(expected, actual);
  }
}
