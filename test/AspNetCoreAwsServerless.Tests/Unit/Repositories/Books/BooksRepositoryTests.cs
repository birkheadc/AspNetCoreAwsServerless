using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Repositories.Books;
using AspNetCoreAwsServerless.Tests.Mocks.AsyncSearch;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace AspNetCoreAwsServerless.Tests.Unit.Repositories.Books;

public class BooksRepositoryTests
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
  private readonly BooksRepository _repository;
  private readonly Mock<IDynamoDBContext> _dynamoMock;

  public BooksRepositoryTests()
  {
    _mocker = new();
    _repository = _mocker.CreateInstance<BooksRepository>();
    _dynamoMock = _mocker.GetMock<IDynamoDBContext>();
  }

  [Fact]
  public async Task Get_WhenBookExists_ReturnsBook()
  {
    Book expected = _testBooks[0];

    _mocker
      .GetMock<IDynamoDBContext>()
      .Setup(mock => mock.LoadAsync<Book>(It.IsAny<Id<Book>>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(_testBooks[0]);

    ApiResult<Book> result = await _repository.Get(_testBooks[0].Id);

    _dynamoMock.Verify(mock => mock.LoadAsync<Book>(expected.Id, default), Times.Once);

    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public async Task Get_WhenBookNotExists_ReturnsNotFound()
  {
    Book expected = _testBooks[0];

    ApiResult<Book> result = await _repository.Get(expected.Id);

    _dynamoMock.Verify(mock => mock.LoadAsync<Book>(expected.Id, default), Times.Once);

    Assert.False(result.IsSuccess);
    Assert.Equal(404, result.Errors.StatusCode);
  }

  [Fact]
  public async Task Get_WhenError_ReturnsInternalServerError()
  {
    Book expected = _testBooks[0];

    _mocker
      .GetMock<IDynamoDBContext>()
      .Setup(mock => mock.LoadAsync<Book>(It.IsAny<object>(), It.IsAny<CancellationToken>()))
      .ThrowsAsync(new InternalServerErrorException(""));

    ApiResult<Book> result = await _repository.Get(expected.Id);

    _dynamoMock.Verify(mock => mock.LoadAsync<Book>(expected.Id, default), Times.Once);

    Assert.False(result.IsSuccess);
    Assert.Equal(500, result.Errors.StatusCode);
  }

  [Fact]
  public async Task GetAll_ReturnsAllBooks()
  {
    List<Book> expected = _testBooks;

    _mocker
      .GetMock<IDynamoDBContext>()
      .Setup(mock =>
        mock.ScanAsync<Book>(It.IsAny<List<ScanCondition>>(), It.IsAny<DynamoDBOperationConfig>())
      )
      .Returns(new MockAsyncSearch<Book>(expected));

    ApiResult<List<Book>> result = await _repository.GetAll();

    _dynamoMock.Verify(
      mock => mock.ScanAsync<Book>(new List<ScanCondition>(), default),
      Times.Once
    );

    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public async Task GetAll_WhenError_ReturnsInternalServerError()
  {
    List<Book> expected = _testBooks;

    _mocker
      .GetMock<IDynamoDBContext>()
      .Setup(mock =>
        mock.ScanAsync<Book>(It.IsAny<List<ScanCondition>>(), It.IsAny<DynamoDBOperationConfig>())
      )
      .Throws(new InternalServerErrorException(""));

    ApiResult<List<Book>> result = await _repository.GetAll();

    _dynamoMock.Verify(
      mock => mock.ScanAsync<Book>(new List<ScanCondition>(), default),
      Times.Once
    );

    Assert.False(result.IsSuccess);
    Assert.Equal(500, result.Errors.StatusCode);
  }

  [Fact]
  public async Task Put_CallsSaveAsync_ReturnsResult()
  {
    Book expected = _testBooks[0];

    ApiResult<Book> result = await _repository.Put(expected);

    _dynamoMock.Verify(mock => mock.SaveAsync(expected, default), Times.Once);

    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public async Task Put_WhenError_ReturnsInternalServerError()
  {
    Book expected = _testBooks[0];

    _mocker
      .GetMock<IDynamoDBContext>()
      .Setup(mock => mock.SaveAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
      .Throws(new InternalServerErrorException(""));

    ApiResult<Book> result = await _repository.Put(expected);

    _dynamoMock.Verify(mock => mock.SaveAsync(expected, default), Times.Once);

    Assert.False(result.IsSuccess);
    Assert.Equal(500, result.Errors.StatusCode);
  }

  [Fact]
  public async Task Delete_WhenNotError_ReturnsSuccess()
  {
    Id<Book> id = _testBooks[0].Id;

    ApiResult result = await _repository.Delete(id);

    _dynamoMock.Verify(mock => mock.DeleteAsync<Book>(id, default), Times.Once);

    Assert.True(result.IsSuccess);
  }

  [Fact]
  public async Task Delete_WhenError_ReturnsInternalServerError()
  {
    Id<Book> id = _testBooks[0].Id;

    _mocker
      .GetMock<IDynamoDBContext>()
      .Setup(mock => mock.DeleteAsync<Book>(It.IsAny<Id<Book>>(), It.IsAny<CancellationToken>()))
      .Throws(new InternalServerErrorException(""));

    ApiResult result = await _repository.Delete(id);

    _dynamoMock.Verify(mock => mock.DeleteAsync<Book>(id, default), Times.Once);

    Assert.False(result.IsSuccess);
    Assert.Equal(500, result.Errors.StatusCode);
  }
}