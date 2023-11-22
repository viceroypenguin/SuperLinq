<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var seq1 = new[] { "f", "ba", "qax" };
var seq2 = new[] { "f", "ba", "qax", "fuba", };
var seq3 = new[] { "f", "ba", "qax", "FUBA", };

// determine if a sequence has duplicate items
var result = seq1
	.HasDuplicates(
		x => x[0..1]);
Console.WriteLine($"Has Duplicates: {result}");

result = seq2
	.HasDuplicates(
		x => x[0..1]);
Console.WriteLine($"Has Duplicates: {result}");

result = seq3
	.HasDuplicates(
		x => x[0..1]);
Console.WriteLine($"Has Duplicates: {result}");

// This code produces the following output:
// Has Duplicates: False
// Has Duplicates: True
// Has Duplicates: False
