using System.Collections;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Flattens a sequence containing arbitrarily-nested sequences.
	/// </summary>
	/// <param name="source">The sequence that will be flattened.</param>
	/// <returns>
	/// A sequence that contains the elements of <paramref name="source"/>
	/// and all nested sequences (except strings).
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>

	public static IEnumerable<object?> Flatten(this IEnumerable source) =>
		Flatten(source, obj => obj is not string);

	/// <summary>
	/// Flattens a sequence containing arbitrarily-nested sequences. An
	/// additional parameter specifies a predicate function used to
	/// determine whether a nested <see cref="IEnumerable"/> should be
	/// flattened or not.
	/// </summary>
	/// <param name="source">The sequence that will be flattened.</param>
	/// <param name="predicate">
	/// A function that receives each element that implements
	/// <see cref="IEnumerable"/> and indicates if its elements should be
	/// recursively flattened into the resulting sequence.
	/// </param>
	/// <returns>
	/// A sequence that contains the elements of <paramref name="source"/>
	/// and all nested sequences for which the predicate function
	/// returned <see langword="true"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	/// <paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException">
	/// <paramref name="predicate"/> is <see langword="null"/>.</exception>

	public static IEnumerable<object?> Flatten(this IEnumerable source, Func<IEnumerable, bool> predicate)
	{
		predicate.ThrowIfNull();

		return Flatten(source, obj => obj is IEnumerable inner && predicate(inner) ? inner : null);
	}

	/// <summary>
	/// Flattens a sequence containing arbitrarily-nested sequences. An
	/// additional parameter specifies a function that projects an inner
	/// sequence via a property of an object.
	/// </summary>
	/// <param name="source">The sequence that will be flattened.</param>
	/// <param name="selector">
	/// A function that receives each element of the sequence as an object
	/// and projects an inner sequence to be flattened. If the function
	/// returns <see langword="null"/> then the object argument is considered a leaf
	/// of the flattening process.
	/// </param>
	/// <returns>
	/// A sequence that contains the elements of <paramref name="source"/>
	/// and all nested sequences projected via the
	/// <paramref name="selector"/> function.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	/// <paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException">
	/// <paramref name="selector"/> is <see langword="null"/>.</exception>

	public static IEnumerable<object?> Flatten(this IEnumerable source, Func<object?, IEnumerable?> selector)
	{
		source.ThrowIfNull();
		selector.ThrowIfNull();

		return _(); IEnumerable<object?> _()
		{
			var e = source.GetEnumerator();
			var stack = new Stack<IEnumerator>();

			stack.Push(e);

			try
			{
				while (stack.Any())
				{
					e = stack.Pop();

reloop:

					while (e.MoveNext())
					{
						if (selector(e.Current) is { } inner)
						{
							stack.Push(e);
							e = inner.GetEnumerator();
							goto reloop;
						}
						else
						{
							yield return e.Current;
						}
					}

					(e as IDisposable)?.Dispose();
					e = null;
				}
			}
			finally
			{
				(e as IDisposable)?.Dispose();
				foreach (var se in stack)
					(se as IDisposable)?.Dispose();
			}
		}
	}
}
