using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AspNetCoreAwsServerless.Dtos.Cognito;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Services.Cognito;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace AspNetCoreAwsServerless.Tests.Integration;

public class IntegrationTestsClientFactory(WebApplicationFactory<Program> factory)
{
  private readonly WebApplicationFactory<Program> _factory = factory.WithWebHostBuilder(builder =>
  {
    Mock<ICognitoService> _cognitoService = new();

    _cognitoService.Setup(service => service.GetTokens(It.IsAny<LoginDto>()))
      .ReturnsAsync((LoginDto dto) => dto.Code == "good"
        ? ApiResult<CognitoTokens>.Success(GenerateTestAccountToken())
        : ApiResultErrors.Unauthorized);

    builder.ConfigureTestServices(services =>
    {
      //Populate with mock services

      // Mock Cognito Service because it relies on an auth code from the frontend
      services.RemoveAll<ICognitoService>();
      services.AddScoped(_ => _cognitoService.Object);
    });
  });

  public HttpClient CreateClient()
  {
    _factory.Server.PreserveExecutionContext = true;
    return _factory.CreateClient();
  }

  public HttpClient CreateClient(WebApplicationFactoryClientOptions options)
  {
    return _factory.CreateClient(options);
  }

  private static CognitoTokens GenerateTestAccountToken()
  {
    JwtSecurityTokenHandler handler = new();
    List<Claim> claims =
    [
      new Claim(JwtRegisteredClaimNames.Sub, "3a69e6f1-6cf4-4b30-9724-27b937916849"),
      new Claim(JwtRegisteredClaimNames.Name, "Colby Test Account"),
      new Claim(JwtRegisteredClaimNames.Email, "birkheadc@gmail.com"),
      new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
    ];

    var token = new JwtSecurityToken(claims: claims);

    return new CognitoTokens()
    {
      IdToken = handler.WriteToken(token),
      AccessToken = "accessToken",
      RefreshToken = "refreshToken",
      ExpiresIn = 300
    };
  }
}
