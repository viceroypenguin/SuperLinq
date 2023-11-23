<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

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
	.ToLookup(x => x.from, x => (x.to, x.cost), StringComparer.OrdinalIgnoreCase);

// Find the shortest path from start to end
var result = SuperEnumerable
	.GetShortestPathCost<string, int>(
		"start",
		(state, cost) => map[state]
			.Select(x => (x.to, x.cost + cost)),
		"end",
		StringComparer.OrdinalIgnoreCase,
		default);

Console.WriteLine($"cost: {result}");

// This code produces the following output:
// cost: 10
