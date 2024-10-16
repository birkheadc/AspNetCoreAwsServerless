using AspNetCoreAwsServerless.Caches.Session;
using AspNetCoreAwsServerless.Converters.Session;
using AspNetCoreAwsServerless.Dtos.Cognito;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Cognito;
using AspNetCoreAwsServerless.Services.Session;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Result;
using Moq;
using Moq.AutoMock;

namespace AspNetCoreAwsServerless.Tests.Unit.Services.Session;

public class SessionServiceTests
{
  private readonly AutoMocker _mocker = new();
  private readonly SessionService _service;
  private readonly Mock<ICognitoService> _cognitoServiceMock;
  private readonly Mock<IUsersService> _usersServiceMock;
  private readonly Mock<ISessionCache> _sessionCacheMock;
  private readonly Mock<ISessionConverter> _sessionConverterMock;
  public SessionServiceTests()
  {
    _service = _mocker.CreateInstance<SessionService>();
    _cognitoServiceMock = _mocker.GetMock<ICognitoService>();
    _usersServiceMock = _mocker.GetMock<IUsersService>();
    _sessionCacheMock = _mocker.GetMock<ISessionCache>();
    _sessionConverterMock = _mocker.GetMock<ISessionConverter>();
  }
  [Fact]
  public async Task Login_ReturnsFailure_WhenCognitoService_ReturnsFailure()
  {
    _cognitoServiceMock.Setup(c => c.GetTokens(It.IsAny<LoginDto>())).ReturnsAsync(ApiResult<CognitoTokens>.Failure(ApiResultErrors.BadRequest));

    ApiResult<SessionContext> result = await _service.Login(_mocker.CreateInstance<LoginDto>());

    Assert.True(result.IsFailure);
    Assert.Equal(ApiResultErrors.BadRequest, result.Errors);
  }

  [Fact]
  public async Task Login_ReturnsFailure_WhenUsersService_ReturnsFailure()
  {
    _cognitoServiceMock.Setup(c => c.GetTokens(It.IsAny<LoginDto>())).ReturnsAsync(ApiResult<CognitoTokens>.Success(_mocker.CreateInstance<CognitoTokens>()));
    _usersServiceMock.Setup(u => u.GetOrCreateNew(It.IsAny<IdToken>())).ReturnsAsync(ApiResult<User>.Failure(ApiResultErrors.BadRequest));

    ApiResult<SessionContext> result = await _service.Login(_mocker.CreateInstance<LoginDto>());

    Assert.True(result.IsFailure);
    Assert.Equal(ApiResultErrors.BadRequest, result.Errors);
  }

  [Fact]
  public async Task Login_ReturnsUserAndTokens_WhenSuccessful()
  {
    User user = _mocker.CreateInstance<User>();
    CognitoTokens cognitoTokens = _mocker.CreateInstance<CognitoTokens>();
    SessionTokens sessionTokens = _mocker.CreateInstance<SessionTokens>();

    _cognitoServiceMock.Setup(c => c.GetTokens(It.IsAny<LoginDto>())).ReturnsAsync(ApiResult<CognitoTokens>.Success(cognitoTokens));
    _usersServiceMock.Setup(u => u.GetOrCreateNew(It.IsAny<IdToken>())).ReturnsAsync(ApiResult<User>.Success(user));
    _sessionConverterMock.Setup(s => s.ToSessionTokens(It.IsAny<CognitoTokens>())).Returns(sessionTokens);

    ApiResult<SessionContext> result = await _service.Login(_mocker.CreateInstance<LoginDto>());

    _sessionCacheMock.Verify(s => s.SetAccessToken(user.Id, cognitoTokens.AccessToken), Times.Once);

    Assert.True(result.IsSuccess);
    Assert.Equal(user, result.Value.User);
    Assert.Equal(sessionTokens, result.Value.Tokens);
  }
}
