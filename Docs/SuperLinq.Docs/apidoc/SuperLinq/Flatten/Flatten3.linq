<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new object[]
{
	true,
	false,
	1,
	"bar",
	new object[]
	{
		2,
		new[]
		{
			3,
		},
	},
	'c',
	4,
};

// Flatten a hierarchical sequence
var result = sequence
	.Flatten(obj =>
		obj switch
		{
			int => null,
			IEnumerable inner => inner,
			_ => Enumerable.Empty<object>(),
		});

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 2, 3, 4]
