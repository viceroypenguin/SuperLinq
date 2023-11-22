<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[] { "a", "B", "c", "A", "b", "A", };

// Count elements in a sequence grouped by key
var result = sequence.CountBy(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(a, 3), (B, 2), (c, 1)]
