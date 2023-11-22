<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 4);

// Replace a value in a sequence
var result = sequence
	.TagFirstLast();

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(1, True, False), (2, False, False), (3, False, False), (4, False, True)]
