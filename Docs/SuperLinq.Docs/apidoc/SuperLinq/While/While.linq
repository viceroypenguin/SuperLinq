<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 2);

// Repeat a sequence while a condition is true
var count = 0;
var result = SuperEnumerable
	.While(
		() => count++ < 3,
		sequence);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 2, 1, 2, 1, 2]
