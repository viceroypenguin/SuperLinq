<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[] { "foo", "bar", "baz" };

// get lagged elements
var result = sequence.Lag(1, (cur, lag) => new { cur, lag, });

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [{ cur = foo, lag =  }, { cur = bar, lag = foo }, { cur = baz, lag = bar }]
