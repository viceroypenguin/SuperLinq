<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

// Generate a sequence using a sequence function
var result = SuperEnumerable
	.Sequence(5, 10);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");
	
result = SuperEnumerable
	.Sequence(10, 5);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [5, 6, 7, 8, 9, 10]
// [10, 9, 8, 7, 6, 5]
