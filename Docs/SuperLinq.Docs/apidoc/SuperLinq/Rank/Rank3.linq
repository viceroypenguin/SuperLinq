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
	.Rank(
		Comparer<(int key, string text)>.Create((x, y) => x.key.CompareTo(y.key)));

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [((1, 9), 1), ((1, 10), 1), ((2, 7), 3), ((2, 8), 3), ((3, 5), 5), ((3, 6), 5), ((4, 3), 7), ((4, 4), 7), ((5, 1), 9), ((5, 2), 9)]
