<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var seq1 = new[] { "foo", "bar", "baz" };
var seq2 = new[] { "foo", "bar", "baz", "foo", };
var seq3 = new[] { "foo", "bar", "baz", "FOO", };

// determine if a sequence has duplicate items
var result = seq1
	.HasDuplicates();
Console.WriteLine($"Has Duplicates: {result}");

result = seq2
	.HasDuplicates();
Console.WriteLine($"Has Duplicates: {result}");

result = seq3
	.HasDuplicates();
Console.WriteLine($"Has Duplicates: {result}");

// This code produces the following output:
// Has Duplicates: False
// Has Duplicates: True
// Has Duplicates: False
