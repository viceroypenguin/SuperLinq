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

// Get the top N items
var result = sequence
	.PartialSort(
		3, 
		Comparer<(int key, string text)>.Create((x, y) => x.key.CompareTo(y.key)),
		OrderByDirection.Descending);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(5, 1), (5, 2), (4, 3)]
