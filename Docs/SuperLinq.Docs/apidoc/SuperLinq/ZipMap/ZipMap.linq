<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", };

// Filter a sequence based on a leading value
var result = sequence
	.ZipMap(x => x.ToString().Length);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(one, 3), (two, 3), (three, 5), (four, 4), (five, 4), (six, 3), (seven, 5), (eight, 5), (nine, 4), (ten, 3)]
