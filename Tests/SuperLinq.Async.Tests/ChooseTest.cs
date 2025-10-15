using System.Globalization;

namespace SuperLinq.Async.Tests;

public sealed class ChooseTest
{
	[Fact]
	public void IsLazy()
	{
		_ = new AsyncBreakingSequence<object>()
			.Choose(BreakingFunc.Of<object, (bool, object)>());
	}

	[Fact]
	public async Task WithEmptySource()
	{
		await using var xs = TestingSequence.Of<int>();
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
			   .AsTestingSequence();

		await xs
			.Choose(s => (int.TryParse(s, NumberStyles.Integer,
										  CultureInfo.InvariantCulture,
										  out var n), n))
			.AssertSequenceEqual(2, 3, 4, 6, 7, 9);
	}

	[Fact]
	public async Task ThoseThatAreIntegers()
	{
		await using var xs = TestingSequence.Of<int?>(4, 1, 2, null, 4, null, 6, null, null, 9);

		await xs.Choose(e => e is { } n ? (true, n) : (false, default))
			.AssertSequenceEqual(4, 1, 2, 4, 6, 9);
	}

	[Fact]
	public async Task ThoseEven()
	{
		await using var xs = AsyncEnumerable.Range(1, 10)
			.AsTestingSequence();

		await xs.Choose(x => x % 2 is 0 ? (true, x) : (false, default))
			.AssertSequenceEqual(2, 4, 6, 8, 10);
	}
}
