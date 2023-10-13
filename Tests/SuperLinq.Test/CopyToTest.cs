using System.Collections.ObjectModel;

namespace Test;

#pragma warning disable IDE0034

public class CopyToTest
{
	[Fact]
	public void NullArgumentTest()
	{
		_ = Assert.Throws<ArgumentNullException>(
			"source",
			() => default(IEnumerable<int>)!.CopyTo(Array.Empty<int>()));
		_ = Assert.Throws<ArgumentNullException>(
			"source",
			() => default(IEnumerable<int>)!.CopyTo((IList<int>)[]));
		_ = Assert.Throws<ArgumentNullException>(
			"source",
			() => default(IEnumerable<int>)!.CopyTo(Array.Empty<int>(), 1));
		_ = Assert.Throws<ArgumentNullException>(
			"source",
			() => default(IEnumerable<int>)!.CopyTo(Array.Empty<int>().AsSpan()));
		_ = Assert.Throws<ArgumentNullException>(
			"array",
			() => Seq<int>().CopyTo(default(int[])!));
		_ = Assert.Throws<ArgumentNullException>(
			"list",
			() => Seq<int>().CopyTo(default(IList<int>)!));
		_ = Assert.Throws<ArgumentNullException>(
			"list",
			() => Seq<int>().CopyTo(default(IList<int>)!, 1));
	}

	[Fact]
	public void ThrowsOnNegativeIndex()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(
			"index",
			() => Seq<int>().CopyTo(Array.Empty<int>(), -1));
	}

	[Fact]
	public void ThrowsOnTooMuchDataForArray()
	{
		_ = Assert.Throws<ArgumentException>(
			() => Seq(1).CopyTo(Array.Empty<int>()));
		_ = Assert.Throws<ArgumentException>(
			() => new List<int> { 1 }.AsEnumerable().CopyTo(Array.Empty<int>()));
		_ = Assert.Throws<ArgumentException>(
			() => new List<int> { 1 }.AsReadOnly().AsEnumerable().CopyTo(Array.Empty<int>()));
	}

	[Fact]
	public void ThrowsOnTooMuchDataForIListArray()
	{
#pragma warning disable IDE0301 // Simplify collection initialization
		_ = Assert.ThrowsAny<ArgumentException>(
			() => Seq(1).CopyTo((IList<int>)Array.Empty<int>()));
		_ = Assert.ThrowsAny<ArgumentException>(
			() => Enumerable.Range(1, 1).CopyTo((IList<int>)Array.Empty<int>()));
		_ = Assert.ThrowsAny<ArgumentException>(
			() => new List<int> { 1 }.AsEnumerable().CopyTo((IList<int>)Array.Empty<int>()));
		_ = Assert.ThrowsAny<ArgumentException>(
			() => new List<int> { 1 }.AsReadOnly().AsEnumerable().CopyTo((IList<int>)Array.Empty<int>()));
#pragma warning restore IDE0301 // Simplify collection initialization
	}

	[Fact]
	public void ThrowsOnTooMuchDataForSpan()
	{
		_ = Assert.Throws<ArgumentException>(
			() => Seq(1).CopyTo([]));
		_ = Assert.Throws<ArgumentException>(
			() => new List<int> { 1 }.AsEnumerable().CopyTo([]));
	}

	[Fact]
	public void CopiesDataToSpan()
	{
		Span<int> span = stackalloc int[4];

		using var xs = Seq(1).AsTestingSequence();
		var cnt = xs.CopyTo(span);
		span.ToArray().AssertSequenceEqual(1, 0, 0, 0);
		Assert.Equal(1, cnt);

		cnt = new List<int> { 2 }.AsEnumerable().CopyTo(span[1..]);
		span.ToArray().AssertSequenceEqual(1, 2, 0, 0);
		Assert.Equal(1, cnt);

		cnt = new[] { 3, 4, }.AsEnumerable().CopyTo(span[2..]);
		span.ToArray().AssertSequenceEqual(1, 2, 3, 4);
		Assert.Equal(2, cnt);
	}

	[Fact]
	public void CopiesDataToArray()
	{
		var array = new int[4];

		using var xs = Seq(1).AsTestingSequence();
		var cnt = xs.CopyTo(array);
		array.AssertSequenceEqual(1, 0, 0, 0);
		Assert.Equal(1, cnt);

		cnt = new List<int> { 2 }.AsEnumerable().CopyTo(array, 1);
		array.AssertSequenceEqual(1, 2, 0, 0);
		Assert.Equal(1, cnt);

		cnt = Enumerable.Range(3, 1).CopyTo(array, 2);
		array.AssertSequenceEqual(1, 2, 3, 0);
		Assert.Equal(1, cnt);

		cnt = new[] { 4, }.AsEnumerable().CopyTo(array, 3);
		array.AssertSequenceEqual(1, 2, 3, 4);
		Assert.Equal(1, cnt);
	}

	[Fact]
	public void CopiesDataToList()
	{
		var list = new List<int>();

		var cnt = new[] { 1, }.AsEnumerable().CopyTo(list);
		list.AssertSequenceEqual(1);
		Assert.Equal(1, cnt);

		using (var xs = TestingSequence.Of(2))
		{
			cnt = xs.CopyTo(list, 1);
			list.AssertSequenceEqual(1, 2);
			Assert.Equal(1, cnt);
		}

		using (var xs = TestingSequence.Of(3, 4))
		{
			cnt = xs.AsEnumerable().CopyTo(list, 1);
			list.AssertSequenceEqual(1, 3, 4);
			Assert.Equal(2, cnt);
		}
	}

	[Fact]
	public void CopiesDataToIList()
	{
		var list = new Collection<int>();

		using (var xs = TestingSequence.Of(1))
		{
			var cnt = xs.CopyTo(list);
			list.AssertSequenceEqual(1);
			Assert.Equal(1, cnt);
		}

		using (var xs = TestingSequence.Of(2))
		{
			var cnt = xs.CopyTo(list, 1);
			list.AssertSequenceEqual(1, 2);
			Assert.Equal(1, cnt);
		}

		using (var xs = TestingSequence.Of(3, 4))
		{
			var cnt = xs.CopyTo(list, 1);
			list.AssertSequenceEqual(1, 3, 4);
			Assert.Equal(2, cnt);
		}
	}
}
