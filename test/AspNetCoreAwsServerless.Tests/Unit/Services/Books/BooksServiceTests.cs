using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Repositories.Books;
using AspNetCoreAwsServerless.Services.Books;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace AspNetCoreAwsServerless.Tests.Unit.Services.Books;

public class BooksServiceTests
{
  private readonly List<Book> _testBooks =
  [
    new()
    {
      Id = new(),
      Title = "Book 1",
      Author = "Author 1",
      Pages = 100
    },
    new()
    {
      Id = new(),
      Title = "Book 2",
      Author = "Author 2",
      Pages = 200
    },
    new()
    {
      Id = new(),
      Title = "Book 3",
      Author = "Author 3",
      Pages = 300
    }
  ];

  [Fact]
  public async Task Get_WhenBookExists_ReturnsBook()
  {
    AutoMocker mocker = new();

    Book expected = _testBooks[1];

    mocker.GetMock<IBooksRepository>().Setup(mock => mock.Get(expected.Id)).ReturnsAsync(expected);

    BooksService service = mocker.CreateInstance<BooksService>();
    ApiResult<Book> result = await service.Get(expected.Id);

    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public async Task GetAll_ReturnsAllBooks()
  {
    AutoMocker mocker = new();

    List<Book> expected = _testBooks;

    mocker.GetMock<IBooksRepository>().Setup(mock => mock.GetAll()).ReturnsAsync(expected);

    BooksService service = mocker.CreateInstance<BooksService>();
    ApiResult<List<Book>> result = await service.GetAll();

    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public async Task Create_CallsConverterToEntity_AndCallsRepositoryPut_AndReturnsResult()
  {
    AutoMocker mocker = new();

    BookCreateDto dto =
      new()
      {
        Title = "New Book",
        Author = "New Author",
        Pages = 50
      };

    Book expected =
      new()
      {
        Id = new(),
        Title = dto.Title,
        Author = dto.Author,
        Pages = dto.Pages
      };

    mocker
      .GetMock<IBooksRepository>()
      .Setup(mock => mock.Put(It.IsAny<Book>()))
      .ReturnsAsync((Book input) => input);
    mocker
      .GetMock<IBooksConverter>()
      .Setup(mock => mock.ToEntity(It.IsAny<BookCreateDto>()))
      .Returns(expected);

    BooksService service = mocker.CreateInstance<BooksService>();
    ApiResult<Book> result = await service.Create(dto);

    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public async Task Put_CallsRepositoryPut_AndReturnsResult()
  {
    AutoMocker mocker = new();

    BookPutDto dto =
      new()
      {
        Id = _testBooks[2].Id.ToString(),
        Title = "New Title",
        Author = "New Author",
        Pages = 150
      };

    Book expected =
      new()
      {
        Id = new(dto.Id),
        Title = dto.Title,
        Author = dto.Author,
        Pages = dto.Pages
      };

    mocker
      .GetMock<IBooksRepository>()
      .Setup(mock => mock.Put(It.IsAny<Book>()))
      .ReturnsAsync((Book book) => book);
    mocker
      .GetMock<IBooksConverter>()
      .Setup(mock => mock.ToEntity(It.IsAny<BookPutDto>()))
      .Returns(expected);

    BooksService service = mocker.CreateInstance<BooksService>();
    ApiResult<Book> result = await service.Put(dto);

    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public async Task Patch_FetchesBookFromRepository_AndCallsRepositoryPut_AndReturnsResult()
  {
    AutoMocker mocker = new();

    BookPatchDto dto = new() { Title = "New Title", };

    Book expected =
      new()
      {
        Id = _testBooks[0].Id,
        Title = dto.Title,
        Author = _testBooks[0].Author,
        Pages = _testBooks[0].Pages
      };

    mocker
      .GetMock<IBooksRepository>()
      .Setup(mock => mock.Get(It.IsAny<Id<Book>>()))
      .ReturnsAsync(_testBooks[0]);
    mocker
      .GetMock<IBooksRepository>()
      .Setup(mock => mock.Put(It.IsAny<Book>()))
      .ReturnsAsync((Book book) => book);
    mocker
      .GetMock<IBooksConverter>()
      .Setup(mock => mock.ToEntity(It.IsAny<BookPatchDto>(), It.IsAny<Book>()))
      .Returns(expected);

    BooksService service = mocker.CreateInstance<BooksService>();
    ApiResult<Book> result = await service.Patch(_testBooks[0].Id, dto);

    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public async Task Delete_CallsRepositoryDelete()
  {
    AutoMocker mocker = new();

    Id<Book> expected = new();

    Mock<IBooksRepository> repositoryMock = mocker.GetMock<IBooksRepository>();

    BooksService service = mocker.CreateInstance<BooksService>();
    await service.Delete(expected);

    repositoryMock.Verify(mock => mock.Delete(expected), Times.Once);
  }
}
