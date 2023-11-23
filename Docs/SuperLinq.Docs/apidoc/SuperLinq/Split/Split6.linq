<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(0, 11);

// split a sequence using a key value
var result = sequence
	.Split(5, EqualityComparer<int>.Default, g => g.Sum());

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [10, 40]
