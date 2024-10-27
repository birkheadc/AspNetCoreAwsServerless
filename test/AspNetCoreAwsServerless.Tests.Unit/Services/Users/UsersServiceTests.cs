using System.Security.Claims;
using Amazon.DynamoDBv2.Model;
using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Roles;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Repositories.Users;
using AspNetCoreAwsServerless.Services.Jwt;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;
using Moq;
using Moq.AutoMock;

namespace AspNetCoreAwsServerless.Tests.Unit.Services.Users;

public class UsersServiceTests
{
  private readonly AutoMocker _mocker = new();
  private readonly UsersService _service;
  private readonly Mock<IUsersRepository> _usersRepositoryMock;
  private readonly Mock<IJwtService> _jwtServiceMock;
  private readonly Mock<IUsersConverter> _usersConverterMock;

  public UsersServiceTests()
  {
    _service = _mocker.CreateInstance<UsersService>();
    _usersRepositoryMock = _mocker.GetMock<IUsersRepository>();
    _jwtServiceMock = _mocker.GetMock<IJwtService>();
    _usersConverterMock = _mocker.GetMock<IUsersConverter>();
  }

  [Fact]
  public async Task Get_ReturnsUser()
  {
    User expected = _mocker.GetMock<User>().Object;

    _usersRepositoryMock.Setup(mock => mock.Get(It.IsAny<Id<User>>())).ReturnsAsync(expected);
    ApiResult<User> actual = await _service.Get(new Id<User>(Guid.NewGuid()));

    actual.Should().HaveSucceeded().And.HaveValue(expected);
  }

  [Fact]
  public async Task GetOrCreateNew_ReturnsInternalServerError_WhenUserIdIsMissing()
  {
    _jwtServiceMock.Setup(mock => mock.Decode(It.IsAny<string>())).Returns([]);
    ApiResult<User> actual = await _service.GetOrCreateNew(_mocker.CreateInstance<IdToken>());

    actual.Should().HaveFailed().And.HaveErrors(ApiResultErrors.InternalServerError);
  }

  [Fact]
  public async Task GetOrCreateNew_ReturnsInternalServerError_WhenEmailIsMissing()
  {
    _jwtServiceMock
      .Setup(mock => mock.Decode(It.IsAny<string>()))
      .Returns([new Claim("sub", Guid.NewGuid().ToString())]);
    ApiResult<User> actual = await _service.GetOrCreateNew(_mocker.CreateInstance<IdToken>());

    actual.Should().HaveFailed().And.HaveErrors(ApiResultErrors.InternalServerError);
  }

  [Fact]
  public async Task GetOrCreateNew_ReturnsUser_WhenUserExists()
  {
    User expected = _mocker.CreateInstance<User>();
    expected.Id = new Id<User>(Guid.NewGuid());
    expected.EmailAddress = "test@test.com";
    _usersRepositoryMock.Setup(mock => mock.Get(It.IsAny<Id<User>>())).ReturnsAsync(expected);

    _jwtServiceMock
      .Setup(mock => mock.Decode(It.IsAny<string>()))
      .Returns(
        [new Claim("sub", expected.Id.ToString()), new Claim("email", expected.EmailAddress)]
      );

    ApiResult<User> actual = await _service.GetOrCreateNew(_mocker.CreateInstance<IdToken>());

    actual.Should().HaveSucceeded().And.HaveValue(expected);
  }

  [Fact]
  public async Task GetOrCreateNew_CreatesAndReturnsNewUser_WhenUserDoesNotExist()
  {
    User expected = _mocker.CreateInstance<User>();
    expected.Id = new Id<User>(Guid.NewGuid());
    expected.EmailAddress = "test@test.com";

    _usersRepositoryMock.Setup(mock => mock.Get(It.IsAny<Id<User>>())).ReturnsAsync(expected);

    _jwtServiceMock
      .Setup(mock => mock.Decode(It.IsAny<string>()))
      .Returns(
        [new Claim("sub", expected.Id.ToString()), new Claim("email", expected.EmailAddress)]
      );

    ApiResult<User> actual = await _service.GetOrCreateNew(_mocker.CreateInstance<IdToken>());

    actual.Should().HaveSucceeded().And.HaveValue(expected);
  }

