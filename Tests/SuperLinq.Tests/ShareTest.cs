using System.Collections;

namespace SuperLinq.Tests;

public sealed class ShareTest
{
	[Fact]
	public void ShareIsLazy()
	{
		_ = new BreakingSequence<int>().Share();
	}

	[Fact]
	public void ShareWithSingleConsumer()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		using var result = seq.Share();
		result.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.Equal(0, result.Count);
	}

	[Fact]
	public void ShareWithMultipleConsumers()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		using var result = seq.Share();

		using var r1 = result.Read();
		using var r2 = result.Read();

		Assert.Equal(1, r1.Read());
		Assert.Equal(2, r2.Read());
		Assert.Equal(3, r2.Read());
		Assert.Equal(4, r1.Read());
		Assert.Equal(5, r1.Read());
		Assert.Equal(6, r1.Read());
		Assert.Equal(7, r2.Read());
		Assert.Equal(8, r2.Read());
		Assert.Equal(9, r1.Read());
		Assert.Equal(10, r2.Read());

		r1.ReadEnd();
		r2.ReadEnd();
	}

	[Fact]
	public void ShareWithInnerConsumer()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		using var result = seq.Share();

		using var r1 = result.Read();
		Assert.Equal(1, r1.Read());
		Assert.Equal(2, r1.Read());

		using (var r2 = result.Read())
		{
			Assert.Equal(3, r2.Read());
			Assert.Equal(4, r1.Read());
			Assert.Equal(5, r2.Read());
			Assert.Equal(6, r2.Read());
		}

		Assert.Equal(7, r1.Read());
		Assert.Equal(8, r1.Read());
		Assert.Equal(9, r1.Read());
		Assert.Equal(10, r1.Read());

		r1.ReadEnd();
	}

	[Fact]
	public void ShareWithSequentialPartialConsumers()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		using var result = seq.Share();

		using (var r1 = result.Read())
		{
			Assert.Equal(1, r1.Read());
			Assert.Equal(2, r1.Read());
			Assert.Equal(3, r1.Read());
			Assert.Equal(4, r1.Read());
			Assert.Equal(5, r1.Read());
		}

		using (var r2 = result.Read())
		{
			Assert.Equal(6, r2.Read());
			Assert.Equal(7, r2.Read());
			Assert.Equal(8, r2.Read());
			Assert.Equal(9, r2.Read());
			Assert.Equal(10, r2.Read());
			r2.ReadEnd();
		}

		using var r3 = result.Read();
		r3.ReadEnd();
	}

	[Fact]
	public void ShareDisposesAfterSourceIsIteratedEntirely()
	{
		using var seq = Enumerable.Range(0, 10).AsTestingSequence();

		using var buffer = seq.Share();
		buffer.Consume();

		Assert.True(seq.IsDisposed);
	}

	[Fact]
	public void ShareDisposesWithPartialEnumeration()
	{
		using var seq = Enumerable.Range(0, 10).AsTestingSequence();

		using var buffer = seq.Share();

		using (buffer)
			buffer.Take(5).Consume();

		Assert.True(seq.IsDisposed);
	}

	[Fact]
	public void ShareRestartsAfterReset()
	{
		var starts = 0;

		IEnumerable<int> TestSequence()
		{
			starts++;
			yield return 1;
			yield return 2;
		}

		using var seq = TestSequence().AsTestingSequence(maxEnumerations: 2);
		using var buffer = seq.Share();

		buffer.Take(1).Consume();
		Assert.Equal(1, starts);

		buffer.Reset();
		buffer.Take(1).Consume();
		Assert.Equal(2, starts);
	}

	[Fact]
	public void ShareThrowsWhenCacheDisposedDuringIteration()
	{
		using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		using var buffer = seq.Share();

		using var reader = buffer.Read();

		Assert.Equal(0, reader.Read());
		buffer.Dispose();

		_ = Assert.Throws<ObjectDisposedException>(
			() => reader.Read());
	}

	[Fact]
	public void ShareThrowsWhenResetDuringIteration()
	{
		using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		using var buffer = seq.Share();

		using var reader = buffer.Read();

		Assert.Equal(0, reader.Read());
		buffer.Reset();

		var ex = Assert.Throws<InvalidOperationException>(
			() => reader.Read());
		Assert.Equal("Buffer reset during iteration.", ex.Message);
	}

	[Fact]
	public void ShareThrowsWhenGettingIteratorAfterDispose()
	{
		using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		using var buffer = seq.Share();
		buffer.Consume();
		buffer.Dispose();

		_ = Assert.Throws<ObjectDisposedException>(
			buffer.Consume);
	}

	[Fact]
	public void ShareThrowsWhenResettingAfterDispose()
	{
		using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		using var buffer = seq.Share();
		buffer.Consume();
		buffer.Dispose();

		_ = Assert.Throws<ObjectDisposedException>(
			buffer.Reset);
	}

	[Fact]
	public void ShareRethrowsErrorDuringIterationToAllIteratorsUntilReset()
	{
		using var xs = SeqExceptionAt(2).AsTestingSequence(maxEnumerations: 2);

		using var buffer = xs.Share();

		using (var r1 = buffer.Read())
		using (var r2 = buffer.Read())
		{
			Assert.Equal(1, r1.Read());
			_ = Assert.Throws<TestException>(() => r2.Read());

			Assert.True(xs.IsDisposed);
			_ = Assert.Throws<TestException>(() => r1.Read());
		}

		_ = Assert.Throws<TestException>(() => buffer.Read());

		buffer.Reset();

		using (var r1 = buffer.Read())
			Assert.Equal(1, r1.Read());
	}

	[Fact]
	public void ShareRethrowsErrorDuringFirstIterationStartToAllIterationsUntilReset()
	{
		using var seq = new FailingEnumerable().AsTestingSequence(maxEnumerations: 2);

		using var buffer = seq.Share();

		for (var i = 0; i < 2; i++)
			_ = Assert.Throws<TestException>(() => buffer.First());

		buffer.Reset();
		buffer.AssertSequenceEqual(1);
	}

	private sealed class FailingEnumerable : IEnumerable<int>
	{
		private bool _started;
		public IEnumerator<int> GetEnumerator()
		{
			if (!_started)
			{
				_started = true;
				throw new TestException();
			}

			return Enumerable.Range(1, 1).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
