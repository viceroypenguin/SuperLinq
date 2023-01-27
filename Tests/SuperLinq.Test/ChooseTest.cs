using System.Globalization;

namespace Test;

public class ChooseTest
{
	[Fact]
	public void IsLazy()
	{
		new BreakingSequence<object>()
			.Choose(BreakingFunc.Of<object, (bool, object)>());
	}

	[Fact]
	public void WithEmptySource()
	{
		using var xs = TestingSequence.Of<int>();
		Assert.Empty(xs.Choose(BreakingFunc.Of<int, (bool, int)>()));
	}

	[Fact]
	public void None()
	{
		using var xs = Enumerable.Range(1, 10).AsTestingSequence();
		Assert.Empty(xs.Choose(_ => (false, 0)));
	}

	[Fact]
	public void ThoseParsable()
	{
		using var xs =
			"O,l,2,3,4,S,6,7,B,9"
			   .Split(',')
			   .AsTestingSequence();

		xs
			.Choose(s => (int.TryParse(s, NumberStyles.Integer,
										  CultureInfo.InvariantCulture,
										  out var n), n))
			.AssertSequenceEqual(2, 3, 4, 6, 7, 9);
	}

	[Fact]
	public void ThoseThatAreIntegers()
	{
		using var xs = TestingSequence.Of<int?>(4, 1, 2, null, 4, null, 6, null, null, 9);

		xs.Choose(e => e is { } n ? (true, n) : (false, default))
			.AssertSequenceEqual(4, 1, 2, 4, 6, 9);
	}

	[Fact]
	public void ThoseEven()
	{
		using var xs = Enumerable.Range(1, 10)
			.AsTestingSequence();

		xs.Choose(x => x % 2 is 0 ? (true, x) : (false, default))
			.AssertSequenceEqual(2, 4, 6, 8, 10);
	}
}
