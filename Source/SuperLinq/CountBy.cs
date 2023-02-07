namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Applies a key-generating function to each element of a sequence and returns a sequence of
	/// unique keys and their number of occurrences in the original sequence.
	/// </summary>
	/// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
	/// <typeparam name="TKey">Type of the projected element.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="keySelector">Function that transforms each item of source sequence into a key to be compared against the others.</param>
	/// <returns>A sequence of unique keys and their number of occurrences in the original sequence.</returns>

	public static IEnumerable<(TKey key, int count)> CountBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		return source.CountBy(keySelector, comparer: null);
	}

	/// <summary>
	/// Applies a key-generating function to each element of a sequence and returns a sequence of
	/// unique keys and their number of occurrences in the original sequence.
	/// An additional argument specifies a comparer to use for testing equivalence of keys.
	/// </summary>
	/// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
	/// <typeparam name="TKey">Type of the projected element.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="keySelector">Function that transforms each item of source sequence into a key to be compared against the others.</param>
	/// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
	/// If null, the default equality comparer for <typeparamref name="TSource"/> is used.</param>
	/// <returns>A sequence of unique keys and their number of occurrences in the original sequence.</returns>

	public static IEnumerable<(TKey key, int count)> CountBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);

		return Core(source, keySelector, comparer ?? EqualityComparer<TKey>.Default);

		static IEnumerable<(TKey key, int count)> Core(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			// Avoid the temptation to inline the Loop method, which
			// exists solely to separate the scope & lifetimes of the
			// locals needed for the actual looping of the source &
			// production of the results (that happens once at the start
			// of iteration) from those needed to simply yield the
			// results. It is harder to reason about the lifetimes (if the
			// code is inlined) with respect to how the compiler will
			// rewrite the iterator code as a state machine. For
			// background, see:
			// http://blog.stephencleary.com/2010/02/q-should-i-set-variables-to-null-to.html

			var (keys, counts) = Loop(source, keySelector, comparer);

			for (var i = 0; i < keys.Count; i++)
				yield return (keys[i], counts[i]);
		}

		static (List<TKey>, List<int>) Loop(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> cmp)
		{
			var dic = new Collections.NullKeyDictionary<TKey, int>(cmp);

			var keys = new List<TKey>();
			var counts = new List<int>();

			foreach (var item in source)
			{
				var key = keySelector(item);

				if (dic.TryGetValue(key, out var index))
				{
					counts[index]++;
				}
				else
				{
					index = keys.Count;
					dic[key] = index;
					keys.Add(key);
					counts.Add(1);
				}
			}

			return (keys, counts);
		}
	}
}
