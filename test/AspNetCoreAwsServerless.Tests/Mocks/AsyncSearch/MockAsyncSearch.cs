using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;

namespace AspNetCoreAwsServerless.Tests.Mocks.AsyncSearch;

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
    Console.WriteLine($"Count: {Items.Count}");
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
}
