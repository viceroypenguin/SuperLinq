using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class EquiZipTest
{
	[Test]
	public void BothSequencesDisposedWithUnequalLengthsAndLongerFirst()
	{
		using var longer = TestingSequence.Of(1, 2, 3);
		using var shorter = TestingSequence.Of(1, 2);

		// Yes, this will throw... but then we should still have disposed both sequences
		Assert.Throws<InvalidOperationException>(() =>
			longer.EquiZip(shorter, (x, y) => x + y).Consume());
	}

	[Test]
	public void BothSequencesDisposedWithUnequalLengthsAndShorterFirst()
	{
		using var longer = TestingSequence.Of(1, 2, 3);
		using var shorter = TestingSequence.Of(1, 2);

		// Yes, this will throw... but then we should still have disposed both sequences
		Assert.Throws<InvalidOperationException>(() =>
			shorter.EquiZip(longer, (x, y) => x + y).Consume());
	}

	[Test]
	public void ZipWithEqualLengthSequencesFailStrategy()
	{
		var zipped = new[] { 1, 2, 3 }.EquiZip(new[] { 4, 5, 6 }, ValueTuple.Create);
		Assert.That(zipped, Is.Not.Null);
		zipped.AssertSequenceEqual((1, 4), (2, 5), (3, 6));
	}

	[Test]
	public void ZipWithFirstSequenceShorterThanSecondFailStrategy()
	{
		var zipped = new[] { 1, 2 }.EquiZip(new[] { 4, 5, 6 }, ValueTuple.Create);
		Assert.That(zipped, Is.Not.Null);
		Assert.Throws<InvalidOperationException>(() =>
			zipped.Consume());
	}

	[Test]
	public void ZipWithFirstSequnceLongerThanSecondFailStrategy()
	{
		var zipped = new[] { 1, 2, 3 }.EquiZip(new[] { 4, 5 }, ValueTuple.Create);
		Assert.That(zipped, Is.Not.Null);
		Assert.Throws<InvalidOperationException>(() =>
			zipped.Consume());
	}

	[Test]
	public void ZipIsLazy()
	{
		var bs = new BreakingSequence<int>();
		bs.EquiZip(bs, BreakingFunc.Of<int, int, int>());
	}

	[Test]
	public void MoveNextIsNotCalledUnnecessarily()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2, 3);
		using var s3 = SuperEnumerable.From(() => 1,
										   () => 2,
										   () => throw new TestException())
									 .AsTestingSequence();

		Assert.Throws<InvalidOperationException>(() =>
			s1.EquiZip(s2, s3, (x, y, z) => x + y + z).Consume());
	}

	[Test]
	public void ZipDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);

		Assert.Throws<InvalidOperationException>(() =>
			s1.EquiZip(new BreakingSequence<int>(), ValueTuple.Create).Consume());
	}
}
