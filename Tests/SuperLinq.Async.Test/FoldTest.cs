namespace Test.Async;

public sealed class FoldTest
{
	[Fact]
	public async Task FoldWithTooFewItems()
	{
		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await AsyncEnumerable.Range(1, 3).Fold(AsyncBreakingFunc.Of<int, int, int, int, int>()));
	}

	[Fact]
	public async Task FoldWithEmptySequence()
	{
		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await AsyncEnumerable.Empty<int>().Fold(AsyncBreakingFunc.Of<int, int>()));
	}

	[Fact]
	public async Task FoldWithTooManyItems()
	{
		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await AsyncEnumerable.Range(1, 3).Fold(AsyncBreakingFunc.Of<int, int, int>()));
	}

	[Fact]
	public async Task Fold()
	{
		const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

		await using (var ts = Alphabet.Take(1).AsTestingSequence()) Assert.Equal("a", await ts.Fold(a => string.Join(string.Empty, a)));
		await using (var ts = Alphabet.Take(2).AsTestingSequence()) Assert.Equal("ab", await ts.Fold((a, b) => string.Join(string.Empty, a, b)));
		await using (var ts = Alphabet.Take(3).AsTestingSequence()) Assert.Equal("abc", await ts.Fold((a, b, c) => string.Join(string.Empty, a, b, c)));
		await using (var ts = Alphabet.Take(4).AsTestingSequence()) Assert.Equal("abcd", await ts.Fold((a, b, c, d) => string.Join(string.Empty, a, b, c, d)));
		await using (var ts = Alphabet.Take(5).AsTestingSequence()) Assert.Equal("abcde", await ts.Fold((a, b, c, d, e) => string.Join(string.Empty, a, b, c, d, e)));
		await using (var ts = Alphabet.Take(6).AsTestingSequence()) Assert.Equal("abcdef", await ts.Fold((a, b, c, d, e, f) => string.Join(string.Empty, a, b, c, d, e, f)));
		await using (var ts = Alphabet.Take(7).AsTestingSequence()) Assert.Equal("abcdefg", await ts.Fold((a, b, c, d, e, f, g) => string.Join(string.Empty, a, b, c, d, e, f, g)));
		await using (var ts = Alphabet.Take(8).AsTestingSequence()) Assert.Equal("abcdefgh", await ts.Fold((a, b, c, d, e, f, g, h) => string.Join(string.Empty, a, b, c, d, e, f, g, h)));
		await using (var ts = Alphabet.Take(9).AsTestingSequence()) Assert.Equal("abcdefghi", await ts.Fold((a, b, c, d, e, f, g, h, i) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i)));
		await using (var ts = Alphabet.Take(10).AsTestingSequence()) Assert.Equal("abcdefghij", await ts.Fold((a, b, c, d, e, f, g, h, i, j) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j)));
		await using (var ts = Alphabet.Take(11).AsTestingSequence()) Assert.Equal("abcdefghijk", await ts.Fold((a, b, c, d, e, f, g, h, i, j, k) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k)));
		await using (var ts = Alphabet.Take(12).AsTestingSequence()) Assert.Equal("abcdefghijkl", await ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l)));
		await using (var ts = Alphabet.Take(13).AsTestingSequence()) Assert.Equal("abcdefghijklm", await ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m)));
		await using (var ts = Alphabet.Take(14).AsTestingSequence()) Assert.Equal("abcdefghijklmn", await ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n)));
		await using (var ts = Alphabet.Take(15).AsTestingSequence()) Assert.Equal("abcdefghijklmno", await ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o)));
		await using (var ts = Alphabet.Take(16).AsTestingSequence()) Assert.Equal("abcdefghijklmnop", await ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p)));
	}
}
