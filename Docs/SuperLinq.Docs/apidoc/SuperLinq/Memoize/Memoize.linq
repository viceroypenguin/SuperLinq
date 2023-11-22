<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var count = 0;
var sequence = Enumerable.Range(1, 10)
	.Do(_ => count++);

// get leading elements
var result = sequence.Memoize();

Console.WriteLine($"iterations: {count}");
Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");
Console.WriteLine($"iterations: {count}");
Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");
Console.WriteLine($"iterations: {count}");

// This code produces the following output:
// iterations: 0
// [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
// iterations: 10
// [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
// iterations: 10
