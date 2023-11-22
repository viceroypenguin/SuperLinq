<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[] { "foo", "bar", "baz" };

// get leading elements
var result = sequence.Lead(1, "LEAD", (cur, lag) => new { cur, lag, });

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [{ cur = foo, lag = bar }, { cur = bar, lag = baz }, { cur = baz, lag = LEAD }]
