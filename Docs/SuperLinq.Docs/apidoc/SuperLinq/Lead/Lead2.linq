<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[] { "foo", "bar", "baz" };

// get leading elements
var result = sequence.Lead(1, (cur, lead) => new { cur, lead, });

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [{ cur = foo, lead = bar }, { cur = bar, lead = baz }, { cur = baz, lead =  }]
