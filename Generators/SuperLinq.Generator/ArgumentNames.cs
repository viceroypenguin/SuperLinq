namespace SuperLinq;

public record ArgumentNames(string[] Arity, string[] Ordinals, string[] Cardinals)
{
	public static ArgumentNames Instance { get; } = new(
		Arity: new[]
		{
			"zero" ,
			"one"  ,
			"two"  ,
			"three",
			"four" ,
			"five" ,
			"six"  ,
			"seven",
			"eight",
		},
		Ordinals: new[]
		{
			"zeroth" ,
			"first"  ,
			"second" ,
			"third"  ,
			"fourth" ,
			"fifth"  ,
			"sixth"  ,
			"seventh",
			"eighth" ,
		},
		Cardinals: new[]
		{
			"Zeroth" ,
			"First"  ,
			"Second" ,
			"Third"  ,
			"Fourth" ,
			"Fifth"  ,
			"Sixth"  ,
			"Seventh",
			"Eighth" ,
		});
}

