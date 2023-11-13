<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 10);

// Skip elements until the condition is true
var result = sequence
	.SkipUntil(x => x == 5);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// One possible random output is as follows:
// [6, 7, 8, 9, 10]
