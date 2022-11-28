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
			() => new List<int> { 1 }.CopyTo(Array.Empty<int>()));
		Assert.Throws<ArgumentException>(
			() => new List<int> { 1 }.AsReadOnly().CopyTo(Array.Empty<int>()));
	}

	[Fact]
	public void ThrowsOnTooMuchDataForIListArray()
	{
		Assert.Throws<ArgumentException>(
			() => Seq(1).CopyTo((IList<int>)Array.Empty<int>()));
		Assert.Throws<ArgumentException>(
			() => new List<int> { 1 }.CopyTo((IList<int>)Array.Empty<int>()));
		Assert.Throws<ArgumentException>(
			() => new List<int> { 1 }.AsReadOnly().CopyTo((IList<int>)Array.Empty<int>()));
	}

	[Fact]
	public void ThrowsOnTooMuchDataForSpan()
	{
		Assert.Throws<ArgumentException>(
			() => Seq(1).CopyTo(Array.Empty<int>().AsSpan()));
		Assert.Throws<ArgumentException>(
			() => new List<int> { 1 }.CopyTo(Array.Empty<int>().AsSpan()));
	}

	[Fact]
	public void CopiesDataToSpan()
	{
		Span<int> span = stackalloc int[4];

		Seq(1).CopyTo(span);
		span.ToArray().AssertSequenceEqual(1, 0, 0, 0);

		new List<int> { 2 }.CopyTo(span[1..]);
		span.ToArray().AssertSequenceEqual(1, 2, 0, 0);

		Enumerable.Range(3, 1).CopyTo(span[2..]);
		span.ToArray().AssertSequenceEqual(1, 2, 3, 0);
	}

	[Fact]
	public void CopiesDataToArray()
	{
		var array = new int[4];

		Seq(1).CopyTo(array);
		array.AssertSequenceEqual(1, 0, 0, 0);

		new List<int> { 2 }.CopyTo(array, 1);
		array.AssertSequenceEqual(1, 2, 0, 0);

		new List<int> { 3 }.AsReadOnly().CopyTo(array, 2);
		array.AssertSequenceEqual(1, 2, 3, 0);

		Enumerable.Range(4, 1).CopyTo(array, 3);
		array.AssertSequenceEqual(1, 2, 3, 4);
	}

	[Fact]
	public void CopiesDataToList()
	{
		var list = new List<int>();

		Seq(1).CopyTo(list);
		list.AssertSequenceEqual(1);

		Seq(2).CopyTo(list, 1);
		list.AssertSequenceEqual(1, 2);

		Seq(3, 4).CopyTo(list, 1);
		list.AssertSequenceEqual(1, 3, 4);
	}

	[Fact]
	public void CopiesDataToIList()
	{
		var list = new Collection<int>();

		Seq(1).CopyTo(list);
		list.AssertSequenceEqual(1);

		Seq(2).CopyTo(list, 1);
		list.AssertSequenceEqual(1, 2);

		Seq(3, 4).CopyTo(list, 1);
		list.AssertSequenceEqual(1, 3, 4);
	}
}