  [Fact]
  public async Task Patch_Fails_WhenTargetUserIsNotFound()
  {
    User oldUser = _mocker.CreateInstance<User>();
    oldUser.Id = new Id<User>(Guid.NewGuid());
    oldUser.DisplayName = "Old Display Name";

    UserPatchDto dto = new() { DisplayName = "New Display Name" };

    _usersRepositoryMock
      .Setup(mock => mock.Get(It.IsAny<Id<User>>()))
      .ReturnsAsync(ApiResult<User>.NotFound);

    ApiResult<User> result = await _service.Patch(oldUser.Id, dto);

    result.Should().HaveFailed().And.HaveErrors(ApiResultErrors.NotFound);
  }

  [Fact]
  public async Task Patch_UpdatesUser_WithNewData()
  {
    User oldUser = _mocker.CreateInstance<User>();
    oldUser.Id = new Id<User>(Guid.NewGuid());
    oldUser.DisplayName = "Old Display Name";

    UserPatchDto dto = new() { DisplayName = "New Display Name" };

    User expected = _mocker.CreateInstance<User>();
    expected.Id = oldUser.Id;
    expected.DisplayName = dto.DisplayName;

    _usersRepositoryMock.Setup(mock => mock.Get(It.IsAny<Id<User>>())).ReturnsAsync(oldUser);
    _usersRepositoryMock.Setup(mock => mock.Put(It.IsAny<User>())).ReturnsAsync(expected);
    _usersConverterMock
      .Setup(mock => mock.FromEntityAndPatchDto(It.IsAny<User>(), It.IsAny<UserPatchDto>()))
      .Returns(expected);

    ApiResult<User> actual = await _service.Patch(oldUser.Id, dto);

    _usersRepositoryMock.Verify(mock => mock.Put(expected), Times.Once);
    actual.Should().HaveSucceeded().And.HaveValue(expected);
  }

  [Fact]
  public async Task UpdateRoles_UpdatesUserRoles()
  {
    User oldUser = _mocker.CreateInstance<User>();
    oldUser.Id = new Id<User>(Guid.NewGuid());
    oldUser.Roles = [];

    UserRolesPatchDto dto = new() { Roles = [UserRole.Admin] };

    User expected = _mocker.CreateInstance<User>();
    expected.Id = oldUser.Id;
    expected.Roles = dto.Roles;

    _usersRepositoryMock.Setup(mock => mock.Get(It.IsAny<Id<User>>())).ReturnsAsync(oldUser);
    _usersRepositoryMock.Setup(mock => mock.Put(It.IsAny<User>())).ReturnsAsync(expected);

    ApiResult<User> actual = await _service.UpdateRoles(oldUser.Id, dto);

    _usersRepositoryMock.Verify(mock => mock.Put(expected), Times.Once);
    actual.Should().HaveSucceeded().And.HaveValue(expected);
  }

  [Fact]
  public async Task UpdateRoles_Fails_WhenTargetUserIsNotFound()
  {
    User oldUser = _mocker.CreateInstance<User>();
    oldUser.Id = new Id<User>(Guid.NewGuid());
    oldUser.Roles = [];

    UserRolesPatchDto dto = new() { Roles = [UserRole.Admin] };

    User expected = _mocker.CreateInstance<User>();
    expected.Id = oldUser.Id;
    expected.Roles = dto.Roles;

    _usersRepositoryMock
      .Setup(mock => mock.Get(It.IsAny<Id<User>>()))
      .ReturnsAsync(ApiResultErrors.NotFound);

    ApiResult<User> result = await _service.UpdateRoles(oldUser.Id, dto);

    result.Should().HaveFailed().And.HaveErrors(ApiResultErrors.NotFound);
  }

  [Fact]
  public async Task UpdateRoles_ReturnsBadRequest_WhenTargetUserIsAlreadySuperAdmin()
  {
    User oldUser = _mocker.CreateInstance<User>();
    oldUser.Id = new Id<User>(Guid.NewGuid());
    oldUser.Roles = [UserRole.SuperAdmin];

    UserRolesPatchDto dto = new() { Roles = [UserRole.Admin] };

    User expected = _mocker.CreateInstance<User>();
    expected.Id = oldUser.Id;
    expected.Roles = dto.Roles;

    _usersRepositoryMock.Setup(mock => mock.Get(It.IsAny<Id<User>>())).ReturnsAsync(oldUser);

    ApiResult<User> result = await _service.UpdateRoles(oldUser.Id, dto);

    result.Should().HaveFailed().And.HaveErrors(ApiResultErrors.BadRequest);
  }
}
