<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
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
	new("Bear", 3),
	new("Polly", 2),
	new("Minnie", 2),
	new("Mittens", 1),
	new("Patches", 1),
	new("Paws", 1),
};

var results = people
	.FullGroupJoin(
		pets,
		p => p.FamilyId,
		p => p.FamilyId);

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
