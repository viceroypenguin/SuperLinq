namespace SuperLinq.Tests;

public sealed class FoldTest
{
	[Test]
	public void FoldWithTooFewItems()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			Enumerable.Range(1, 3).Fold(BreakingFunc.Of<int, int, int, int, int>()));
	}

	[Test]
	public void FoldWithEmptySequence()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			Enumerable.Empty<int>().Fold(BreakingFunc.Of<int, int>()));
	}

	[Test]
	public void FoldWithTooManyItems()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			Enumerable.Range(1, 3).Fold(BreakingFunc.Of<int, int, int>()));
	}

	[Test]
	public void Fold()
	{
		const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

		using (var ts = Alphabet.Take(1).AsTestingSequence()) Assert.Equal("a", ts.Fold(a => string.Join("", a)));
		using (var ts = Alphabet.Take(2).AsTestingSequence()) Assert.Equal("ab", ts.Fold((a, b) => string.Join("", a, b)));
		using (var ts = Alphabet.Take(3).AsTestingSequence()) Assert.Equal("abc", ts.Fold((a, b, c) => string.Join("", a, b, c)));
		using (var ts = Alphabet.Take(4).AsTestingSequence()) Assert.Equal("abcd", ts.Fold((a, b, c, d) => string.Join("", a, b, c, d)));
		using (var ts = Alphabet.Take(5).AsTestingSequence()) Assert.Equal("abcde", ts.Fold((a, b, c, d, e) => string.Join("", a, b, c, d, e)));
		using (var ts = Alphabet.Take(6).AsTestingSequence()) Assert.Equal("abcdef", ts.Fold((a, b, c, d, e, f) => string.Join("", a, b, c, d, e, f)));
		using (var ts = Alphabet.Take(7).AsTestingSequence()) Assert.Equal("abcdefg", ts.Fold((a, b, c, d, e, f, g) => string.Join("", a, b, c, d, e, f, g)));
		using (var ts = Alphabet.Take(8).AsTestingSequence()) Assert.Equal("abcdefgh", ts.Fold((a, b, c, d, e, f, g, h) => string.Join("", a, b, c, d, e, f, g, h)));
		using (var ts = Alphabet.Take(9).AsTestingSequence()) Assert.Equal("abcdefghi", ts.Fold((a, b, c, d, e, f, g, h, i) => string.Join("", a, b, c, d, e, f, g, h, i)));
		using (var ts = Alphabet.Take(10).AsTestingSequence()) Assert.Equal("abcdefghij", ts.Fold((a, b, c, d, e, f, g, h, i, j) => string.Join("", a, b, c, d, e, f, g, h, i, j)));
		using (var ts = Alphabet.Take(11).AsTestingSequence()) Assert.Equal("abcdefghijk", ts.Fold((a, b, c, d, e, f, g, h, i, j, k) => string.Join("", a, b, c, d, e, f, g, h, i, j, k)));
		using (var ts = Alphabet.Take(12).AsTestingSequence()) Assert.Equal("abcdefghijkl", ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l) => string.Join("", a, b, c, d, e, f, g, h, i, j, k, l)));
		using (var ts = Alphabet.Take(13).AsTestingSequence()) Assert.Equal("abcdefghijklm", ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m) => string.Join("", a, b, c, d, e, f, g, h, i, j, k, l, m)));
		using (var ts = Alphabet.Take(14).AsTestingSequence()) Assert.Equal("abcdefghijklmn", ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => string.Join("", a, b, c, d, e, f, g, h, i, j, k, l, m, n)));
		using (var ts = Alphabet.Take(15).AsTestingSequence()) Assert.Equal("abcdefghijklmno", ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => string.Join("", a, b, c, d, e, f, g, h, i, j, k, l, m, n, o)));
		using (var ts = Alphabet.Take(16).AsTestingSequence()) Assert.Equal("abcdefghijklmnop", ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => string.Join("", a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p)));
	}
}
