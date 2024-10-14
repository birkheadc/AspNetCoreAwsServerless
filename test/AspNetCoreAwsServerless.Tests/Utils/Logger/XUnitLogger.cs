using Xunit.Abstractions;

namespace AspNetCoreAwsServerless.Tests.Utils.Logger;

public class XunitLogger<T>(ITestOutputHelper output) : ILogger<T>, IDisposable
{
  private ITestOutputHelper _output = output;

  public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) where TState : notnull
  {
    _output.WriteLine(state.ToString());
  }

  public bool IsEnabled(LogLevel logLevel)
  {
    return true;
  }

  public IDisposable BeginScope<TState>(TState state) where TState : notnull
  {
    return this;
  }

  public void Dispose()
  {
  }
}