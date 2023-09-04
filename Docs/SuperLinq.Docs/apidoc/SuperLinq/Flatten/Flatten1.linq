<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new object[]
{
	1, 2, 3,
	new object[]
	{
		4, 5,
		new object[] { 6, 7, },
		8, 9, 10, 
	},
	11,
	new object[]
	{
		12, 13, 14,
		new object[] { 15, 16, 17, },
		18, 19,
	},
	20,
};

// Flatten a hierarchical sequence
var result = sequence.Flatten();

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20]
