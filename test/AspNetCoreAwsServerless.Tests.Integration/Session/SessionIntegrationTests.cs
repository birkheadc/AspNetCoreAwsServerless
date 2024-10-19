using System.Text.Json;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Services.Cognito;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Moq.AutoMock;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Tests.Integration.Session;

public class SessionIntegrationTests
  : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly IntegrationTestsClientFactory _factory;
  private readonly AutoMocker _mocker;
  private readonly Mock<CognitoService> _cognitoService;
  public SessionIntegrationTests(WebApplicationFactory<Program> factory)
  {
    _mocker = new();
    _cognitoService = _mocker.GetMock<CognitoService>();
    _factory = new(factory);
  }

  [Fact]
  public async Task Login_Fails_When_CodeIsInvalid()
  {
    HttpClient client = _factory.CreateClient();
    string uri = "/session";

    var body = new LoginDto()
    {
      Code = "bad",
      RedirectUri = "redirect-uri"
    };

    HttpContent content = new StringContent(JsonSerializer.Serialize(body));
    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

    HttpResponseMessage response = await client.PostAsync(uri, content);

    Assert.False(response.IsSuccessStatusCode);
    Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
  }

  [Fact]
  public async Task Login_Succeeds_When_CodeIsValid()
  {
    HttpClient client = _factory.CreateClient();
    string uri = "/session";

    var body = new LoginDto()
    {
      Code = "good",
      RedirectUri = "redirect-uri"
    };

    HttpContent content = new StringContent(JsonSerializer.Serialize(body));
    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

    HttpResponseMessage response = await client.PostAsync(uri, content);

    Assert.True(response.IsSuccessStatusCode);
  }
}
