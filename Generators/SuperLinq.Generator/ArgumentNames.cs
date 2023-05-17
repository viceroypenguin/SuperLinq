namespace SuperLinq.Generator;

/// <summary>
/// int->string translations of different types.
/// </summary>
public record ArgumentNames(string[] Arity, string[] Ordinals, string[] Cardinals)
{
	/// <summary>
	/// int->string translations of different types.
	/// </summary>
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

