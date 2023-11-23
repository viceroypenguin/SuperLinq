<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	(key: 3, text: "1"),
	(key: 3, text: "2"),
	(key: 2, text: "3"),
	(key: 2, text: "4"),
	(key: 1, text: "5"),
	(key: 1, text: "6"),
	(key: 3, text: "7"),
	(key: 3, text: "8"),
	(key: 2, text: "9"),
	(key: 2, text: "10"),
	(key: 1, text: "11"),
	(key: 1, text: "12"),
};

// Get distinct 
var result = sequence
	.DistinctUntilChanged(x => x.key);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(3, 1), (2, 3), (1, 5), (3, 7), (2, 9), (1, 11)]
