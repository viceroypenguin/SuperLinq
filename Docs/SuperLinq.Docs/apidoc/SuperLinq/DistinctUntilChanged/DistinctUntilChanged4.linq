<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	(key: "aa", text: "1"),
	(key: "Aa", text: "2"),
	(key: "AA", text: "3"),
	(key: "BB", text: "4"),
	(key: "bB", text: "5"),
	(key: "Cc", text: "6"),
	(key: "CC", text: "7"),
	(key: "Aa", text: "8"),
	(key: "aA", text: "9"),
	(key: "bb", text: "10"),
	(key: "bB", text: "11"),
	(key: "CC", text: "12"),
};

// Get distinct 
var result = sequence
	.DistinctUntilChanged(
		x => x.key,
		StringComparer.OrdinalIgnoreCase);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(aa, 1), (BB, 4), (Cc, 6), (Aa, 8), (bb, 10), (CC, 12)]
