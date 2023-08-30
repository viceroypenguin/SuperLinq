<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[] { "aaa", "bb", "c", "dddd", };

// Determine if sequence ends with the ends sequence
var result = sequence.ExceptBy(new[] { "a", "b", }, s => s[0]);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [c, dddd]
