using AspNetCoreAwsServerless.Utils.Result;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Primitives;

namespace AspNetCoreAwsServerless.Tests.Unit.Extensions.ApiResultExtensions;

public class ApiResultAssertions(ApiResult result) : ReferenceTypeAssertions<ApiResult, ApiResultAssertions>(result)
{
  private ApiResult _result = result;
  protected override string Identifier => "ApiResultAssertions";

  [CustomAssertion]
  public AndConstraint<ApiResultAssertions> HaveSucceeded()
  {
    _result.IsSuccess.Should().BeTrue();
    return new AndConstraint<ApiResultAssertions>(this);
  }

  [CustomAssertion]
  public AndConstraint<ApiResultAssertions> HaveFailed()
  {
    _result.IsSuccess.Should().BeFalse();
    return new AndConstraint<ApiResultAssertions>(this);
  }

  [CustomAssertion]
  public AndConstraint<ApiResultAssertions> HaveStatusCode(int expected)
  {
    _result.Errors?.StatusCode.Should().Be(expected);
    return new AndConstraint<ApiResultAssertions>(this);
  }
}

public class ApiResultAssertions<T>(ApiResult<T> result) : ReferenceTypeAssertions<ApiResult<T>, ApiResultAssertions<T>>(result)
{
  private ApiResult<T> _result = result;
  protected override string Identifier => "ApiResultAssertions";

  [CustomAssertion]
  public AndConstraint<ApiResultAssertions<T>> HaveSucceeded()
  {
    _result.IsSuccess.Should().BeTrue();
    return new AndConstraint<ApiResultAssertions<T>>(this);
  }

  [CustomAssertion]
  public AndConstraint<ApiResultAssertions<T>> HaveFailed()
  {
    _result.IsSuccess.Should().BeFalse();
    return new AndConstraint<ApiResultAssertions<T>>(this);
  }

  [CustomAssertion]
  public AndConstraint<ApiResultAssertions<T>> HaveValue(T expected)
  {
    _result.Value.Should().BeEquivalentTo(expected);
    return new AndConstraint<ApiResultAssertions<T>>(this);
  }

  [CustomAssertion]
  public AndConstraint<ApiResultAssertions<T>> HaveValue(T expected, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> options)
  {
    _result.Value.Should().BeEquivalentTo(expected, options);
    return new AndConstraint<ApiResultAssertions<T>>(this);
  }

  [CustomAssertion]
  public AndConstraint<ApiResultAssertions<T>> HaveStatusCode(int expected)
  {
    _result.Errors?.StatusCode.Should().Be(expected);
    return new AndConstraint<ApiResultAssertions<T>>(this);
  }

  [CustomAssertion]
  public AndConstraint<ApiResultAssertions<T>> HaveErrors(ApiResultErrors expected)
  {
    _result.Errors.Should().BeEquivalentTo(expected);
    return new AndConstraint<ApiResultAssertions<T>>(this);
  }
}
