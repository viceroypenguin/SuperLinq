<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 19);
	        
// Aggregate elements in a sequence grouped by key
var result = sequence
	.AggregateBy(
		x => x % 3,
		0,
		(acc, e) => acc + e);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [[1, 70], [2, 57], [0, 63]]
