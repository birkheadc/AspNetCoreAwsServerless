using System.Text.Json.Serialization;

namespace AspNetCoreAwsServerless.Dtos.Cognito;

public class CognitoTokens
{
  [JsonPropertyName("access_token")]
  public required string AccessToken { get; init; }
  [JsonPropertyName("refresh_token")]
  public required string RefreshToken { get; init; }
  [JsonPropertyName("id_token")]
  public required string IdToken { get; init; }
  [JsonPropertyName("expires_in")]
  public required int ExpiresIn { get; init; }
}
