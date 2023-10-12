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
		x => x.key);

Console.WriteLine(
	"[ " + 
	string.Join(
		", ", 
		result.Select(c => "[" + string.Join(", ", c) + "]")) +
	" ]");

// This code produces the following output:
// [ [(1, 123), (1, 456), (1, 789)], [(2, 987), (2, 654), (2, 321)], [(3, 789), (3, 456), (3, 123)], [(1, 123), (1, 456), (1, 781)] ]
