namespace Test;

// Keep testing `Unfold` for now
#pragma warning disable CS0618

public class UnfoldTest
{
	[Fact]
	public void UnfoldInfiniteSequence()
	{
		var result = SuperEnumerable
			.Unfold(
				1,
				x => (Result: x, State: x + 1),
				_ => true,
				e => e.State,
				e => e.Result)
			.Take(100);


		result.AssertSequenceEqual(
			SuperEnumerable.Generate(1, x => x + 1).Take(100));
	}

	[Fact]
	public void UnfoldFiniteSequence()
	{
		var result = SuperEnumerable
			.Unfold(
				1,
				x => (Result: x, State: x + 1),
				e => e.Result <= 100,
				e => e.State,
				e => e.Result);

		result.AssertSequenceEqual(
			SuperEnumerable.Generate(1, x => x + 1).Take(100));
	}

	[Fact]
	public void UnfoldIsLazy()
	{
		_ = SuperEnumerable
			.Unfold(
				0,
				BreakingFunc.Of<int, (int, int)>(),
				BreakingFunc.Of<(int, int), bool>(),
				BreakingFunc.Of<(int, int), int>(),
				BreakingFunc.Of<(int, int), int>());
	}


	[Fact]
	public void UnfoldSingleElementSequence()
	{
		var result = SuperEnumerable
			.Unfold(
				0,
				x => (Result: x, State: x + 1),
				x => x.Result == 0,
				e => e.State,
				e => e.Result);

		result.AssertSequenceEqual(0);
	}

	[Fact]
	public void UnfoldEmptySequence()
	{
		var result = SuperEnumerable
			.Unfold(
				0,
				x => (Result: x, State: x + 1),
				x => x.Result < 0,
				e => e.State,
				e => e.Result);

		result.AssertSequenceEqual();
	}

	[Fact]
	public void UnfoldIsIdempotent()
	{
		var result = SuperEnumerable
			.Unfold(
				1,
				n => (Result: n, Next: n + 1),
				_ => true,
				n => n.Next,
				n => n.Result)
			.Take(5);

		result.AssertSequenceEqual(
			Enumerable.Range(1, 5));

		result.AssertSequenceEqual(
			Enumerable.Range(1, 5));
	}
}
