using System.Globalization;
using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class ChooseTest
{
	[Test]
	public void IsLazy()
	{
		new BreakingSequence<object>()
			.Choose(BreakingFunc.Of<object, (bool, object)>());
	}

	[Test]
	public void WithEmptySource()
	{
		using var xs = Enumerable.Empty<int>().AsTestingSequence();
		Assert.That(xs.Choose(BreakingFunc.Of<int, (bool, int)>()), Is.Empty);
	}

	[Test]
	public void None()
	{
		using var xs = Enumerable.Range(1, 10).AsTestingSequence();
		Assert.That(xs.Choose(_ => (false, 0)), Is.Empty);
	}

	[Test]
	public void ThoseParsable()
	{
		using var xs =
			"O,l,2,3,4,S,6,7,B,9"
			   .Split(',')
			   .Choose(s => (int.TryParse(s, NumberStyles.Integer,
										  CultureInfo.InvariantCulture,
										  out var n), n))
			   .AsTestingSequence();

		xs.AssertSequenceEqual(2, 3, 4, 6, 7, 9);
	}

	// A cheap trick to masquerade a tuple as an option

	static class Option
	{
		public static (bool IsSome, T Value) Some<T>(T value) => (true, value);
	}

	static class Option<T>
	{
		public static readonly (bool IsSome, T Value) None = (false, default);
	}

	[Test]
	public void ThoseThatAreIntegers()
	{
		new int?[] { 0, 1, 2, null, 4, null, 6, null, null, 9 }
			.Choose(e => e is { } n ? Option.Some(n) : Option<int>.None)
			.AssertSequenceEqual(0, 1, 2, 4, 6, 9);
	}

	[Test]
	public void ThoseEven()
	{
		Enumerable.Range(1, 10)
				  .Choose(x => x % 2 is 0 ? Option.Some(x) : Option<int>.None)
				  .AssertSequenceEqual(2, 4, 6, 8, 10);
	}
}
