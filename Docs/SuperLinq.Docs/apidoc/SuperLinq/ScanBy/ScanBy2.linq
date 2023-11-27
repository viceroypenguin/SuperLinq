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

// execute a scan of the sequence
var result = sequence
	.ScanBy(
		x => x[..1],
		k => 0,
		(a, k, b) => a + 1,
		StringComparer.OrdinalIgnoreCase);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(B, 1), (f, 1), (B, 2), (Q, 1), (B, 3), (F, 2), (b, 4), (b, 5), (f, 3), (B, 6)]
