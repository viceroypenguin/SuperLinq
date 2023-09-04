<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 16);

// Fold a sequence into a single value.
var result = sequence
	.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) =>
		a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p);

Console.WriteLine(result);

// This code produces the following output:
// 136
