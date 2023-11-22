<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	new { Key = "jan", Value = 123, },
	new { Key = "feb", Value = 987, },
	new { Key = "mar", Value = 789, },
	new { Key = "Jan", Value = 123, },
	new { Key = "JAN", Value = 781, },
};

// Group adjacent items
var result = sequence
	.IndexBy(
		x => x.Key,
		StringComparer.OrdinalIgnoreCase);

Console.WriteLine(
	"[ " + 
	string.Join(
		", ", 
		result) +
	" ]");

// This code produces the following output:
// [ (0, { Key = jan, Value = 123 }), (0, { Key = feb, Value = 987 }), (0, { Key = mar, Value = 789 }), (1, { Key = Jan, Value = 123 }), (2, { Key = JAN, Value = 781 }) ]
