<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	(key: "jan", value: 123),
	(key: "Jan", value: 456),
	(key: "JAN", value: 789),
	(key: "feb", value: 987),
	(key: "Feb", value: 654),
	(key: "FEB", value: 321),
	(key: "mar", value: 789),
	(key: "Mar", value: 456),
	(key: "MAR", value: 123),
	(key: "jan", value: 123),
	(key: "Jan", value: 456),
	(key: "JAN", value: 781),
};

// Group adjacent items
var result = sequence
	.GroupAdjacent(
		x => x.key,
		x => x.value,
		StringComparer.OrdinalIgnoreCase);

Console.WriteLine(
	"[ " + 
	string.Join(
		", ", 
		result.Select(c => "[" + string.Join(", ", c) + "]")) +
	" ]");

// This code produces the following output:
// [ [123, 456, 789], [987, 654, 321], [789, 456, 123], [123, 456, 781] ]
