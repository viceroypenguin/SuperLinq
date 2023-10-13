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
		Arity:
		[
			"zero",
			"one",
			"two",
			"three",
			"four",
			"five",
			"six",
			"seven",
			"eight",
		],
		Ordinals:
		[
			"zeroth",
			"first",
			"second",
			"third",
			"fourth",
			"fifth",
			"sixth",
			"seventh",
			"eighth",
		],
		Cardinals:
		[
			"Zeroth",
			"First",
			"Second",
			"Third",
			"Fourth",
			"Fifth",
			"Sixth",
			"Seventh",
			"Eighth",
		]);
}

