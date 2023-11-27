<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var start = (x: 0, y: 0);
var end = (x: -2, y: -2);
((int x, int y) p, double cost, double bestGuess) GetNeighbor((int x, int y) p, double newCost)
{
	var xD = p.x - end.x;
	var yD = p.y - end.y;
	var dist = Math.Sqrt((xD * xD) + (yD * yD));
	return (p, newCost, newCost + dist);
}

IEnumerable<((int x, int y) p, double cost, double bestGuess)> GetNeighbors((int x, int y) p, double cost)
{
	yield return GetNeighbor((p.x + 1, p.y), cost + 1.001d);
	yield return GetNeighbor((p.x, p.y + 1), cost + 1.002d);
	yield return GetNeighbor((p.x - 1, p.y), cost + 1.003d);
	yield return GetNeighbor((p.x, p.y - 1), cost + 1.004d);
}

// Find the shortest path from start to end
var result = SuperEnumerable
	.GetShortestPath<(int x, int y), double>(
		start,
		GetNeighbors,
		end,
		new PointComparer(),
		null);

Console.WriteLine(string.Join(" -> ", result.Select(x => $"({x.nextState}, {x.cost:N3})")));

// This code produces the following output:
// ((0, 0), 0.000) -> ((-1, 0), 1.003) -> ((-1, -1), 2.007) -> ((-2, -1), 3.010) -> ((-2, -2), 4.014)

class PointComparer : IEqualityComparer<(int x, int y)>
{
	public bool Equals((int x, int y) x, (int x, int y) y) =>
		ManhattanDistance(x) == ManhattanDistance(y);

	public int GetHashCode((int x, int y) obj) =>
		ManhattanDistance(obj).GetHashCode();

	private static double ManhattanDistance((int x, int y) obj) =>
		Math.Sqrt((obj.x * obj.x) + (obj.y * obj.y));
}