namespace SuperLinq.Tests;

public sealed class FinallyTest
{
	[Test]
	public void FinallyIsLazy()
	{
		_ = new BreakingSequence<int>().Finally(BreakingAction.Of());
	}

	[Test]
	public void FinallyExecutesOnCompletion()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var ran = false;
		var result = seq.Finally(() => ran = true);

		Assert.False(ran);
		result.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.True(ran);
	}

	[Test]
	public void FinallyExecutesOnException()
	{
		using var seq = SeqExceptionAt(4).AsTestingSequence();

		var ran = false;
		var result = seq.Finally(() => ran = true);

		Assert.False(ran);
		_ = Assert.Throws<TestException>(() =>
		{
			var i = 1;
			foreach (var item in result)
				Assert.Equal(i++, item);
		});
		Assert.True(ran);
	}

	[Test]
	public void FinallyExecutesOnTake()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var ran = false;
		var result = seq.Finally(() => ran = true);

		Assert.False(ran);
		result
			.Take(5)
			.AssertSequenceEqual(Enumerable.Range(1, 5));
		Assert.True(ran);
	}

	[Test]
	public void FinallyExecutesOnEarlyDisposal()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var ran = false;
		var result = seq.Finally(() => ran = true);

		Assert.False(ran);

		{
			using var iter = result.GetEnumerator();
			for (var i = 1; i < 5; i++)
			{
				Assert.True(iter.MoveNext());
				Assert.Equal(i, iter.Current);
			}
		}

		Assert.True(ran);
	}
}
