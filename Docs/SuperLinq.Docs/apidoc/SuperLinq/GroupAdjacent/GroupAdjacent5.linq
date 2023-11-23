<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	(key: 1, value: 123),
	(key: 1, value: 456),
	(key: 1, value: 789),
	(key: 2, value: 987),
	(key: 2, value: 654),
	(key: 2, value: 321),
	(key: 3, value: 789),
	(key: 3, value: 456),
	(key: 3, value: 123),
	(key: 1, value: 123),
	(key: 1, value: 456),
	(key: 1, value: 781),
};

// Group adjacent items
var result = sequence
	.GroupAdjacent(
		x => x.key,
		(k, g) => new { Key = k, Items = "[" + string.Join(", ", g.Select(x => x.value)) + "]", });

Console.WriteLine(
	"[ " + 
	string.Join(
		", ", 
		result) +
	" ]");

// This code produces the following output:
// [ { Key = 1, Items = [123, 456, 789] }, { Key = 2, Items = [987, 654, 321] }, { Key = 3, Items = [789, 456, 123] }, { Key = 1, Items = [123, 456, 781] } ]
