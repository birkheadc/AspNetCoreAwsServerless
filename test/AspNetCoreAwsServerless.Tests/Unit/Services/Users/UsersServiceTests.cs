using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Dtos.Cognito;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Repositories.Users;
using AspNetCoreAwsServerless.Services.Cognito;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace AspNetCoreAwsServerless.Tests.Unit.Services.Users;

public class UsersServiceTests
{
  private readonly AutoMocker _mocker = new();
  private readonly UsersService _service;
  private readonly Mock<IUsersRepository> _usersRepositoryMock;
  private readonly Mock<ICognitoService> _cognitoServiceMock;
  private readonly Mock<IUsersConverter> _usersConverterMock;
  public UsersServiceTests()
  {
    _service = _mocker.CreateInstance<UsersService>();
    _usersRepositoryMock = _mocker.GetMock<IUsersRepository>();
    _cognitoServiceMock = _mocker.GetMock<ICognitoService>();
    _usersConverterMock = _mocker.GetMock<IUsersConverter>();
  }

  [Fact]
  public async Task Get_ReturnsUser()
  {
    User expected = new() { Id = new Id<User>(Guid.NewGuid()), EmailAddress = "test@test.com" };

    _usersRepositoryMock.Setup(mock => mock.Get(It.IsAny<Id<User>>())).ReturnsAsync(expected);
    ApiResult<User> actual = await _service.Get(new Id<User>(Guid.NewGuid()));

    Assert.NotNull(actual);
    Assert.Equal(expected, actual.Value);
  }

  [Fact]
  public async Task GetOrCreateNew_Fails_WhenIdIsNull()
  {
    ApiResult<User> actual = await _service.GetOrCreateNew(null, null);

    Assert.True(actual.IsFailure);
  }

  [Fact]
  public async Task GetOrCreateNew_Fails_WhenUserNotFound_AndAccessTokenIsNull()
  {
    _usersRepositoryMock.Setup(mock => mock.Get(It.IsAny<Id<User>>())).ReturnsAsync(ApiResult<User>.Failure(ApiResultErrors.NotFound));

    ApiResult<User> actual = await _service.GetOrCreateNew(Guid.NewGuid().ToString(), null);

    Assert.True(actual.IsFailure);
  }

  [Fact]
  public async Task GetOrCreateNew_ReturnsUser_WhenUserExists()
  {
    User expected = new() { Id = new Id<User>(Guid.NewGuid()), EmailAddress = "test@test.com" };
    _usersRepositoryMock.Setup(mock => mock.Get(It.IsAny<Id<User>>())).ReturnsAsync(expected);

    ApiResult<User> actual = await _service.GetOrCreateNew(Guid.NewGuid().ToString(), null);

    Assert.NotNull(actual);
    Assert.Equal(expected, actual.Value);
  }

  [Fact]
  public async Task GetOrCreateNew_CreatesAndReturnsUser_WhenUserDoesNotExist_AndCognitoServiceReturnsUser()
  {
    Id<User> id = new(Guid.NewGuid());
    User expected = new() { Id = id, EmailAddress = "test@test.com" };
    CognitoUser cognitoUser = new() { Username = expected.Id.ToString(), EmailAddress = expected.EmailAddress };

    _usersRepositoryMock.Setup(mock => mock.Get(It.IsAny<Id<User>>())).ReturnsAsync(ApiResult<User>.Failure(ApiResultErrors.NotFound));
    _cognitoServiceMock.Setup(mock => mock.GetUser(It.IsAny<string>())).ReturnsAsync(cognitoUser);
    _usersRepositoryMock.Setup(mock => mock.Put(It.IsAny<User>())).ReturnsAsync(ApiResult<User>.Success(expected));

    _usersConverterMock.Setup(mock => mock.FromCognitoUser(It.IsAny<CognitoUser>())).Returns(expected);

    ApiResult<User> actual = await _service.GetOrCreateNew(id.ToString(), "accessToken");

    Assert.NotNull(actual);
    _usersRepositoryMock.Verify(mock => mock.Put(It.IsAny<User>()), Times.Once);
    Assert.Equal(expected, actual.Value);
  }
}
