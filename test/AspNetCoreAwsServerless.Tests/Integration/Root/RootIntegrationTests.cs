// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Xunit;
// using Xunit.Abstractions;

// namespace AspNetCoreAwsServerless.Tests.Integration.Root;

// public class RootIntegrationTests(WebApplicationFactory<Program> factory)
//     : IClassFixture<WebApplicationFactory<Program>>
// {
//   private readonly IntegrationTestsClientFactory _factory = new(factory);

//   [Fact]
//   public async Task Get_Secure_ReturnsForbidden()
//   {
//     HttpClient client = _factory.CreateClient();
//     string uri = "/secure";

//     HttpResponseMessage response = await client.GetAsync(uri);

//     Assert.False(response.IsSuccessStatusCode);
//     Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
//   }
// }
