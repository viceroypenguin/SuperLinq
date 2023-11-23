<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	(key: "aaa", value: 1),
	(key: "bb", value: 2),
	(key: "c", value: 3),
	(key: "dddd", value: 4),
};

// Determine if sequence ends with the ends sequence
var result = sequence
	.ExceptBy(
		new[] { (key: "A", value: 13), (key: "D", value: 14), },
		s => s.key[..1],
		StringComparer.OrdinalIgnoreCase);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(bb, 2), (c, 3)]
