<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 10);

// Fill in missing elements from previous elements
var result = sequence
	.FillForward(
		i => i % 3 < 2,
		(cur, nxt) => cur * nxt * 100);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 2, 600, 800, 5, 3000, 3500, 8, 7200, 8000]
