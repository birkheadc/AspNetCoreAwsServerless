using System.Security.Claims;
using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Dtos.Cognito;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Repositories.Users;
using AspNetCoreAwsServerless.Services.Cognito;
using AspNetCoreAwsServerless.Services.Jwt;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Services.Users;

public class UsersService(
  IUsersRepository usersRepository,
  ILogger<UsersService> logger,
  ICognitoService cognitoService,
  IUsersConverter usersConverter,
  IJwtService jwtService
) : IUsersService
{
  private readonly IUsersRepository _usersRepository = usersRepository;
  private readonly ILogger<UsersService> _logger = logger;
  private readonly ICognitoService _cognitoService = cognitoService;
  private readonly IUsersConverter _usersConverter = usersConverter;

  private readonly IJwtService _jwtService = jwtService;

  public async Task<ApiResult<User>> Get(Id<User> id)
  {
    return await _usersRepository.Get(id);
  }

  public async Task<ApiResult<User>> GetOrCreateNew(IdToken token)
  {
    IEnumerable<Claim> claims = _jwtService.Decode(token.Value);
    string? userId = claims.FirstOrDefault(c => c.Type == "sub")?.Value;
    string? email = claims.FirstOrDefault(c => c.Type == "email")?.Value;

    if (userId is null)
    {
      _logger.LogError("User id [Claim.sub] is missing from the IdToken.");
      return ApiResultErrors.InternalServerError;
    }

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

    User user = new()
    {
      Id = new Id<User>(userId),
      EmailAddress = email
    };

    return await _usersRepository.Put(user);
  }

  public Task<ApiResult<User>> Patch(Id<User> id, UserPatchDto dto)
  {
    throw new NotImplementedException();
  }

  public Task<ApiResult<User>> Put(UserPutDto dto)
  {
    throw new NotImplementedException();
  }
}
