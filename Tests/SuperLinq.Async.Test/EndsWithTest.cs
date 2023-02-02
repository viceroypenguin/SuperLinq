namespace Test.Async;

public class EndsWithTest
{
	[Theory]
	[InlineData(new[] { 1, 2, 3 }, new[] { 2, 3 }, true)]
	[InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, true)]
	[InlineData(new[] { 1, 2, 3 }, new[] { 0, 1, 2, 3 }, false)]
	public async Task EndsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second, bool expected)
	{
		await using var f = first.AsTestingSequence();
		Assert.Equal(expected, await f.EndsWith(second));
	}

	[Theory]
	[InlineData(new[] { '1', '2', '3' }, new[] { '2', '3' }, true)]
	[InlineData(new[] { '1', '2', '3' }, new[] { '1', '2', '3' }, true)]
	[InlineData(new[] { '1', '2', '3' }, new[] { '0', '1', '2', '3' }, false)]
	public async Task EndsWithWithChars(IEnumerable<char> first, IEnumerable<char> second, bool expected)
	{
		await using var f = first.AsTestingSequence();
		await using var s = second.AsTestingSequence();
		Assert.Equal(expected, await f.EndsWith(s));
	}

	[Theory]
	[InlineData("123", "23", true)]
	[InlineData("123", "123", true)]
	[InlineData("123", "0123", false)]
	public async Task EndsWithWithStrings(string first, string second, bool expected)
	{
		await using var f = first.AsTestingSequence();
		await using var s = second.AsTestingSequence();
		Assert.Equal(expected, await f.EndsWith(s));
	}

	[Fact]
	public async Task EndsWithReturnsTrueIfBothEmpty()
	{
		await using var f = TestingSequence.Of<int>();
		await using var s = TestingSequence.Of<int>();
		Assert.True(await f.EndsWith(s));
	}

	[Fact]
	public async Task EndsWithReturnsFalseIfOnlyFirstIsEmpty()
	{
		await using var f = TestingSequence.Of<int>();
		await using var s = TestingSequence.Of(1, 2, 3);
		Assert.False(await f.EndsWith(s));
	}

	[Theory]
	[InlineData("", "", true)]
	[InlineData("1", "", true)]
	public async Task EndsWithReturnsTrueIfSecondIsEmpty(string first, string second, bool expected)
	{
		await using var f = first.AsTestingSequence();
		await using var s = second.AsTestingSequence();
		Assert.Equal(expected, await f.EndsWith(s));
	}

	[Fact]
	public async Task EndsWithUsesSpecifiedEqualityComparerOrDefault()
	{
		var first = AsyncSeq(1, 2, 3);
		var second = AsyncSeq(4, 5, 6);

		Assert.False(await first.EndsWith(second));
		Assert.False(await first.EndsWith(second, comparer: null));
		Assert.False(await first.EndsWith(second, EqualityComparer.Create<int>(delegate { return false; })));
		Assert.True(await first.EndsWith(second, EqualityComparer.Create<int>(delegate { return true; })));
	}
}
