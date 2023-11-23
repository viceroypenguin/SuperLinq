<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var s1 = new[] { 3, 7, 11 };
var s2 = new[] { 2, 4, 20 };
var s3 = new[] { 17, 19, 25 };

// Execute a sorted merge of multiple sequences into a single sequence
var result = s1
	.SortedMergeBy(x => -x, OrderByDirection.Descending, s2, s3);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// One possible random output is as follows:
// [2, 3, 4, 7, 11, 17, 19, 20, 25]
