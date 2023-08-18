<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 10);
var indices = new int[] { 0, 1, 8, 9, 3, 4, 2, };

// Select elements from sequence using the values in indices
var result = sequence
	.BindByIndex(indices);

Console.WriteLine(
	"[" + 
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 2, 9, 10, 4, 5, 3]
