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

public class IntegrationTestsClientFactory
{
  private readonly WebApplicationFactory<Program> _factory;

  public IntegrationTestsClientFactory(WebApplicationFactory<Program> factory)
  {
    _factory = factory.WithWebHostBuilder(builder =>
    {
      Mock<ICognitoService> _cognitoService = new();

      // sub: 80a8b2e8-de5e-4cd8-87fb-4d80a6d2f787, name: John Doe, email: email@email.email, iat: 1516239022
      string idToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI4MGE4YjJlOC1kZTVlLTRjZDgtODdmYi00ZDgwYTZkMmY3ODciLCJuYW1lIjoiSm9obiBEb2UiLCJpYXQiOjE1MTYyMzkwMjIsImVtYWlsIjoiZW1haWxAZW1haWwuZW1haWwifQ.IgCqcGr2wjMjzdjoEyVApV0touvL5uyUZ2wRFitIEoo";
      _cognitoService.Setup(service => service.GetTokens(It.Is<LoginDto>(dto => dto.Code == "good"))).ReturnsAsync(ApiResult<CognitoTokens>.Success(new CognitoTokens()
      {
        AccessToken = "accessToken",
        RefreshToken = "refreshToken",
        IdToken = idToken,
        ExpiresIn = 300
      }));

      _cognitoService.Setup(service => service.GetTokens(It.Is<LoginDto>(dto => dto.Code != "good"))).ReturnsAsync(ApiResultErrors.Unauthorized);

      builder.ConfigureTestServices(services =>
      {
        //Populate with mock services, like database

        // Mock Cognito Service
        services.RemoveAll<ICognitoService>();
        services.AddScoped(_ => _cognitoService.Object);
      });
    });
  }

  public HttpClient CreateClient()
  {
    _factory.Server.PreserveExecutionContext = true;
    return _factory.CreateClient();
  }

  public HttpClient CreateClient(WebApplicationFactoryClientOptions options)
  {
    return _factory.CreateClient(options);
  }
}
