<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

// Repeat a value indefinitely
var result = SuperEnumerable
	.Repeat(1)
	.Take(10);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 1, 1, 1, 1, 1, 1, 1, 1, 1]
