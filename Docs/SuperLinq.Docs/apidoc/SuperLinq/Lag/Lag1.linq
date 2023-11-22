<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[] { "foo", "bar", "baz" };

// get lagged elements
var result = sequence.Lag(1);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(foo, ), (bar, foo), (baz, bar)]
