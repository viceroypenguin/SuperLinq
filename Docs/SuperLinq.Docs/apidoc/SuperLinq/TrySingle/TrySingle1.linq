<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

// Determine cardinality of sequence
var result = Enumerable.Range(1, 0).TrySingle("none", "one", "many");
Console.WriteLine(result.ToString());

result = Enumerable.Range(1, 1).TrySingle("none", "one", "many");
Console.WriteLine(result.ToString());

result = Enumerable.Range(1, 10).TrySingle("none", "one", "many");
Console.WriteLine(result.ToString());

// This code produces the following output:
// (none, 0)
// (one, 1)
// (many, 0)
