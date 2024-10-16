using System.Security.Claims;
using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Dtos.Cognito;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Repositories.Users;
using AspNetCoreAwsServerless.Services.Cognito;
using AspNetCoreAwsServerless.Services.Jwt;
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
  private readonly Mock<IJwtService> _jwtServiceMock;
  public UsersServiceTests()
  {
    _service = _mocker.CreateInstance<UsersService>();
    _usersRepositoryMock = _mocker.GetMock<IUsersRepository>();
    _cognitoServiceMock = _mocker.GetMock<ICognitoService>();
    _usersConverterMock = _mocker.GetMock<IUsersConverter>();
    _jwtServiceMock = _mocker.GetMock<IJwtService>();
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
  public async Task GetOrCreateNew_ReturnsInternalServerError_WhenUserIdIsMissing()
  {
    _jwtServiceMock.Setup(mock => mock.Decode(It.IsAny<string>())).Returns([]);
    ApiResult<User> actual = await _service.GetOrCreateNew(_mocker.CreateInstance<IdToken>());

    Assert.True(actual.IsFailure);
    Assert.Equal(ApiResultErrors.InternalServerError, actual.Errors);
  }

  [Fact]
  public async Task GetOrCreateNew_ReturnsInternalServerError_WhenEmailIsMissing()
  {
    _jwtServiceMock.Setup(mock => mock.Decode(It.IsAny<string>())).Returns([new Claim("sub", Guid.NewGuid().ToString())]);
    ApiResult<User> actual = await _service.GetOrCreateNew(_mocker.CreateInstance<IdToken>());

    Assert.True(actual.IsFailure);
    Assert.Equal(ApiResultErrors.InternalServerError, actual.Errors);
  }

  [Fact]
  public async Task GetOrCreateNew_ReturnsUser_WhenUserExists()
  {
    User expected = _mocker.CreateInstance<User>();
    expected.Id = new Id<User>(Guid.NewGuid());
    expected.EmailAddress = "test@test.com";
    _usersRepositoryMock.Setup(mock => mock.Get(It.IsAny<Id<User>>())).ReturnsAsync(expected);

    _jwtServiceMock.Setup(mock => mock.Decode(It.IsAny<string>())).Returns([new Claim("sub", expected.Id.ToString()), new Claim("email", expected.EmailAddress)]);

    ApiResult<User> actual = await _service.GetOrCreateNew(_mocker.CreateInstance<IdToken>());

    Assert.True(actual.IsSuccess);
    Assert.Equal(expected, actual.Value);
  }

  [Fact]
  public async Task GetOrCreateNew_CreatesAndReturnsNewUser_WhenUserDoesNotExist()
  {
    User expected = _mocker.CreateInstance<User>();
    expected.Id = new Id<User>(Guid.NewGuid());
    expected.EmailAddress = "test@test.com";

    _usersRepositoryMock.Setup(mock => mock.Get(It.IsAny<Id<User>>())).ReturnsAsync(expected);

    _jwtServiceMock.Setup(mock => mock.Decode(It.IsAny<string>())).Returns([new Claim("sub", expected.Id.ToString()), new Claim("email", expected.EmailAddress)]);

    ApiResult<User> actual = await _service.GetOrCreateNew(_mocker.CreateInstance<IdToken>());

    Assert.True(actual.IsSuccess);
    Assert.Equal(expected, actual.Value);
  }
}