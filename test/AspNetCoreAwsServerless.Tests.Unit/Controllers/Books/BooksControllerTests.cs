using AspNetCoreAwsServerless.Controllers.Books;
using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Services.Books;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Paginated;
using AspNetCoreAwsServerless.Utils.Result;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;

namespace AspNetCoreAwsServerless.Tests.Unit.Controllers.Books;

public class BooksControllerTests
{
  private readonly AutoMocker _mocker;
  private readonly BooksController _controller;
  private readonly Mock<IBooksService> _booksService;
  private readonly Mock<IBooksConverter> _booksConverter;

  public BooksControllerTests()
  {
    _mocker = new();
    _controller = _mocker.CreateInstance<BooksController>();
    _booksService = _mocker.GetMock<IBooksService>();
    _booksConverter = _mocker.GetMock<IBooksConverter>();
  }

  [Fact]
  public async Task GetFirstPage_CallsServiceGetPage_AndReturnsResult()
  {
    Paginated<BookDto> expected = new() { Values = [], PaginationToken = "token" };
    _booksService
      .Setup(service => service.GetPage(null))
      .ReturnsAsync(ApiResult<Paginated<Book>>.Success(_mocker.CreateInstance<Paginated<Book>>()));
    _booksConverter
      .Setup(converter => converter.ToDto(It.IsAny<Paginated<Book>>()))
      .Returns(expected);

    ActionResult<Paginated<BookDto>> result = await _controller.GetFirstPage();

    _booksService.Verify(service => service.GetPage(null), Times.Once);

    result.Should().HaveValue(expected);
  }

  [Fact]
  public async Task GetPage_CallsServiceGetPage_WithPaginationToken_AndReturnsResult()
  {
    Paginated<BookDto> expected = new() { Values = [], PaginationToken = "token" };
    _booksService
      .Setup(service => service.GetPage(It.IsAny<string>()))
      .ReturnsAsync(ApiResult<Paginated<Book>>.Success(_mocker.CreateInstance<Paginated<Book>>()));
    _booksConverter
      .Setup(converter => converter.ToDto(It.IsAny<Paginated<Book>>()))
      .Returns(expected);

    ActionResult<Paginated<BookDto>> result = await _controller.GetPage("token");

    _booksService.Verify(service => service.GetPage("token"), Times.Once);

    result.Should().HaveValue(expected);
  }

  [Fact]
  public async Task Get_CallsServiceGet_WithId_AndReturnsResult()
  {
    Id<Book> id = new(Guid.NewGuid());
    BookDto expected =
      new()
      {
        Id = id.ToString(),
        Title = "title",
        Author = "author",
        Pages = 100,
      };

    _booksService
      .Setup(service => service.Get(It.IsAny<Id<Book>>()))
      .ReturnsAsync(ApiResult<Book>.Success(_mocker.CreateInstance<Book>()));
    _booksConverter.Setup(converter => converter.ToDto(It.IsAny<Book>())).Returns(expected);

    ActionResult<BookDto> result = await _controller.Get(id);

    _booksService.Verify(service => service.Get(It.IsAny<Id<Book>>()), Times.Once);

    result.Should().HaveValue(expected);
  }

  [Fact]
  public async Task Create_CallsServiceCreate_WithCreateDto_AndReturnsResult()
  {
    BookCreateDto createDto =
      new()
      {
        Title = "title",
        Author = "author",
        Pages = 100,
      };
    BookDto expected =
      new()
      {
        Id = Guid.NewGuid().ToString(),
        Title = "title",
        Author = "author",
        Pages = 100,
      };

    _booksService
      .Setup(service => service.Create(It.IsAny<BookCreateDto>()))
      .ReturnsAsync(ApiResult<Book>.Success(_mocker.CreateInstance<Book>()));
    _booksConverter.Setup(converter => converter.ToDto(It.IsAny<Book>())).Returns(expected);

    ActionResult<BookDto> result = await _controller.Create(createDto);

    _booksService.Verify(service => service.Create(createDto), Times.Once);

    result.Should().HaveValue(expected);
  }

  [Fact]
  public async Task CreateMany_CallsServiceCreateMany_WithCreateManyDto_AndReturnsResult()
  {
    BookCreateManyDto createManyDto =
      new()
      {
        Books =
        [
          new BookCreateDto
          {
            Title = "title",
            Author = "author",
            Pages = 100,
          },
        ],
      };
    BookDto expected =
      new()
      {
        Id = Guid.NewGuid().ToString(),
        Title = createManyDto.Books[0].Title,
        Author = createManyDto.Books[0].Author,
        Pages = createManyDto.Books[0].Pages,
      };

    _booksService
      .Setup(service => service.CreateMany(It.IsAny<BookCreateManyDto>()))
      .ReturnsAsync(ApiResult.Success);

    ActionResult result = await _controller.CreateMany(createManyDto);

    _booksService.Verify(service => service.CreateMany(createManyDto), Times.Once);

    result.Should().HaveSucceeded();
  }

  [Fact]
  public async Task Put_CallsServicePut_WithPutDto_AndReturnsResult()
  {
    BookPutDto putDto =
      new()
      {
        Id = new(Guid.NewGuid().ToString()),
        Title = "title",
        Author = "author",
        Pages = 100,
      };
    BookDto expected =
      new()
      {
        Id = putDto.Id.ToString(),
        Title = putDto.Title,
        Author = putDto.Author,
        Pages = putDto.Pages,
      };

    _booksService
      .Setup(service => service.Put(It.IsAny<BookPutDto>()))
      .ReturnsAsync(ApiResult<Book>.Success(_mocker.CreateInstance<Book>()));
    _booksConverter.Setup(converter => converter.ToDto(It.IsAny<Book>())).Returns(expected);

    ActionResult<BookDto> result = await _controller.Put(putDto);

    _booksService.Verify(service => service.Put(putDto), Times.Once);

    result.Should().HaveValue(expected);
  }

  [Fact]
  public async Task Delete_CallsServiceDelete_WithId_AndReturnsResult()
  {
    Id<Book> id = new(Guid.NewGuid());

    _booksService.Setup(service => service.Delete(id)).ReturnsAsync(ApiResult.Success);

    ActionResult result = await _controller.Delete(id);

    _booksService.Verify(service => service.Delete(id), Times.Once);
    result.Should().HaveSucceeded();
  }

  [Fact]
  public async Task SeedMany_CallsServiceCreateMany_WithNum_AndReturnsResult()
  {
    int num = 10;

    _booksService
      .Setup(service => service.CreateMany(It.IsAny<BookCreateManyDto>()))
      .ReturnsAsync(ApiResult.Success);

    ActionResult result = await _controller.SeedMany(num);

    _booksService.Verify(service => service.CreateMany(It.IsAny<BookCreateManyDto>()), Times.Once);
    result.Should().HaveSucceeded();
  }

  [Fact]
  public async Task SeedMany_WithNumGreaterThan999_ReturnsBadRequest()
  {
    int num = 1000;
    ActionResult result = await _controller.SeedMany(num);

    _booksService.Verify(service => service.CreateMany(It.IsAny<BookCreateManyDto>()), Times.Never);

    result.Should().HaveFailed();
  }

  [Fact]
  public async Task SeedMany_WithNumLessThan1_ReturnsBadRequest()
  {
    int num = 0;
    ActionResult result = await _controller.SeedMany(num);

    _booksService.Verify(service => service.CreateMany(It.IsAny<BookCreateManyDto>()), Times.Never);

    result.Should().HaveFailed();
  }
}
