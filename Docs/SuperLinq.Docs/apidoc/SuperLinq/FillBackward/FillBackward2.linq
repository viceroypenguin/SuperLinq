<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 10);

// Fill in missing elements from later elements
var result = sequence
	.FillBackward(
		i => i % 3 < 2);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [2, 2, 5, 5, 5, 8, 8, 8, 9, 10]
