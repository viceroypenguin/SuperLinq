<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 10);

// Fill in missing elements from previous elements
var result = sequence
	.FillForward(
		i => i % 3 < 2);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 2, 2, 2, 5, 5, 5, 8, 8, 8]
