<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var seq1 = Enumerable.Range(1, 5);
var seq2 = Enumerable.Range(1, 4);
var seq3 = Enumerable.Range(1, 3);
	        
// Interleave the elements from multiple sequences into a single sequence
var result = new[] { seq1, seq2, seq3, }
	.Interleave();
	        
Console.WriteLine(string.Join(", ", result));

// This code produces the following output:
// 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 5
