namespace Test;

public class GetShortestPathTest
{
	public static IEnumerable<object?[]> GetStringIntCostData { get; } =
		new[]
		{
			new object?[] { null, null, 15, },
			new object?[] { StringComparer.InvariantCultureIgnoreCase, null, 10, },
			new object?[] { null, Comparer<int>.Create((x, y) => -x.CompareTo(y)), 150, },
			new object?[] { StringComparer.InvariantCultureIgnoreCase, Comparer<int>.Create((x, y) => -x.CompareTo(y)), 1000, },
		};

	private static ILookup<string, (string to, int cost)> BuildStringIntMap()
	{
		var costs =
			new[]
			{
				(from: "start", to: "a", cost: 1),
				(from: "a", to: "b", cost: 2),
				(from: "b", to: "c", cost: 3),
				(from: "c", to: "d", cost: 4),
				(from: "d", to: "end", cost: 5),
				(from: "start", to: "A", cost: 10),
				(from: "A", to: "B", cost: 20),
				(from: "B", to: "C", cost: 30),
				(from: "C", to: "D", cost: 40),
				(from: "D", to: "end", cost: 50),
				(from: "start", to: "END", cost: 10),
				(from: "start", to: "END", cost: 1000),
			};
		var map = costs
			.Concat(costs.Select(x => (from: x.to, to: x.from, x.cost)))
			.Where(x =>
				x.to != "start"
				&& x.from != "end")
			.ToLookup(x => x.from, x => (x.to, x.cost));
		return map;
	}

	[Theory]
	[MemberData(nameof(GetStringIntCostData))]
	public void GetStringIntCost(
		IEqualityComparer<string>? stateComparer,
		IComparer<int>? costComparer,
		int expectedCost)
	{
		var map = BuildStringIntMap();
		var actualCost = SuperEnumerable.GetShortestPathCost(
			"start",
			(x, c) => map[x].Select(y => (y.to, c + y.cost)),
			"end",
			stateComparer,
			costComparer);

		Assert.Equal(expectedCost, actualCost);
	}
}
