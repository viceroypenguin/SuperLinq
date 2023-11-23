<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	(cnt: 1, item: "A"),
	(cnt: 2, item: "B"),
	(cnt: 3, item: "C"),
	(cnt: 4, item: "D"),
	(cnt: 5, item: "E"),
	(cnt: 1, item: "a"),
	(cnt: 2, item: "b"),
	(cnt: 3, item: "c"),
	(cnt: 4, item: "d"),
	(cnt: 5, item: "e"),
}.AsEnumerable();

// Find the maximum items of the sequence
var result = sequence
	.MaxItemsBy(x => x.cnt);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(5, E), (5, e)]
