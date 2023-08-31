<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 10);

// Fill in missing elements from later elements
var result = sequence
	.FillBackward(
		i => i % 3 < 2,
		(cur, nxt) => cur * nxt * 100);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [200, 2, 1500, 2000, 5, 4800, 5600, 8, 9, 10]
