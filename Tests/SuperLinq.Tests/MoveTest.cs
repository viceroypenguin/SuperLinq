namespace SuperLinq.Tests;

public sealed class MoveTest
{
	[Test]
	public void MoveWithNegativeFromIndex()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new[] { 1 }.Move(-1, 0, 0));
	}

	[Test]
	public void MoveWithNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new[] { 1 }.Move(0, -1, 0));
	}

	[Test]
	public void MoveWithNegativeToIndex()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new[] { 1 }.Move(0, 0, -1));
	}

	[Test]
	public void MoveIsLazy()
	{
		_ = new BreakingSequence<int>().Move(0, 0, 0);
	}

	[Test]
	[MethodDataSource(nameof(MoveSource))]
	public void Move(int length, int fromIndex, int count, int toIndex)
	{
		var source = Enumerable.Range(0, length);

		using var test = source.AsTestingSequence();

		var result = test.Move(fromIndex, count, toIndex);

		var slice = source.Skip(fromIndex).Take(count);
		var exclude = source.Take(fromIndex).Concat(source.Skip(fromIndex + count));
		var expectations = exclude.Take(toIndex).Concat(slice).Concat(exclude.Skip(toIndex));
		result.AssertSequenceEqual(expectations);
	}

	public static IEnumerable<(int length, int fromIndex, int count, int toIndex)> MoveSource()
	{
		const int Length = 10;
		return
			from index in Enumerable.Range(0, Length)
			from count in Enumerable.Range(0, Length + 1)
			from tcd in new (int length, int fromIndex, int count, int toIndex)[]
			{
				(Length, index, count, Math.Max(0, index - 1)),
				(Length, index, count, index + 1),
			}
			select tcd;
	}

	[Test]
	[MethodDataSource(nameof(MoveWithSequenceShorterThanToIndexSource))]
	public void MoveWithSequenceShorterThanToIndex(int length, int fromIndex, int count, int toIndex)
	{
		var source = Enumerable.Range(0, length);

		using var test = source.AsTestingSequence();

		var result = test.Move(fromIndex, count, toIndex);

		var expectations = source.Take(fromIndex)
			.Concat(source.Skip(fromIndex + count))
			.Concat(source.Skip(fromIndex).Take(count));
		Assert.Equal(expectations, result);
	}

	public static IEnumerable<(int length, int fromIndex, int count, int toIndex)> MoveWithSequenceShorterThanToIndexSource() =>
		Enumerable.Range(10, 10 + 5)
			.Select(toIndex => (10, 5, 2, toIndex));

	[Test]
	public void MoveIsRepeatable()
	{
		using var source = Enumerable.Range(0, 10).AsTestingSequence(maxEnumerations: 2);

		var result = source.Move(0, 5, 10);
		var array1 = result.ToArray();
		var array2 = result.ToArray();
		Assert.Equal(array1, array2);
	}

	[Test]
	public void MoveWithFromIndexEqualsToIndex()
	{
		using var source = Enumerable.Range(0, 10).AsTestingSequence();

		var result = source.Move(5, 999, 5);
		result.AssertSequenceEqual(Enumerable.Range(0, 10));
	}

	[Test]
	public void MoveWithCountEqualsZero()
	{
		using var source = Enumerable.Range(0, 10).AsTestingSequence();

		var result = source.Move(5, 0, 999);
		result.AssertSequenceEqual(Enumerable.Range(0, 10));
	}
}
