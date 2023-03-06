using System.Collections;
using CommunityToolkit.Diagnostics;

namespace Test;

public class PublishTest
{
	[Fact]
	public void PublishIsLazy()
	{
		_ = new BreakingSequence<int>().Publish();
	}

	[Fact]
	public void PublishWithSingleConsumer()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		using var result = seq.Publish();
		result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Fact]
	public void PublishWithMultipleConsumers()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		using var result = seq.Publish();

		using var r1 = result.Read();
		Assert.Equal(1, r1.Read());
		Assert.Equal(2, r1.Read());
		Assert.Equal(0, result.Count);

		using var r2 = result.Read();
		Assert.Equal(3, r1.Read());
		Assert.Equal(3, r2.Read());
		Assert.Equal(0, result.Count);

		Assert.Equal(4, r1.Read());
		Assert.Equal(5, r1.Read());
		Assert.Equal(6, r1.Read());
		Assert.Equal(7, r1.Read());
		Assert.Equal(4, result.Count);

		Assert.Equal(4, r2.Read());
		Assert.Equal(5, r2.Read());
		Assert.Equal(2, result.Count);

		Assert.Equal(8, r1.Read());
		Assert.Equal(9, r1.Read());
		Assert.Equal(4, result.Count);

		using var r3 = result.Read();
		Assert.Equal(10, r3.Read());
		r3.ReadEnd();
		Assert.Equal(5, result.Count);

		Assert.Equal(6, r2.Read());
		Assert.Equal(7, r2.Read());
		Assert.Equal(3, result.Count);

		Assert.Equal(10, r1.Read());
		r1.ReadEnd();

		Assert.Equal(8, r2.Read());
		Assert.Equal(9, r2.Read());
		Assert.Equal(10, r2.Read());
		r2.ReadEnd();
		Assert.Equal(0, result.Count);
	}

	[Fact]
	public void PublishWithInnerConsumer()
	{
		using var seq = Enumerable.Range(1, 6).AsTestingSequence();

		using var result = seq.Publish();

		using var r1 = result.Read();
		Assert.Equal(1, r1.Read());
		Assert.Equal(2, r1.Read());

		using (var r2 = result.Read())
		{
			Assert.Equal(3, r2.Read());
			Assert.Equal(4, r2.Read());
			Assert.Equal(2, result.Count);
		}

		Assert.Equal(3, r1.Read());
		Assert.Equal(4, r1.Read());
		Assert.Equal(5, r1.Read());
		Assert.Equal(6, r1.Read());

		r1.ReadEnd();
		Assert.Equal(0, result.Count);
	}

	[Fact]
	public void PublishWithSequentialPartialConsumers()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		using var result = seq.Publish();

		using (var r1 = result.Read())
		{
			Assert.Equal(1, r1.Read());
			Assert.Equal(2, r1.Read());
			Assert.Equal(3, r1.Read());
			Assert.Equal(4, r1.Read());
			Assert.Equal(5, r1.Read());
			Assert.Equal(0, result.Count);
		}

		using (var r2 = result.Read())
		{
			Assert.Equal(6, r2.Read());
			Assert.Equal(7, r2.Read());
			Assert.Equal(8, r2.Read());
			Assert.Equal(9, r2.Read());
			Assert.Equal(10, r2.Read());
			r2.ReadEnd();
			Assert.Equal(0, result.Count);
		}

		using var r3 = result.Read();
		r3.ReadEnd();
		Assert.Equal(0, result.Count);
	}

	[Fact]
	public void PublishDisposesAfterSourceIsIteratedEntirely()
	{
		using var seq = Enumerable.Range(0, 10).AsTestingSequence();

		using var buffer = seq.Publish();
		buffer.Consume();

		Assert.True(seq.IsDisposed);
	}

	[Fact]
	public void PublishDisposesWithPartialEnumeration()
	{
		using var seq = Enumerable.Range(0, 10).AsTestingSequence();

		using var buffer = seq.Publish();

		using (buffer)
			buffer.Take(5).Consume();

		Assert.True(seq.IsDisposed);
	}

	[Fact]
	public void PublishRestartsAfterReset()
	{
		var starts = 0;

		IEnumerable<int> TestSequence()
		{
			starts++;
			yield return 1;
			yield return 2;
		}

		using var seq = TestSequence().AsTestingSequence(maxEnumerations: 2);
		using var buffer = seq.Publish();

		buffer.Take(1).Consume();
		Assert.Equal(1, starts);

		buffer.Reset();
		buffer.Take(1).Consume();
		Assert.Equal(2, starts);
	}

	[Fact]
	public void PublishThrowsWhenCacheDisposedDuringIteration()
	{
		using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		using var buffer = seq.Publish();

		using var reader = buffer.Read();

		Assert.Equal(0, reader.Read());
		buffer.Dispose();

		_ = Assert.Throws<ObjectDisposedException>(
			() => reader.Read());
	}

	[Fact]
	public void PublishThrowsWhenResetDuringIteration()
	{
		using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		using var buffer = seq.Publish();

		using var reader = buffer.Read();

		Assert.Equal(0, reader.Read());
		buffer.Reset();

		var ex = Assert.Throws<InvalidOperationException>(
			() => reader.Read());
		Assert.Equal("Buffer reset during iteration.", ex.Message);
	}

	[Fact]
	public void PublishThrowsWhenGettingIteratorAfterDispose()
	{
		using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		using var buffer = seq.Publish();
		buffer.Consume();
		buffer.Dispose();

		_ = Assert.Throws<ObjectDisposedException>(
			buffer.Consume);
	}

	[Fact]
	public void PublishThrowsWhenResettingAfterDispose()
	{
		using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		using var buffer = seq.Publish();
		buffer.Consume();
		buffer.Dispose();

		_ = Assert.Throws<ObjectDisposedException>(
			buffer.Reset);
	}

	[Fact]
	public void PublishRethrowsErrorDuringIterationToAllIteratorsUntilReset()
	{
		using var xs = SeqExceptionAt(2).AsTestingSequence(maxEnumerations: 2);

		using var buffer = xs.Publish();

		using (var r1 = buffer.Read())
		using (var r2 = buffer.Read())
		{
			Assert.Equal(1, r1.Read());
			Assert.Equal(1, r2.Read());

			_ = Assert.Throws<TestException>(() => r1.Read());
			_ = Assert.Throws<TestException>(() => r2.Read());
			Guard.IsTrue(xs.IsDisposed);
		}

		using (var r3 = buffer.Read())
			_ = Assert.Throws<TestException>(() => r3.Read());

		buffer.Reset();

		using (var r4 = buffer.Read())
			Assert.Equal(1, r4.Read());
	}

	[Fact]
	public void PublishRethrowsErrorDuringFirstIterationStartToAllIterationsUntilReset()
	{
		using var seq = new FailingEnumerable().AsTestingSequence(maxEnumerations: 2);

		using var buffer = seq.Publish();

		for (var i = 0; i < 2; i++)
			_ = Assert.Throws<TestException>(() => buffer.First());

		buffer.Reset();
		buffer.AssertSequenceEqual(1);
	}

	private class FailingEnumerable : IEnumerable<int>
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

	[Fact]
	public void PublishLambdaIsLazy()
	{
		_ = new BreakingSequence<int>().Publish(BreakingFunc.Of<IEnumerable<int>, IEnumerable<string>>());
	}

	[Fact]
	public void PublishLambdaSimple()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Publish(xs => xs.Zip(xs, (l, r) => l + r).Take(4));
		result.AssertSequenceEqual(2, 4, 6, 8);
	}
}
