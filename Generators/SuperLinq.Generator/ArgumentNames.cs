namespace SuperLinq;

public record ArgumentNames(string[] Arity, string[] Ordinals)
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
		});
}

