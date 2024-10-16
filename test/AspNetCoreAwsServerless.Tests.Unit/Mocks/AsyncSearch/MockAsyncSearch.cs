using Amazon.DynamoDBv2.DataModel;

namespace AspNetCoreAwsServerless.Tests.Unit.Mocks.AsyncSearch;

public class MockAsyncSearch<T> : AsyncSearch<T>
{
  public List<T> Items { get; set; }

  public MockAsyncSearch()
  {
    Items = [];
  }

  public MockAsyncSearch(List<T> items)
  {
    Items = items;
  }

  public override Task<List<T>> GetNextSetAsync(
    CancellationToken cancellationToken = default(CancellationToken)
  )
  {
    return Task.FromResult(Items);
  }

  public override Task<List<T>> GetRemainingAsync(
    CancellationToken cancellationToken = default(CancellationToken)
  )
  {
    return Task.FromResult(Items);
  }

  public override string PaginationToken => "MockPaginationToken";
}
