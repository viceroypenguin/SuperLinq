<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	"BAR",
	"foo",
	"Baz",
	"Qux",
	"BAZ",
	"FOO",
	"bAr",
	"baz",
	"fOo",
	"BaZ",
};

// check that sequence starts with a known sequence of values
var result = sequence
	.StartsWith(new[] { "BAR", "foo", "Baz", });

Console.WriteLine($"StartsWith ['BAR', 'foo', 'Baz']: {result}");

result = sequence
	.StartsWith(new[] { "bar", "foo", "Baz", });

Console.WriteLine($"StartsWith ['bar', 'foo', 'Baz']: {result}");

result = sequence
	.StartsWith(new[] { "foo", "Baz", });

Console.WriteLine($"StartsWith ['foo', 'Baz']: {result}");

// This code produces the following output:
// StartsWith ['BAR', 'foo', 'Baz']: True
// StartsWith ['bar', 'foo', 'Baz']: False
// StartsWith ['foo', 'Baz']: False
