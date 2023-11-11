<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 10);

// execute a scan of the sequence
var result = sequence
	.ScanBy(
		x => x % 2,
		k => k * 1000,
		(a, k, b) => a + b);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(1, 1001), (0, 2), (1, 1004), (0, 6), (1, 1009), (0, 12), (1, 1016), (0, 20), (1, 1025), (0, 30)]
