<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 14);

// Fold a sequence into a single value.
var result = sequence
	.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n) =>
		a + b + c + d + e + f + g + h + i + j + k + l + m + n);

Console.WriteLine(result);

// This code produces the following output:
// 105
