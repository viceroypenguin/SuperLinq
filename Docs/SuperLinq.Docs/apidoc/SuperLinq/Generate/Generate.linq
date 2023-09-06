<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

// Generate a sequence using a generator function
var result = SuperEnumerable
	.Generate(1, n => n * 2)
	.Take(10);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 2, 4, 8, 16, 32, 64, 128, 256, 512]
