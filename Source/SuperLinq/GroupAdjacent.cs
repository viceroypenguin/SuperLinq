﻿using System.Collections;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Groups the adjacent elements of a sequence according to a specified key selector function.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key returned by <paramref name="keySelector"/>.
	/// </typeparam>
	/// <param name="source">
	///	    A sequence whose elements to group.
	/// </param>
	/// <param name="keySelector">
	///	    A function to extract the key for each element.
	/// </param>
	/// <returns>
	///	    A sequence of groupings where each grouping (<see cref="IGrouping{TKey,TElement}"/>) contains the key and
	///     the adjacent elements in the same order as found in the source sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This method is implemented by using deferred execution and streams the groupings. The grouping elements,
	///     however, are buffered. Each grouping is therefore yielded as soon as it is complete and before the next
	///     grouping occurs.
	/// </remarks>
	public static IEnumerable<IGrouping<TKey, TSource>> GroupAdjacent<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector)
	{
		return GroupAdjacent(source, keySelector, Identity);
	}

	/// <summary>
	///	    Groups the adjacent elements of a sequence according to a specified key selector function and compares the
	///     keys by using a specified comparer.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key returned by <paramref name="keySelector"/>.
	/// </typeparam>
	/// <param name="source">
	///	    A sequence whose elements to group.
	/// </param>
	/// <param name="keySelector">
	///	    A function to extract the key for each element.
	/// </param>
	/// <param name="comparer">
	///	    An <see cref="IEqualityComparer{T}"/> to compare keys.
	/// </param>
	/// <returns>
	///	    A sequence of groupings where each grouping (<see cref="IGrouping{TKey,TElement}"/>) contains the key and
	///     the adjacent elements in the same order as found in the source sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This method is implemented by using deferred execution and streams the groupings. The grouping elements,
	///     however, are buffered. Each grouping is therefore yielded as soon as it is complete and before the next
	///     grouping occurs.
	/// </remarks>
	public static IEnumerable<IGrouping<TKey, TSource>> GroupAdjacent<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		IEqualityComparer<TKey>? comparer)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);

		return GroupAdjacent(source, keySelector, Identity, comparer);
	}

	/// <summary>
	///	    Groups the adjacent elements of a sequence according to a specified key selector function and projects the
	///     elements for each group by using a specified function.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	///	</typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key returned by <paramref name="keySelector"/>.
	///	</typeparam>
	/// <typeparam name="TElement">
	///	    The type of the elements in the resulting groupings.
	///	</typeparam>
	/// <param name="source">
	///	    A sequence whose elements to group.
	///	</param>
	/// <param name="keySelector">
	///	    A function to extract the key for each element.
	///	</param>
	/// <param name="elementSelector">
	///	    A function to map each source element to an element in the resulting grouping.
	///	</param>
	/// <returns>
	///	    A sequence of groupings where each grouping (<see cref="IGrouping{TKey,TElement}"/>) contains the key and
	///     the adjacent elements (of type <typeparamref name="TElement"/>) in the same order as found in the source
	///     sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="keySelector"/>, or <paramref name="elementSelector"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This method is implemented by using deferred execution and streams the groupings. The grouping elements,
	///     however, are buffered. Each grouping is therefore yielded as soon as it is complete and before the next
	///     grouping occurs.
	/// </remarks>
	public static IEnumerable<IGrouping<TKey, TElement>> GroupAdjacent<TSource, TKey, TElement>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		Func<TSource, TElement> elementSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);
		Guard.IsNotNull(elementSelector);

		return GroupAdjacentImpl(
			source, keySelector, elementSelector,
			CreateGroupAdjacentGrouping,
			comparer: null);
	}

	/// <summary>
	///	    Groups the adjacent elements of a sequence according to a specified key selector function. The keys are
	///     compared by using a comparer and each group's elements are projected by using a specified function.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key returned by <paramref name="keySelector"/>.
	/// </typeparam>
	/// <typeparam name="TElement">
	///	    The type of the elements in the resulting groupings.
	/// </typeparam>
	/// <param name="source">
	///	    A sequence whose elements to group.
	/// </param>
	/// <param name="keySelector">
	///	    A function to extract the key for each element.
	/// </param>
	/// <param name="elementSelector">
	///	    A function to map each source element to an element in the resulting grouping.
	/// </param>
	/// <param name="comparer">
	///	    An <see cref="IEqualityComparer{T}"/> to compare keys.</param>
	/// <returns>
	///	    A sequence of groupings where each grouping (<see cref="IGrouping{TKey,TElement}"/>) contains the key and
	///     the adjacent elements (of type <typeparamref name="TElement"/>) in the same order as found in the source
	///     sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="keySelector"/>, or <paramref name="elementSelector"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This method is implemented by using deferred execution and streams the groupings. The grouping elements,
	///     however, are buffered. Each grouping is therefore yielded as soon as it is complete and before the next
	///     grouping occurs.
	/// </remarks>
	public static IEnumerable<IGrouping<TKey, TElement>> GroupAdjacent<TSource, TKey, TElement>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		Func<TSource, TElement> elementSelector,
		IEqualityComparer<TKey>? comparer)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);
		Guard.IsNotNull(elementSelector);

		return GroupAdjacentImpl(
			source, keySelector, elementSelector,
			CreateGroupAdjacentGrouping,
			comparer);
	}

	/// <summary>
	///	    Groups the adjacent elements of a sequence according to a specified key selector function. The keys are
	///     compared by using a comparer and each group's elements are projected by using a specified function.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key returned by <paramref name="keySelector"/>.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the elements in the resulting sequence.
	/// </typeparam>
	/// <param name="source">
	///	    A sequence whose elements to group.
	/// </param>
	/// <param name="keySelector">
	///	    A function to extract the key for each element.
	/// </param>
	/// <param name="resultSelector">
	///	    A function to map each key and associated source elements to a result object.
	/// </param>
	/// <returns>
	///	    A collection of elements of type
	/// <typeparamref name="TResult" /> where each element represents a projection over a group and its key.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="keySelector"/>, or <paramref name="resultSelector"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This method is implemented by using deferred execution and streams the groupings. The grouping elements,
	///     however, are buffered. Each grouping is therefore yielded as soon as it is complete and before the next
	///     grouping occurs.
	/// </remarks>
	public static IEnumerable<TResult> GroupAdjacent<TSource, TKey, TResult>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);
		Guard.IsNotNull(resultSelector);

		return GroupAdjacentImpl(
			source, keySelector, Identity,
			resultSelector,
			comparer: null);
	}

	/// <summary>
	///	    Groups the adjacent elements of a sequence according to a specified key selector function. The keys are
	///     compared by using a comparer and each group's elements are projected by using a specified function.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key returned by <paramref name="keySelector"/>.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the elements in the resulting sequence.
	/// </typeparam>
	/// <param name="source">
	///	    A sequence whose elements to group.
	/// </param>
	/// <param name="keySelector">
	///	    A function to extract the key for each element.
	/// </param>
	/// <param name="resultSelector">
	///	    A function to map each key and associated source elements to a result object.
	/// </param>
	/// <param name="comparer">
	///	    An <see cref="IEqualityComparer{TKey}"/> to compare keys.
	/// </param>
	/// <returns>
	///	    A collection of elements of type
	/// <typeparamref name="TResult" /> where each element represents a projection over a group and its key.</returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="keySelector"/>, or <paramref name="resultSelector"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This method is implemented by using deferred execution and streams the groupings. The grouping elements,
	///     however, are buffered. Each grouping is therefore yielded as soon as it is complete and before the next
	///     grouping occurs.
	/// </remarks>
	public static IEnumerable<TResult> GroupAdjacent<TSource, TKey, TResult>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		Func<TKey, IEnumerable<TSource>, TResult> resultSelector,
		IEqualityComparer<TKey>? comparer)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);
		Guard.IsNotNull(resultSelector);

		return GroupAdjacentImpl(
			source, keySelector, Identity,
			resultSelector,
			comparer);
	}

	private static IEnumerable<TResult> GroupAdjacentImpl<TSource, TKey, TElement, TResult>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		Func<TSource, TElement> elementSelector,
		Func<TKey, List<TElement>, TResult> resultSelector,
		IEqualityComparer<TKey>? comparer)
	{
		comparer ??= EqualityComparer<TKey>.Default;

		using var iterator = source.GetEnumerator();

		if (!iterator.MoveNext())
			yield break;

		var k = keySelector(iterator.Current);
		var members = new List<TElement>() { elementSelector(iterator.Current), };

		while (iterator.MoveNext())
		{
			var key = keySelector(iterator.Current);
			var element = elementSelector(iterator.Current);

			if (!comparer.Equals(k, key))
			{
				yield return resultSelector(k, members);
				k = key;
				members = new List<TElement>();
			}

			members.Add(element);
		}

		yield return resultSelector(k, members);
	}

	private static Grouping<TKey, TElement> CreateGroupAdjacentGrouping<TKey, TElement>(TKey key, List<TElement> members) =>
		new(key, members.AsReadOnly());

	[Serializable]
	private sealed class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
	{
		private readonly IEnumerable<TElement> _members;

		public Grouping(TKey key, IEnumerable<TElement> members)
		{
			Key = key;
			_members = members;
		}

		public TKey Key { get; }

		public IEnumerator<TElement> GetEnumerator() => _members.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
