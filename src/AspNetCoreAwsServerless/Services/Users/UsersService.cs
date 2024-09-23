using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Dtos.CognitoUser;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Repositories.Users;
using AspNetCoreAwsServerless.Services.Cognito;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Services.Users;

public class UsersService(IUsersRepository usersRepository, ILogger<UsersService> logger, ICognitoService cognitoService, IUsersConverter usersConverter) : IUsersService
{
  private readonly IUsersRepository _usersRepository = usersRepository;
  private readonly ILogger<UsersService> _logger = logger;
  private readonly ICognitoService _cognitoService = cognitoService;
  private readonly IUsersConverter _usersConverter = usersConverter;

  public async Task<ApiResult<User>> Get(Id<User> id)
  {
    return await _usersRepository.Get(id);
  }

  public async Task<ApiResult<User>> GetOrCreateNew(string? id, string? accessToken)
  {
    _logger.LogInformation("GetOrCreateNew Id: {id} | AccessToken?: {accessToken?}", id, accessToken is not null);

    if (id is null)
    {
      return ApiResult<User>.Failure(ApiResultErrors.Unauthorized);
    }

    ApiResult<User> userResult = await Get(new Id<User>(id));
    if (userResult.IsSuccess)
    {
      return userResult;
    }

    if (accessToken is null)
    {
      return ApiResult<User>.Failure(ApiResultErrors.Unauthorized);
    }

    ApiResult<CognitoUser> cognitoUserResult = await _cognitoService.GetUser(accessToken);
    if (cognitoUserResult.IsFailure)
    {
      return ApiResult<User>.Failure(ApiResultErrors.Unauthorized);
    }

    User user = _usersConverter.FromCognitoUser(cognitoUserResult.Value);

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
