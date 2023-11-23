<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

// Generate a sequence using a sequence function
var result = SuperEnumerable
	.Range(1, 10, 2);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 3, 5, 7, 9, 11, 13, 15, 17, 19]
