namespace SuperLinq;

public partial class SuperEnumerable
{
	/// <summary>
	///	    Immediately executes the given action on each element in the source sequence.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements in the sequence
	/// </typeparam>
	/// <param name="source">
	///	    The sequence of elements
	/// </param>
	/// <param name="action">
	///	    The action to execute on each element
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="action"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator executes immediately.
	/// </remarks>
	public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(action);

		foreach (var element in source)
			action(element);
	}

	/// <summary>
	///	    Immediately executes the given action on each element in the source sequence. Each element's index is used
	///     in the logic of the action.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements in the sequence
	/// </typeparam>
	/// <param name="source">
	///	    The sequence of elements
	/// </param>
	/// <param name="action">
	///	    The action to execute on each element; the second parameter of the action represents the index of the source
	///     element.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="action"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator executes immediately.
	/// </remarks>
	public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(action);

		var index = 0;
		foreach (var element in source)
			action(element, index++);
	}
}
