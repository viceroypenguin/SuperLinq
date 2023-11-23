<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 10);

// Hold a resource for the duration of an enumeration
var result = SuperEnumerable
	.Using(
		() => new DummyDisposable(),
		d => d.GetValues())
	.Do(x => Console.Write($"{x}, "));

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// Constructor
// GetValues
// 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
// Dispose
// [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

class DummyDisposable : IDisposable
{
	public DummyDisposable()
	{
		Console.WriteLine("Constructor");
	}

	public IEnumerable<int> GetValues()
	{
		Console.WriteLine("GetValues");
		return Enumerable.Range(1, 10);
	}
	
	public void Dispose()
	{
		Console.WriteLine();
		Console.WriteLine("Dispose");
	}
}
