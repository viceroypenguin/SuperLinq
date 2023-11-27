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
	.Index(5);

Console.WriteLine(
	"[ " +
	string.Join(", ", result) +
	" ]");

// This code produces the following output:
// [ (5, (jan, 123)), (6, (Jan, 456)), (7, (JAN, 789)) ]
