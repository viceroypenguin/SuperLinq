<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var s1 = new[] { 11, 7, 3 };
var s2 = new[] { 20, 4, 2 };
var s3 = new[] { 25, 19, 17 };

// Execute a sorted merge of multiple sequences into a single sequence
var result = s1
	.SortedMerge(OrderByDirection.Descending, s2, s3);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// One possible random output is as follows:
// [25, 20, 19, 17, 11, 7, 4, 3, 2]
