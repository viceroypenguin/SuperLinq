<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);

// Determine if sequence ends with the ends sequence
var ends = Enumerable.Range(4, 2);
var result = sequence.EndsWith(ends);

Console.WriteLine($"EndsWith: {result}");

// This code produces the following output:
// EndsWith: True
