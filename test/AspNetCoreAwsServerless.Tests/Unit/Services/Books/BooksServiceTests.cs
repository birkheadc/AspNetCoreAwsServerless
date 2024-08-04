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
      Id = "8f56f8a7-493b-4870-9225-e08fb152c19a",
      Title = "Book 1",
      Author = "Author 1",
      Pages = 100
    },
    new()
    {
      Id = "9d7cb104-ca04-4a0d-bb83-d498aeabfb7c",
      Title = "Book 2",
      Author = "Author 2",
      Pages = 200
    },
    new()
    {
      Id = "aef601ad-10b3-40f8-8681-94cafe78cf87",
      Title = "Book 3",
      Author = "Author 3",
      Pages = 300
    }
  ];

  private readonly AutoMocker _mocker;
  private readonly BooksService _service;
  private readonly Mock<IBooksRepository> _repositoryMock;

  public BooksServiceTests()
  {
    _mocker = new();
    _service = _mocker.CreateInstance<BooksService>();
    _repositoryMock = _mocker.GetMock<IBooksRepository>();
  }

  [Fact]
  public async Task Get_WhenBookExists_ReturnsBook()
  {
    Book expected = _testBooks[1];

    _mocker.GetMock<IBooksRepository>().Setup(mock => mock.Get(expected.Id)).ReturnsAsync(expected);

    ApiResult<Book> result = await _service.Get(expected.Id);

    _repositoryMock.Verify(mock => mock.Get(expected.Id), Times.Once);

    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public async Task GetAll_ReturnsAllBooks()
  {
    List<Book> expected = _testBooks;

    _mocker.GetMock<IBooksRepository>().Setup(mock => mock.GetAll()).ReturnsAsync(expected);

    ApiResult<List<Book>> result = await _service.GetAll();

    _repositoryMock.Verify(mock => mock.GetAll(), Times.Once);

    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public async Task Create_CallsConverterToEntity_AndCallsRepositoryPut_AndReturnsResult()
  {
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

    _mocker
      .GetMock<IBooksRepository>()
      .Setup(mock => mock.Put(It.IsAny<Book>()))
      .ReturnsAsync((Book input) => input);
    _mocker
      .GetMock<IBooksConverter>()
      .Setup(mock => mock.ToEntity(It.IsAny<BookCreateDto>()))
      .Returns(expected);

    ApiResult<Book> result = await _service.Create(dto);

    _repositoryMock.Verify(mock => mock.Put(expected), Times.Once);

    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public async Task Put_CallsRepositoryPut_AndReturnsResult()
  {
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

    _mocker
      .GetMock<IBooksRepository>()
      .Setup(mock => mock.Put(It.IsAny<Book>()))
      .ReturnsAsync((Book book) => book);
    _mocker
      .GetMock<IBooksConverter>()
      .Setup(mock => mock.ToEntity(It.IsAny<BookPutDto>()))
      .Returns(expected);

    ApiResult<Book> result = await _service.Put(dto);

    _repositoryMock.Verify(mock => mock.Put(expected), Times.Once);

    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public async Task Patch_FetchesBookFromRepository_AndCallsRepositoryPut_AndReturnsResult()
  {
    BookPatchDto dto = new() { Title = "New Title", };

    Book expected =
      new()
      {
        Id = _testBooks[0].Id,
        Title = dto.Title,
        Author = _testBooks[0].Author,
        Pages = _testBooks[0].Pages
      };

    _mocker
      .GetMock<IBooksRepository>()
      .Setup(mock => mock.Get(It.IsAny<Id<Book>>()))
      .ReturnsAsync(_testBooks[0]);
    _mocker
      .GetMock<IBooksRepository>()
      .Setup(mock => mock.Put(It.IsAny<Book>()))
      .ReturnsAsync((Book book) => book);
    _mocker
      .GetMock<IBooksConverter>()
      .Setup(mock => mock.ToEntity(It.IsAny<BookPatchDto>(), It.IsAny<Book>()))
      .Returns(expected);

    ApiResult<Book> result = await _service.Patch(_testBooks[0].Id, dto);

    _repositoryMock.Verify(mock => mock.Put(expected), Times.Once);

    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public async Task Delete_CallsRepositoryDelete()
  {
    Id<Book> expected = new();

    await _service.Delete(expected);

    _repositoryMock.Verify(mock => mock.Delete(expected), Times.Once);
  }
}
