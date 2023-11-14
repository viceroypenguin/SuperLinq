<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(0, 3).Repeat(10);

// split a sequence using a key value
var result = sequence
	.Split(2, EqualityComparer<int>.Default, 4, g => g.Sum());

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 1, 1, 1, 18]
