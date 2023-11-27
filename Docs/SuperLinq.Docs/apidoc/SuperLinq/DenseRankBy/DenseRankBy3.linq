<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	(key: 5, text: "1"),
	(key: 5, text: "2"),
	(key: 4, text: "3"),
	(key: 4, text: "4"),
	(key: 3, text: "5"),
	(key: 3, text: "6"),
	(key: 2, text: "7"),
	(key: 2, text: "8"),
	(key: 1, text: "9"),
	(key: 1, text: "10"),
};

// Get the top N sets of items
var result = sequence
	.DenseRankBy(
		x => x.key,
		Comparer<int>.Create((x, y) => (x % 2).CompareTo(y % 2)));

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [((4, 3), 1), ((4, 4), 1), ((2, 7), 1), ((2, 8), 1), ((5, 1), 2), ((5, 2), 2), ((3, 5), 2), ((3, 6), 2), ((1, 9), 2), ((1, 10), 2)]
