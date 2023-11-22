<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[] { "foo", "bar", "alp", "car", };

// Transform a sequence by index
var result = sequence
	.ToArrayByIndex(26, c => c[0] - 'a', (c, i) => $"{i}:{c}");

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// One possible random output is as follows:
// [0:alp, 1:bar, 2:car, , , 5:foo, , , , , , , , , , , , , , , , , , , , ]
