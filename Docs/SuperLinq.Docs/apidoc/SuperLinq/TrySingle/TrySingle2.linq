<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

// Determine cardinality of sequence
var result = Enumerable.Range(10, 0).TrySingle(0, 1, 2, (count, one) => count switch { 0 => "no elements", 1 => $"single({one})", 2 => "many elements" });
Console.WriteLine(result);

result = Enumerable.Range(10, 1).TrySingle(0, 1, 2, (count, one) => count switch { 0 => "no elements", 1 => $"single({one})", 2 => "many elements" });
Console.WriteLine(result);

result = Enumerable.Range(10, 10).TrySingle(0, 1, 2, (count, one) => count switch { 0 => "no elements", 1 => $"single({one})", 2 => "many elements" });
Console.WriteLine(result);

// This code produces the following output:
// no elements
// single(10)
// many elements
