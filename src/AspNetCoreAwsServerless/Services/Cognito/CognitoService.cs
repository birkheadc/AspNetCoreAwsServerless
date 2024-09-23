using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using AspNetCoreAwsServerless.Dtos.CognitoUser;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Services.Cognito;

public class CognitoService(IAmazonCognitoIdentityProvider cognitoClient, ILogger<CognitoService> logger) : ICognitoService
{
  private readonly IAmazonCognitoIdentityProvider _cognitoClient = cognitoClient;
  private readonly ILogger<CognitoService> _logger = logger;

  public async Task<ApiResult<CognitoUser>> GetUser(string accessToken)
  {
    _logger.LogInformation("Getting user from Cognito: AccessToken? {AccessToken}", accessToken is not null);
    var response = await _cognitoClient.GetUserAsync(new GetUserRequest { AccessToken = accessToken });
    string username = response.Username;
    string? emailAddress = response.UserAttributes.FirstOrDefault(attr => attr.Name == "email")?.Value;
    _logger.LogInformation("Got user from Cognito: Username: {Username}, EmailAddress: {EmailAddress}", username, emailAddress);
    if (emailAddress is null)
    {
      _logger.LogError("User does not have an email address");
      return ApiResult<CognitoUser>.Failure(ApiResultErrors.Unauthorized);
    }
    return ApiResult<CognitoUser>.Success(new CognitoUser() { EmailAddress = emailAddress, Username = username });
  }
}
