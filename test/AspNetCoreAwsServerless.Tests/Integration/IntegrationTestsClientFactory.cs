using AspNetCoreAwsServerless.Dtos.Cognito;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Services.Cognito;
using AspNetCoreAwsServerless.Tests.Utils.Logger;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using Xunit.Abstractions;

namespace AspNetCoreAwsServerless.Tests.Integration;

public class IntegrationTestsClientFactory
{
  private readonly WebApplicationFactory<Program> _factory;

  public IntegrationTestsClientFactory(WebApplicationFactory<Program> factory)
  {
    _factory = factory.WithWebHostBuilder(builder =>
    {
      Mock<ICognitoService> _cognitoService = new();

      // sub: 1234567890, name: John Doe, email: email@email.email, iat: 1516239022
      string idToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJlbWFpbCI6ImVtYWlsQGVtYWlsLmVtYWlsIn0.KQ1MdffxjoHJnUaea1CKolmtLcYhAY-6gC3aOIhcPvE";
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
