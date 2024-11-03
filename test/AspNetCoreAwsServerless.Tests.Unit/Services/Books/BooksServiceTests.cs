using AspNetCoreAwsServerless.Converters.Books;
using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Repositories.Books;
using AspNetCoreAwsServerless.Services.Books;
using AspNetCoreAwsServerless.Tests.Unit.TestData.Books;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;
using Moq;
using Moq.AutoMock;

namespace AspNetCoreAwsServerless.Tests.Unit.Services.Books;

public class BooksServiceTests
{
  private readonly List<Book> _testBooks = [];

  private readonly AutoMocker _mocker;
  private readonly BooksService _service;
  private readonly Mock<IBooksRepository> _repositoryMock;
  private readonly Mock<IBooksConverter> _converterMock;

  public BooksServiceTests()
  {
    _testBooks = BooksTestData.GenerateBooks(50);
    _mocker = new();
    _service = _mocker.CreateInstance<BooksService>();
    _repositoryMock = _mocker.GetMock<IBooksRepository>();
    _converterMock = _mocker.GetMock<IBooksConverter>();
  }

  [Fact]
  public async Task Get_WhenBookExists_ReturnsBook()
  {
    Book expected = _testBooks[1];

    _repositoryMock.Setup(mock => mock.Get(expected.Id)).ReturnsAsync(expected);

    ApiResult<Book> result = await _service.Get(expected.Id);

    _repositoryMock.Verify(mock => mock.Get(expected.Id), Times.Once);

    result.Should().HaveSucceeded().And.HaveValue(expected);
  }

  [Fact]
  public async Task GetAll_ReturnsAllBooks()
  {
    List<Book> expected = _testBooks;

    _repositoryMock.Setup(mock => mock.GetAll()).ReturnsAsync(expected);

    ApiResult<List<Book>> result = await _service.GetAll();

    _repositoryMock.Verify(mock => mock.GetAll(), Times.Once);

    result.Should().HaveSucceeded().And.HaveValue(expected);
  }

  [Fact]
  public async Task Create_CallsConverterToEntity_AndCallsRepositoryPut_AndReturnsResult()
  {
    BookCreateDto dto =
      new()
      {
        Title = "New Book",
        Author = "New Author",
        Pages = 50,
      };

    Book expected =
      new()
      {
        Id = new(),
        Title = dto.Title,
        Author = dto.Author,
        Pages = dto.Pages,
      };

    _repositoryMock.Setup(mock => mock.Put(It.IsAny<Book>())).ReturnsAsync((Book input) => input);
    _converterMock.Setup(mock => mock.ToEntity(It.IsAny<BookCreateDto>())).Returns(expected);

    ApiResult<Book> result = await _service.Create(dto);

    _repositoryMock.Verify(mock => mock.Put(expected), Times.Once);

    result.Should().HaveSucceeded().And.HaveValue(expected);
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
        Pages = 150,
      };

    Book expected =
      new()
      {
        Id = new(dto.Id),
        Title = dto.Title,
        Author = dto.Author,
        Pages = dto.Pages,
      };

    _repositoryMock.Setup(mock => mock.Put(It.IsAny<Book>())).ReturnsAsync((Book book) => book);
    _converterMock.Setup(mock => mock.ToEntity(It.IsAny<BookPutDto>())).Returns(expected);

    ApiResult<Book> result = await _service.Put(dto);

    _repositoryMock.Verify(mock => mock.Put(expected), Times.Once);

    result.Should().HaveSucceeded().And.HaveValue(expected);
  }

  [Fact]
  public async Task Delete_CallsRepositoryDelete()
  {
    Id<Book> expected = new();

    await _service.Delete(expected);

    _repositoryMock.Verify(mock => mock.Delete(expected), Times.Once);
  }

  [Fact]
  public async Task CreateMany_CallsConverterToEntity_AndCallsRepositoryPut()
  {
    List<Book> expected = _testBooks[..3];

    List<BookCreateDto> dtos = [];

    for (int i = 0; i < expected.Count; i++)
    {
      BookCreateDto _dto =
        new()
        {
          Title = expected[i].Title,
          Author = expected[i].Author,
          Pages = expected[i].Pages,
        };
      dtos.Add(_dto);
      _converterMock.Setup(mock => mock.ToEntity(_dto)).Returns(expected[i]);
    }

    BookCreateManyDto dto = new() { Books = dtos };

    _repositoryMock
      .Setup(mock => mock.PutMany(It.IsAny<List<Book>>()))
      .ReturnsAsync(ApiResult.Success());

    ApiResult result = await _service.CreateMany(dto);

    _repositoryMock.Verify(mock => mock.PutMany(expected), Times.Once);
    result.Should().HaveSucceeded();
  }
}
