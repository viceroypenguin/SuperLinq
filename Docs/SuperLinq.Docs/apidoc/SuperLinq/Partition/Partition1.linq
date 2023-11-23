<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(0, 10);

// Partition a sequence
var (evens, odds) = sequence
	.Partition(x => x % 2 == 0);

Console.WriteLine(
	"evens: [" +
	string.Join(", ", evens) +
	"]");

Console.WriteLine(
	"odds: [" +
	string.Join(", ", odds) +
	"]");

// This code produces the following output:
// evens: [0, 2, 4, 6, 8]
// odds: [1, 3, 5, 7, 9]
