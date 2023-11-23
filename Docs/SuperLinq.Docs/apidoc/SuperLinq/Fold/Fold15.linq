<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 15);

// Fold a sequence into a single value.
var result = sequence
	.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) =>
		a + b + c + d + e + f + g + h + i + j + k + l + m + n + o);

Console.WriteLine(result);

// This code produces the following output:
// 120
