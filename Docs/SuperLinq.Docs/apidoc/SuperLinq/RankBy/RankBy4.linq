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

// Rank the items in the sequence
var result = sequence
	.RankBy(
		x => x.key,
		Comparer<int>.Create((x, y) => (x % 2).CompareTo(y % 2)),
		OrderByDirection.Ascending);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [((4, 3), 1), ((4, 4), 1), ((2, 7), 1), ((2, 8), 1), ((5, 1), 5), ((5, 2), 5), ((3, 5), 5), ((3, 6), 5), ((1, 9), 5), ((1, 10), 5)]
