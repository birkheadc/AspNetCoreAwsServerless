using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using AspNetCoreAwsServerless.Config.Books;
using AspNetCoreAwsServerless.Entities.Books;
using AspNetCoreAwsServerless.Repositories.Books;
using AspNetCoreAwsServerless.Tests.Mocks.AsyncSearch;
using AspNetCoreAwsServerless.Tests.TestData.Books;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Paginated;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.Extensions.Options;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace AspNetCoreAwsServerless.Tests.Unit.Repositories.Books;

public class BooksRepositoryTests
{
  private readonly List<Book> _testBooks = [];

  private readonly BooksOptions _options = new() { PageSize = 15 };

  private readonly AutoMocker _mocker;
  private readonly BooksRepository _repository;
  private readonly Mock<IDynamoDBContext> _dynamoMock;

  public BooksRepositoryTests()
  {
    _testBooks = BooksTestData.GenerateBooks(50);
    _mocker = new();
    _mocker.GetMock<IOptions<BooksOptions>>().SetupGet(mock => mock.Value).Returns(_options);
    _repository = _mocker.CreateInstance<BooksRepository>();
    _dynamoMock = _mocker.GetMock<IDynamoDBContext>();
  }

  [Fact]
  public async Task Get_WhenBookExists_ReturnsBook()
  {
    Book expected = _testBooks[0];

    _dynamoMock
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

    _dynamoMock
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

    _dynamoMock
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

    _dynamoMock
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

    _dynamoMock
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

    _dynamoMock
      .Setup(mock => mock.DeleteAsync<Book>(It.IsAny<Id<Book>>(), It.IsAny<CancellationToken>()))
      .Throws(new InternalServerErrorException(""));

    ApiResult result = await _repository.Delete(id);

    _dynamoMock.Verify(mock => mock.DeleteAsync<Book>(id, default), Times.Once);

    Assert.False(result.IsSuccess);
    Assert.Equal(500, result.Errors.StatusCode);
  }

  // Todo: This test fails because BatchWrite<T> is not mockable.
  // [Fact]
  // public async Task PutMany_CallsCreateBatchWrite_AndCallsExecuteAsync()
  // {
  //   List<Book> expected = _testBooks;

  //   Mock<BatchWrite<Book>> _batchWriteMock = _mocker.GetMock<BatchWrite<Book>>();
  //   _dynamoMock
  //     .Setup(mock => mock.CreateBatchWrite<Book>(It.IsAny<DynamoDBOperationConfig>()))
  //     .Returns(_batchWriteMock.Object);

  //   var result = await _repository.PutMany(expected);

  //   _dynamoMock.Verify(mock => mock.CreateBatchWrite<Book>(default), Times.Once);
  //   _batchWriteMock.Verify(mock => mock.AddPutItems(expected), Times.Once);
  //   _batchWriteMock.Verify(mock => mock.ExecuteAsync(default), Times.Once);

  //   Assert.True(result.IsSuccess);
  // }

  [Fact]
  public async Task PutMany_WhenError_ReturnsInternalServerError()
  {
    List<Book> expected = _testBooks;

    _dynamoMock
      .Setup(mock => mock.CreateBatchWrite<Book>(It.IsAny<DynamoDBOperationConfig>()))
      .Throws<Exception>();

    var result = await _repository.PutMany(expected);

    _dynamoMock.Verify(mock => mock.CreateBatchWrite<Book>(default), Times.Once);

    Assert.False(result.IsSuccess);
    Assert.Equal(500, result.Errors.StatusCode);
  }

  [Fact]
  public async Task GetPage_ReturnsPaginatedSetWithToken()
  {
    List<Book> expected = _testBooks[.._options.PageSize];

    _dynamoMock
      .Setup(mock =>
        mock.FromScanAsync<Book>(
          It.IsAny<ScanOperationConfig>(),
          It.IsAny<DynamoDBOperationConfig>()
        )
      )
      .Returns(new MockAsyncSearch<Book>(expected));

    ApiResult<Paginated<Book>> result = await _repository.GetPage();

    _dynamoMock.Verify(
      mock =>
        mock.FromScanAsync<Book>(
          It.Is<ScanOperationConfig>(c => c.Limit == _options.PageSize),
          default
        ),
      Times.Once
    );

    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value.Values);
  }

  [Fact]
  public async Task GetPage_CallsFromScanAsync_WithPaginationToken()
  {
    string expected = "ExpectedPaginationToken";

    _dynamoMock
      .Setup(mock =>
        mock.FromScanAsync<Book>(
          It.IsAny<ScanOperationConfig>(),
          It.IsAny<DynamoDBOperationConfig>()
        )
      )
      .Returns(new MockAsyncSearch<Book>());

    ApiResult<Paginated<Book>> result = await _repository.GetPage(expected);

    _dynamoMock.Verify(
      mock =>
        mock.FromScanAsync<Book>(
          It.Is<ScanOperationConfig>(c =>
            c.Limit == _options.PageSize && c.PaginationToken == expected
          ),
          default
        ),
      Times.Once
    );

    Assert.True(result.IsSuccess);
  }

  [Fact]
  public async Task GetPage_WhenUnexpectedError_ReturnsInternalServerError()
  {
    string expected = "ExpectedPaginationToken";

    _dynamoMock
      .Setup(mock =>
        mock.FromScanAsync<Book>(
          It.IsAny<ScanOperationConfig>(),
          It.IsAny<DynamoDBOperationConfig>()
        )
      )
      .Throws<SystemException>();

    ApiResult<Paginated<Book>> result = await _repository.GetPage(expected);

    _dynamoMock.Verify(
      mock =>
        mock.FromScanAsync<Book>(
          It.Is<ScanOperationConfig>(c =>
            c.Limit == _options.PageSize && c.PaginationToken == expected
          ),
          default
        ),
      Times.Once
    );

    Assert.False(result.IsSuccess);
    Assert.Equal(500, result.Errors.StatusCode);
  }
}
