<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new Item[]
{
	new(key: 5, text: "1"),
	new(key: 5, text: "2"),
	new(key: 4, text: "3"),
	new(key: 4, text: "4"),
	new(key: 3, text: "5"),
	new(key: 3, text: "6"),
	new(key: 2, text: "7"),
	new(key: 2, text: "8"),
	new(key: 1, text: "9"),
	new(key: 1, text: "10"),
};

// Rank the items in the sequence
var result = sequence.DenseRank(OrderByDirection.Ascending);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [((1, 9), 1), ((1, 10), 1), ((2, 7), 2), ((2, 8), 2), ((3, 5), 3), ((3, 6), 3), ((4, 3), 4), ((4, 4), 4), ((5, 1), 5), ((5, 2), 5)]

class Item : IComparable<Item>
{
	public Item(int key, string text)
	{
		Key = key;
		Text = text;
	}

	public int Key { get; }
	public string Text { get; }

	public int CompareTo(Item other) =>
		this.Key.CompareTo(other.Key);

	public override string ToString() =>
		$"({this.Key}, {this.Text})"; 
}
