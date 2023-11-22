<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);

// Filter a sequence based on a matching sequence of bools
var result = sequence
	.Where([true, false, true, true, false]);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 3, 4]
