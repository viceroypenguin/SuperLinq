namespace Test.Async;

public class StartsWithTest
{
	[Theory]
	[InlineData(new[] { 1, 2, 3 }, new[] { 1, 2 }, true)]
	[InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, true)]
	[InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4 }, false)]
	public async Task StartsWithWithIntegers(IEnumerable<int> first, IEnumerable<int> second, bool expected)
	{
		Assert.Equal(expected, await first.ToAsyncEnumerable().StartsWith(second));
	}

	[Theory]
	[InlineData(new[] { '1', '2', '3' }, new[] { '1', '2' }, true)]
	[InlineData(new[] { '1', '2', '3' }, new[] { '1', '2', '3' }, true)]
	[InlineData(new[] { '1', '2', '3' }, new[] { '1', '2', '3', '4' }, false)]
	public async Task StartsWithWithChars(IEnumerable<char> first, IEnumerable<char> second, bool expected)
	{
		Assert.Equal(expected, await first.ToAsyncEnumerable().StartsWith(second));
	}

	[Theory]
	[InlineData("123", "12", true)]
	[InlineData("123", "123", true)]
	[InlineData("123", "1234", false)]
	public async Task StartsWithWithStrings(string first, string second, bool expected)
	{
		// Conflict with String.StartsWith(), which has precedence in this case
		Assert.Equal(expected, await first.ToAsyncEnumerable().StartsWith(second));
	}

	[Fact]
	public async Task StartsWithReturnsTrueIfBothEmpty()
	{
		Assert.True(await AsyncEnumerable.Empty<int>().StartsWith(Array.Empty<int>()));
	}

	[Fact]
	public async Task StartsWithReturnsFalseIfOnlyFirstIsEmpty()
	{
		Assert.False(await AsyncEnumerable.Empty<int>().StartsWith(new[] { 1, 2, 3 }));
	}

	[Theory]
	[InlineData("", "", true)]
	[InlineData("1", "", true)]
	public async Task StartsWithReturnsTrueIfSecondIsEmpty(string first, string second, bool expected)
	{
		// Conflict with String.StartsWith(), which has precedence in this case
		Assert.Equal(expected, await first.ToAsyncEnumerable().StartsWith(second));
	}

	[Fact]
	public async Task StartsWithDisposesBothSequenceEnumerators()
	{
		await using var first = TestingSequence.Of(1, 2, 3);
		await using var second = TestingSequence.Of(1);

		await first.StartsWith(second);
	}

	[Fact]
	public async Task StartsWithUsesSpecifiedEqualityComparerOrDefault()
	{
		var first = AsyncSeq(1, 2, 3);
		var second = AsyncSeq(4, 5, 6);

		Assert.False(await first.StartsWith(second));
		Assert.False(await first.StartsWith(second, comparer: null));
		Assert.False(await first.StartsWith(second, EqualityComparer.Create<int>(delegate { return false; })));
		Assert.True(await first.StartsWith(second, EqualityComparer.Create<int>(delegate { return true; })));
	}
}
