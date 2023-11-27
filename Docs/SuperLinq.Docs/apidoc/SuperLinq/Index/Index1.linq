<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	(key: "jan", value: 123),
	(key: "Jan", value: 456),
	(key: "JAN", value: 789),
};

// Index a sequence
var result = sequence
	.Index();

Console.WriteLine(
	"[ " +
	string.Join(", ", result) +
	" ]");

// This code produces the following output:
// [ (0, (jan, 123)), (1, (Jan, 456)), (2, (JAN, 789)) ]
