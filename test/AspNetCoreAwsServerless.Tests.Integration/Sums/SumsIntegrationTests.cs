// using System.Net.Http.Json;
// using AspNetCoreAwsServerless.Dtos.Sums;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Newtonsoft.Json;

// namespace AspNetCoreAwsServerless.Tests.Integration.Sums;

// public class SumsIntegrationTests(WebApplicationFactory<Program> factory)
//   : IClassFixture<WebApplicationFactory<Program>>
// {
//   private readonly IntegrationTestsClientFactory _factory = new(factory);

//   [Theory, MemberData(nameof(Post_ReturnsCorrectSum_Data))]
//   public async Task Post_ReturnsCorrectSum(SumCreateDto dto, int expected)
//   {
//     HttpClient client = _factory.CreateClient();
//     string uri = "/sums";

//     HttpResponseMessage response = await client.PostAsJsonAsync(uri, dto);
//     response.EnsureSuccessStatusCode();

//     string content = response.Content.ReadAsStringAsync().Result;
//     Assert.NotNull(content);

//     int actual = JsonConvert.DeserializeObject<int>(content);

//     Assert.Equal(expected, actual);
//   }

//   public static IEnumerable<object[]> Post_ReturnsCorrectSum_Data()
//   {
//     return
//     [
//       [new SumCreateDto() { Values = [0, 1, 2, 0] }, 3],
//       [new SumCreateDto() { Values = [100, 60, 1000, 1] }, 1161],
//       [new SumCreateDto() { Values = [0, 0, 0, 0, 0, 0, 0] }, 0]
//     ];
//   }
// }
