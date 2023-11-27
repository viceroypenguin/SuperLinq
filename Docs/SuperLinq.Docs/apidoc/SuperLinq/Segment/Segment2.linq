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
	.Segment(
		(x, i) => i % 3 == 0);

Console.WriteLine(
	"[" + Environment.NewLine + 
	string.Join(
		", " + Environment.NewLine, 
		result.Select(c => "   [" + string.Join(", ", c) + "]")) +
	Environment.NewLine + "]");

// This code produces the following output:
// [
//    [(jan, 123), (Jan, 456), (JAN, 789)],
//    [(feb, 987), (Feb, 654), (FEB, 321)],
//    [(mar, 789), (Mar, 456), (MAR, 123)],
//    [(jan, 123), (Jan, 456), (JAN, 781)]
// ]
