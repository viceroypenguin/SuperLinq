<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);

// Get a countdown counter for the the sequence
var result = sequence.CountDown(2, (item, cd) => new { Item = item, CountDown = cd?.ToString() ?? "null", });

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [{ Item = 1, CountDown = null }, { Item = 2, CountDown = null }, { Item = 3, CountDown = null }, { Item = 4, CountDown = 1 }, { Item = 5, CountDown = 0 }]
