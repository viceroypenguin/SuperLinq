<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

Node root = new(0,
[
	new(1), new(2), new(3),
	new(4,
	[
		new(5), new(6),
		new(7, [ new(8), new(9) ]),
		new(10, [ new(11) ]),
	]),
	new(11),
	new(12,
	[
		new(13), new(14),
		new(15, [ new(16), new(17) ]),
		new(18),
	]),
	new(19), new(20),
]);

// Traverse a tree
var result = SuperEnumerable
	.TraverseDepthFirst(
		root,
		static x => x.Children ?? [])
	.Select(x => x.Id);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20]

record Node(int Id, IEnumerable<Node>? Children = null);
