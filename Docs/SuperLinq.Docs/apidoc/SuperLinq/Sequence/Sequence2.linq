<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

// Generate a sequence using a sequence function
var result = SuperEnumerable
	.Sequence(5, 10, 2);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");
	
result = SuperEnumerable
	.Sequence(10, 5, -2);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [5, 7, 9]
// [10, 8, 6]
