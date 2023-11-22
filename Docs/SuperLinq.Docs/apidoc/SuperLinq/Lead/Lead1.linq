<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[] { "foo", "bar", "baz" };

// get leading elements
var result = sequence.Lead(1);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(foo, bar), (bar, baz), (baz, )]
