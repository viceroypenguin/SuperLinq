using System;
using System.Collections.ObjectModel;

namespace Test;

public class CopyToTest
{
	[Fact]
	public void NullArgumentTest()
	{
		Assert.Throws<ArgumentNullException>(
			"source",
			() => default(IEnumerable<int>)!.CopyTo(Array.Empty<int>()));
		Assert.Throws<ArgumentNullException>(
			"source",
			() => default(IEnumerable<int>)!.CopyTo((IList<int>)Array.Empty<int>()));
		Assert.Throws<ArgumentNullException>(
			"source",
			() => default(IEnumerable<int>)!.CopyTo(Array.Empty<int>(), 1));
		Assert.Throws<ArgumentNullException>(
			"source",
			() => default(IEnumerable<int>)!.CopyTo(Array.Empty<int>().AsSpan()));
		Assert.Throws<ArgumentNullException>(
			"array",
			() => Seq<int>().CopyTo(default(int[])!));
		Assert.Throws<ArgumentNullException>(
			"list",
			() => Seq<int>().CopyTo(default(IList<int>)!));
		Assert.Throws<ArgumentNullException>(
			"list",
			() => Seq<int>().CopyTo(default(IList<int>)!, 1));
	}

	[Fact]
	public void ThrowsOnNegativeIndex()
	{
		Assert.Throws<ArgumentOutOfRangeException>(
			"index",
			() => Seq<int>().CopyTo(Array.Empty<int>(), -1));
	}

	[Fact]
	public void ThrowsOnTooMuchDataForArray()
	{
		Assert.Throws<ArgumentException>(
			() => Seq(1).CopyTo(Array.Empty<int>()));
		Assert.Throws<ArgumentException>(
			() => new List<int> { 1 }.AsEnumerable().CopyTo(Array.Empty<int>()));
		Assert.Throws<ArgumentException>(
			() => new List<int> { 1 }.AsReadOnly().AsEnumerable().CopyTo(Array.Empty<int>()));
	}

	[Fact]
	public void ThrowsOnTooMuchDataForIListArray()
	{
		Assert.Throws<ArgumentException>(
			() => Seq(1).CopyTo((IList<int>)Array.Empty<int>()));
		Assert.Throws<ArgumentException>(
			() => Enumerable.Range(1, 1).CopyTo((IList<int>)Array.Empty<int>()));
		Assert.Throws<ArgumentException>(
			() => new List<int> { 1 }.AsEnumerable().CopyTo((IList<int>)Array.Empty<int>()));
		Assert.Throws<ArgumentException>(
			() => new List<int> { 1 }.AsReadOnly().AsEnumerable().CopyTo((IList<int>)Array.Empty<int>()));
	}

	[Fact]
	public void ThrowsOnTooMuchDataForSpan()
	{
		Assert.Throws<ArgumentException>(
			() => Seq(1).CopyTo(Array.Empty<int>().AsSpan()));
		Assert.Throws<ArgumentException>(
			() => new List<int> { 1 }.AsEnumerable().CopyTo(Array.Empty<int>().AsSpan()));
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

		cnt = Enumerable.Range(3, 2).CopyTo(span[2..]);
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

		cnt = new List<int> { 3 }.AsReadOnly().AsEnumerable().CopyTo(array, 2);
		array.AssertSequenceEqual(1, 2, 3, 0);
		Assert.Equal(1, cnt);

		cnt = Enumerable.Range(4, 1).CopyTo(array, 3);
		array.AssertSequenceEqual(1, 2, 3, 4);
		Assert.Equal(1, cnt);
	}

	[Fact]
	public void CopiesDataToList()
	{
		var list = new List<int>();

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
