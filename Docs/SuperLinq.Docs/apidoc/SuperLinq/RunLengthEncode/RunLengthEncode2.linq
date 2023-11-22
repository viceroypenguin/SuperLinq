<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	"foo",
	"FOO",
	"fOo",
	"bAr",
	"BAR",
	"BAZ",
	"baz",
	"Baz",
	"BaZ",
	"Qux",
};

// Get the run-length encoding of a sequence
var result = sequence
	.RunLengthEncode(
		StringComparer.OrdinalIgnoreCase);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(foo, 3), (bAr, 2), (BAZ, 4), (Qux, 1)]
