<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var pets1 = new List<Pet> { new("Turbo", 2), new("Peanut", 8), };
var pets2 = new List<Pet> { new("Peanut", 8), new("Turbo", 2), };

// Determine if the two collections are equal, using a custom equality comparer
var result = pets1
	.CollectionEqual(
		pets2,
		new PetComparer());

Console.WriteLine(result);

// This code produces the following output:
// True

class Pet
{
	public Pet(string name, int age)
	{
		this.Name = name;
		this.Age = age;
	}

	public string Name { get; }
	public int Age { get; }
}

class PetComparer : IEqualityComparer<Pet>
{
	public bool Equals(Pet x, Pet y) =>
		x.Name == y.Name
		&& x.Age == y.Age;
	public int GetHashCode(Pet x) =>
		HashCode.Combine(x.Name, x.Age);
}
