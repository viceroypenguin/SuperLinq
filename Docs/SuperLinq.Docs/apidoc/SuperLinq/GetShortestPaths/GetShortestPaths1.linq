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
	.ToLookup(x => x.from, x => (x.to, x.cost));

// Find the shortest path from start to end
var result = SuperEnumerable
	.GetShortestPaths<string, int>(
		"start",
		(state, cost) => map[state]
			.Select(x => (x.to, x.cost + cost)));

foreach (var (key, (from, cost)) in result)
{
	Console.WriteLine($"[{key}] = (from: {from}, totalCost: {cost})");
}

// This code produces the following output:
// [start] = (from: , totalCost: 0)
// [a] = (from: start, totalCost: 1)
// [b] = (from: a, totalCost: 3)
// [c] = (from: b, totalCost: 6)
// [END] = (from: start, totalCost: 10)
// [d] = (from: c, totalCost: 10)
// [A] = (from: start, totalCost: 10)
// [end] = (from: d, totalCost: 15)
// [B] = (from: A, totalCost: 30)
// [C] = (from: B, totalCost: 60)
// [D] = (from: C, totalCost: 100)
