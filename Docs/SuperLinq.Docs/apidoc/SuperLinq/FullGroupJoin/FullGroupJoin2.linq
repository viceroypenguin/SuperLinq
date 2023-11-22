<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
</Query>

var people = new Person[]
{
	new("John Doe", 1),
	new("Jane Doe", 1),
	new("Lucy Ricardo", 2),
	new("Ricky Ricardo", 2),
	new("Fred Mertz", 3),
	new("Ethel Mertz", 3),
};

var pets = new Pet[]
{
	new("Bear", 103),
	new("Polly", 102),
	new("Minnie", 102),
	new("Mittens", 101),
	new("Patches", 101),
	new("Paws", 101),
};

var results = people
	.FullGroupJoin(
		pets,
		p => p.FamilyId,
		p => p.FamilyId,
		new IntBy100Comparer());

foreach (var (familyId, familyPeople, familyPets) in results)
{
	var str1 = string.Join(", ", familyPeople.Select(p => p.Name));
	var str2 = string.Join(", ", familyPets.Select(p => p.Name));
	Console.WriteLine($"({familyId}, [{str1}], [{str2}])");
}

// This code produces the following output:
// (1, [John Doe, Jane Doe], [Mittens, Patches, Paws])
// (2, [Lucy Ricardo, Ricky Ricardo], [Polly, Minnie])
// (3, [Fred Mertz, Ethel Mertz], [Bear])


record Person(string Name, int FamilyId);
record Pet(string Name, int FamilyId);

class IntBy100Comparer : IEqualityComparer<int>
{
	public bool Equals(int x, int y) =>
		x % 100 == y % 100;

	public int GetHashCode(int obj) =>
		(obj % 100).GetHashCode();
}
