<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 10);

// get a random subset of the above sequence
var result = sequence.RandomSubset(4);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// One possible output of the above sequence:
// (each run will have different results)
// [3, 6, 7, 5]
