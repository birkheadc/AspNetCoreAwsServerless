using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using AspNetCoreAwsServerless.Config.Cognito;
using AspNetCoreAwsServerless.Dtos.Cognito;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.Extensions.Options;

namespace AspNetCoreAwsServerless.Services.Cognito;

public class CognitoService(
  IAmazonCognitoIdentityProvider cognitoClient,
  ILogger<CognitoService> logger,
  HttpClient httpClient,
  IOptions<CognitoOptions> config
) : ICognitoService
{
  private readonly IAmazonCognitoIdentityProvider _cognitoClient = cognitoClient;
  private readonly ILogger<CognitoService> _logger = logger;
  private readonly HttpClient _httpClient = httpClient;

  private readonly CognitoOptions _config = config.Value;

  public async Task<ApiResult<CognitoTokens>> GetTokens(LoginDto loginDto)
  {
    _logger.LogInformation("Getting tokens from Cognito: Code: {code} | RedirectUri: {redirectUri}", loginDto.Code, loginDto.RedirectUri);

    try
    {
      string url = $"{_config.Url}/oauth2/token";

      // Cognito requires the request body to be in this format
      string requestBody = $"grant_type=authorization_code&code={loginDto.Code}&client_id={_config.ClientId}&redirect_uri={loginDto.RedirectUri}";
      HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/x-www-form-urlencoded");

      content.Headers.Add("X-Amz-Target", "AWSCognitoIdentityProviderService.Client");

      HttpResponseMessage response = await _httpClient.PostAsync(url, content);

      string? responseString = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
      {
        _logger.LogError("Failed to get tokens from Cognito: {response}", responseString);
        return ApiResult<CognitoTokens>.Failure(ApiResultErrors.Unauthorized);
      }

      CognitoTokens? tokens = JsonSerializer.Deserialize<CognitoTokens>(responseString);

      if (tokens is null)
      {
        _logger.LogError("Failed to deserialize tokens");
        return ApiResult<CognitoTokens>.Failure(ApiResultErrors.Unauthorized);
      }

      return ApiResult<CognitoTokens>.Success(tokens);
    }
    catch (Exception exception)
    {
      _logger.LogError(exception, "Error getting tokens from Cognito");
      return ApiResult<CognitoTokens>.Failure(ApiResultErrors.Unauthorized);
    }
  }
}
