namespace Test;

#pragma warning disable CS0618 // Type or member is obsolete

public class OrderedMergeTest
{
	public static IEnumerable<object[]> TestData { get; } =
		from e in new[]
		{
			new
			{
				A = Seq(0, 2, 4),
				B = Seq(0, 1, 2, 3, 4),
				R = Seq(0, 1, 2, 3, 4),
			},
			new
			{
				A = Seq(0, 1, 2, 3, 4),
				B = Seq(0, 2, 4),
				R = Seq(0, 1, 2, 3, 4),
			},
			new
			{
				A = Seq(0, 2, 4),
				B = Seq(1, 3, 5),
				R = Seq(0, 1, 2, 3, 4, 5),
			},
			new
			{
				A = Seq<int>(),
				B = Seq(0, 1, 2),
				R = Seq(0, 1, 2),
			},
			new
			{
				A = Seq(0, 1, 2),
				B = Seq<int>(),
				R = Seq(0, 1, 2),
			},
		}
		select new object[] { e.A, e.B, e.R, };

	[Theory, MemberData(nameof(TestData))]
	public void OrderedMerge(
		IEnumerable<int> first,
		IEnumerable<int> second,
		IEnumerable<int> expected)
	{
		Assert.Equal(expected, first.AsTestingSequence().OrderedMerge(second.AsTestingSequence()));
	}
}

#pragma warning restore CS0618 // Type or member is obsolete
