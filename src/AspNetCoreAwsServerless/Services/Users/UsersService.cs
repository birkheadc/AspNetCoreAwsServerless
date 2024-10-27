using System.Security.Claims;
using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Roles;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Repositories.Users;
using AspNetCoreAwsServerless.Services.Jwt;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Services.Users;

public class UsersService(
  IUsersRepository usersRepository,
  IUsersConverter usersConverter,
  ILogger<UsersService> logger,
  IJwtService jwtService
) : IUsersService
{
  private readonly IUsersRepository _usersRepository = usersRepository;
  private readonly IUsersConverter _usersConverter = usersConverter;
  private readonly ILogger<UsersService> _logger = logger;

  private readonly IJwtService _jwtService = jwtService;

  public async Task<ApiResult<User>> Get(Id<User> id)
  {
    return await _usersRepository.Get(id);
  }

  public async Task<ApiResult<User>> GetOrCreateNew(IdToken token)
  {
    _logger.LogInformation("Getting or creating new user from IdToken");
    IEnumerable<Claim> claims = _jwtService.Decode(token.Value);
    string? userIdString = claims.FirstOrDefault(c => c.Type == "sub")?.Value;
    if (userIdString is null)
    {
      _logger.LogError("User id [Claim.sub] is missing from the IdToken.");
      return ApiResultErrors.InternalServerError;
    }

    Id<User> userId = new(userIdString);

    string? email = claims.FirstOrDefault(c => c.Type == "email")?.Value;

    if (email is null)
    {
      _logger.LogError("Email [Claim.email] is missing from the IdToken.");
      return ApiResultErrors.InternalServerError;
    }

    ApiResult<User> userResult = await Get(userId);

    if (userResult.IsSuccess)
    {
      return userResult;
    }

    User user =
      new()
      {
        Id = userId,
        EmailAddress = email,
        Profile = new(),
        Roles = new(),
      };

    return await _usersRepository.Put(user);
  }

  public async Task<ApiResult<User>> UpdateRoles(Id<User> id, UserRolesDto dto)
  {
    _logger.LogInformation("Updating roles for user {Id} to roles {roles}", id, dto.Roles);
    ApiResult<User> userResult = await Get(id);
    if (userResult.IsFailure)
    {
      return userResult;
    }

    if (userResult.Value.Roles.Roles.Contains(UserRole.SuperAdmin))
    {
      _logger.LogError(
        "User {Id} is a Super Admin and cannot have their roles modified through the API.",
        id
      );
      return ApiResultErrors.BadRequest;
    }

    User newUser = userResult.Value with { Roles = _usersConverter.ToEntity(dto) };

    return await _usersRepository.Put(newUser);
  }
}
