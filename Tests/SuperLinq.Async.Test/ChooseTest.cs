using System.Globalization;

namespace Test.Async;

public class ChooseTest
{
	[Fact]
	public void IsLazy()
	{
		new AsyncBreakingSequence<object>()
			.Choose(BreakingFunc.Of<object, (bool, object)>());
	}

	[Fact]
	public async Task WithEmptySource()
	{
		await using var xs = Enumerable.Empty<int>().AsTestingSequence();
		Assert.Empty(await xs.Choose(BreakingFunc.Of<int, (bool, int)>())
			.ToListAsync());
	}

	[Fact]
	public async Task None()
	{
		await using var xs = Enumerable.Range(1, 10).AsTestingSequence();
		Assert.Empty(await xs.Choose(_ => (false, 0)).ToListAsync());
	}

	[Fact]
	public async Task ThoseParsable()
	{
		await using var xs =
			"O,l,2,3,4,S,6,7,B,9"
			   .Split(',')
			   .ToAsyncEnumerable()
			   .Choose(s => (int.TryParse(s, NumberStyles.Integer,
										  CultureInfo.InvariantCulture,
										  out var n), n))
			   .AsTestingSequence();

		await xs.AssertSequenceEqual(2, 3, 4, 6, 7, 9);
	}

	// A cheap trick to masquerade a tuple as an option

	private static class Option
	{
		public static (bool IsSome, T Value) Some<T>(T value) => (true, value);
	}

	private static class Option<T>
	{
		public static readonly (bool IsSome, T? Value) None = (false, default);
	}

	[Fact]
	public Task ThoseThatAreIntegers()
	{
		return AsyncSeq<int?>(0, 1, 2, null, 4, null, 6, null, null, 9)
			.Choose(e => e is { } n ? Option.Some(n) : Option<int>.None)
			.AssertSequenceEqual(0, 1, 2, 4, 6, 9);
	}

	[Fact]
	public Task ThoseEven()
	{
		return AsyncEnumerable.Range(1, 10)
			.Choose(x => x % 2 is 0 ? Option.Some(x) : Option<int>.None)
			.AssertSequenceEqual(2, 4, 6, 8, 10);
	}
}
