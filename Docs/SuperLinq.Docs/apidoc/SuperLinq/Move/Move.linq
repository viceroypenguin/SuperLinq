<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(0, 6);

// move a subsequence within the larger sequence
var result = sequence
	.Move(3, 2, 0);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [3, 4, 0, 1, 2, 5]
