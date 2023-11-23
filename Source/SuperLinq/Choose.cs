namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Applies a function to each element of the source sequence and returns a new sequence of result elements for
	///     source elements where the function returns a couple (2-tuple) having a <see langword="true"/> as its first
	///     element and result as the second.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements in <paramref name="source"/>.
	///	</typeparam>
	/// <typeparam name="TResult">
	///	    The type of the elements in the returned sequence.
	///	</typeparam>
	/// <param name="source">
	///	    The source sequence.
	///	</param>
	/// <param name="chooser">
	///	    The function that is applied to each source element.
	/// </param>
	/// <returns>
	///	    A sequence <typeparamref name="TResult"/> elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="chooser"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This method uses deferred execution semantics and streams its results.
	/// </remarks>
	public static IEnumerable<TResult> Choose<T, TResult>(
		this IEnumerable<T> source,
		Func<T, (bool, TResult)> chooser)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(chooser);

		return Core(source, chooser);

		static IEnumerable<TResult> Core(IEnumerable<T> source, Func<T, (bool, TResult)> chooser)
		{
			foreach (var item in source)
			{
				var (some, value) = chooser(item);
				if (some)
					yield return value;
			}
		}
	}
}
