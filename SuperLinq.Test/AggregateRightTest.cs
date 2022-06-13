using NUnit.Framework;

namespace Test;

[TestFixture]
public class AggregateRightTest
{
	// Overload 1 Test

	[Test]
	public void AggregateRightWithEmptySequence()
	{
		Assert.Throws<InvalidOperationException>(
			() => Array.Empty<int>().AggregateRight((a, b) => a + b));
	}

	[Test]
	public void AggregateRightFuncIsNotInvokedOnSingleElementSequence()
	{
		const int value = 1;

		var result = new[] { value }.AggregateRight(BreakingFunc.Of<int, int, int>());

		Assert.That(result, Is.EqualTo(value));
	}

	[TestCase(SourceKind.BreakingList)]
	[TestCase(SourceKind.BreakingReadOnlyList)]
	[TestCase(SourceKind.Sequence)]
	public void AggregateRight(SourceKind sourceKind)
	{
		var enumerable = Enumerable.Range(1, 5).Select(x => x.ToString()).ToSourceKind(sourceKind);

		var result = enumerable.AggregateRight((a, b) => string.Format("({0}+{1})", a, b));

		Assert.That(result, Is.EqualTo("(1+(2+(3+(4+5))))"));
	}

	// Overload 2 Test

	[TestCase(5)]
	[TestCase("c")]
	[TestCase(true)]
	public void AggregateRightSeedWithEmptySequence(object defaultValue)
	{
		Assert.That(Array.Empty<int>().AggregateRight(defaultValue, (a, b) => b), Is.EqualTo(defaultValue));
	}

	[Test]
	public void AggregateRightSeedFuncIsNotInvokedOnEmptySequence()
	{
		const int value = 1;

		var result = Array.Empty<int>().AggregateRight(value, BreakingFunc.Of<int, int, int>());

		Assert.That(result, Is.EqualTo(value));
	}

	[Test]
	public void AggregateRightSeed()
	{
		var result = Enumerable.Range(1, 4)
							   .AggregateRight("5", (a, b) => string.Format("({0}+{1})", a, b));

		Assert.That(result, Is.EqualTo("(1+(2+(3+(4+5))))"));
	}

	// Overload 3 Test

	[TestCase(5)]
	[TestCase("c")]
	[TestCase(true)]
	public void AggregateRightResultorWithEmptySequence(object defaultValue)
	{
		Assert.That(Array.Empty<int>().AggregateRight(defaultValue, (a, b) => b, a => a == defaultValue), Is.EqualTo(true));
	}

	[Test]
	public void AggregateRightResultor()
	{
		var result = Enumerable.Range(1, 4)
							   .AggregateRight("5", (a, b) => string.Format("({0}+{1})", a, b), a => a.Length);

		Assert.That(result, Is.EqualTo("(1+(2+(3+(4+5))))".Length));
	}
}
