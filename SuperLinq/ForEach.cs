namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Immediately executes the given action on each element in the source sequence.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the sequence</typeparam>
	/// <param name="source">The sequence of elements</param>
	/// <param name="action">The action to execute on each element</param>

	public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		if (action == null) throw new ArgumentNullException(nameof(action));

		foreach (var element in source)
			action(element);
	}

	/// <summary>
	/// Immediately executes the given action on each element in the source sequence.
	/// Each element's index is used in the logic of the action.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the sequence</typeparam>
	/// <param name="source">The sequence of elements</param>
	/// <param name="action">The action to execute on each element; the second parameter
	/// of the action represents the index of the source element.</param>

	public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		if (action == null) throw new ArgumentNullException(nameof(action));

		var index = 0;
		foreach (var element in source)
			action(element, index++);
	}
}
