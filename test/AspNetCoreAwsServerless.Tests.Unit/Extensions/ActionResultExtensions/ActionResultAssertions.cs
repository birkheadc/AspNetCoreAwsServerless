using FluentAssertions;
using FluentAssertions.Primitives;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Tests.Unit.Extensions.ActionResultExtensions;

public class ActionResultAssertions(ActionResult result) : ReferenceTypeAssertions<ActionResult, ActionResultAssertions>(result)
{
  private readonly ActionResult _result = result;
  protected override string Identifier => "ActionResultAssertions";

  [CustomAssertion]
  public AndConstraint<ActionResultAssertions> HaveValue(object expected)
  {
    (_result as OkObjectResult)?.Value.Should().BeEquivalentTo(expected);
    return new AndConstraint<ActionResultAssertions>(this);
  }

  [CustomAssertion]
  public AndConstraint<ActionResultAssertions> HaveSucceeded()
  {
    _result.Should().BeOfType<OkResult>();
    return new AndConstraint<ActionResultAssertions>(this);
  }

  [CustomAssertion]
  public AndConstraint<ActionResultAssertions> HaveFailed()
  {
    (_result as ObjectResult)?.Value.Should().BeOfType<ProblemHttpResult>();
    return new AndConstraint<ActionResultAssertions>(this);
  }
}

public class ActionResultAssertions<T>(ActionResult<T> result) : ReferenceTypeAssertions<ActionResult<T>, ActionResultAssertions<T>>(result)
{
  private ActionResult<T> _result = result;
  protected override string Identifier => "ActionResultAssertions";

  [CustomAssertion]
  public AndConstraint<ActionResultAssertions<T>> HaveValue(T expected)
  {
    (_result as OkObjectResult)?.Value.Should().BeEquivalentTo(expected);
    return new AndConstraint<ActionResultAssertions<T>>(this);
  }

  [CustomAssertion]
  public AndConstraint<ActionResultAssertions<T>> HaveSucceeded()
  {
    _result.Should().BeOfType<OkResult>();
    return new AndConstraint<ActionResultAssertions<T>>(this);
  }

  [CustomAssertion]
  public AndConstraint<ActionResultAssertions<T>> HaveFailed()
  {
    (_result as ObjectResult)?.Value.Should().BeOfType<ProblemHttpResult>();
    return new AndConstraint<ActionResultAssertions<T>>(this);
  }
}
