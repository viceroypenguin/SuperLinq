<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 4);

// execute a scan of the sequence, returning the aggregation before processing the element
var result = sequence.PreScan((a, b) => a + b, 0);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [0, 1, 3, 6]
