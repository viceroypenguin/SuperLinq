<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var costs =
	new[]
	{
		(from: (id: "start", index: 1), to: (id: "a", index: 2), cost: 1),
		(from: (id: "a", index: 2), to: (id: "b", index: 3), cost: 2),
		(from: (id: "b", index: 3), to: (id: "c", index: 3), cost: 3),
		(from: (id: "c", index: 3), to: (id: "d", index: 4), cost: 4),
		(from: (id: "d", index: 4), to: (id: "end", index: 5), cost: 5),
		(from: (id: "start", index: 1), to: (id: "A", index: 6), cost: 10),
		(from: (id: "A", index: 6), to: (id: "B", index: 7), cost: 20),
		(from: (id: "B", index: 7), to: (id: "C", index: 8), cost: 30),
		(from: (id: "C", index: 8), to: (id: "D", index: 9), cost: 40),
		(from: (id: "D", index: 9), to: (id: "end", index: 5), cost: 50),
		(from: (id: "start", index: 1), to: (id: "END", index: 10), cost: 10),
		(from: (id: "start", index: 1), to: (id: "END", index: 10), cost: 1000),
	};
var map = costs
	.Concat(costs.Select(x => (from: x.to, to: x.from, x.cost)))
	.Where(x =>
		x.to.id != "start"
		&& x.from.id != "end")
	.ToLookup(x => x.from.id, x => (x.to, x.cost));

// Find the shortest path from start to end
var result = SuperEnumerable
	.GetShortestPathCost<(string id, int index), int>(
		("start", 1),
		(state, cost) => map[state.id]
			.Select(x => (x.to, x.cost + cost)),
		x => x.id == "end");

Console.WriteLine($"cost: {result}");

// This code produces the following output:
// cost: 15
