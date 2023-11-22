<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[] { "foo", "bar", "alp", "car", };

// Transform a sequence by index
var result = sequence
	.ToArrayByIndex(c => c[0] - 'a');

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// One possible random output is as follows:
// [alp, bar, car, , , foo]
