<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var matrix = new[]
{
    new[] { 10, 11 },
    new[] { 20 },
    new[] { 30, 31, 32 }
};

// Transpose a 2d sequence
var result = matrix.Transpose();

Console.WriteLine(
	"[" + Environment.NewLine +
	string.Join(
		", " + Environment.NewLine,
		result.Select(c => "   [" + string.Join(", ", c) + "]")) +
	Environment.NewLine + "]");

// This code produces the following output:
// [
//    [10, 20, 30],
//    [11, 31],
//    [32]
// ]
