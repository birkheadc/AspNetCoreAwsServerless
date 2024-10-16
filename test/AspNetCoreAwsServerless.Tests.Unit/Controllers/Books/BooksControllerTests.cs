using AspNetCoreAwsServerless.Controllers.Books;
using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Services.Books;
using AspNetCoreAwsServerless.Utils.Paginated;
using AspNetCoreAwsServerless.Utils.Result;
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
    Paginated<BookDto> expected = new() { Values = [], PaginationToken = "wtf" };
    _booksService.Setup(service => service.GetPage(null)).ReturnsAsync(ApiResult<Paginated<Book>>.Success(new Paginated<Book> { Values = [], PaginationToken = "wtf" }));
    _booksConverter.Setup(converter => converter.ToDto(It.IsAny<Paginated<Book>>())).Returns(expected);

    ActionResult<Paginated<BookDto>> result = await _controller.GetFirstPage();

    Assert.IsType<OkObjectResult>(result.Result);
    Assert.Equal(expected, ((OkObjectResult)result.Result).Value);
  }
}
