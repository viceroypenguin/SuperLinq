<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	new { Key = 1, Value = 123, },
	new { Key = 2, Value = 987, },
	new { Key = 3, Value = 789, },
	new { Key = 1, Value = 123, },
	new { Key = 1, Value = 781, },
};

// Group adjacent items
var result = sequence
	.IndexBy(
		x => x.Key);

Console.WriteLine(
	"[ " + 
	string.Join(
		", ", 
		result) +
	" ]");

// This code produces the following output:
// [ (0, { Key = 1, Value = 123 }), (0, { Key = 2, Value = 987 }), (0, { Key = 3, Value = 789 }), (1, { Key = 1, Value = 123 }), (2, { Key = 1, Value = 781 }) ]
