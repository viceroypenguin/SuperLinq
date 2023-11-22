<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 10);

// Shuffle a sequence
var result = sequence.Shuffle();

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// One possible random output is as follows:
// [10, 9, 3, 8, 1, 6, 2, 4, 7, 5]
