﻿using System.Collections;

namespace Test;

/// <summary>
/// This class implement <see cref="IList{T}"/> but specifically prohibits enumeration using GetEnumerator().
/// It is provided to assist in testing extension methods that MUST NOT call the GetEnumerator()
/// method of <see cref="IEnumerable"/> - either because they should be using the indexer or because they are
/// expected to be lazily evaluated.
/// </summary>
internal sealed class BreakingList<T> : BreakingCollection<T>, IList<T>
{
	public BreakingList(IEnumerable<T> source) : base(source) { }

	public int IndexOf(T item) => List.IndexOf(item);
	public void Insert(int index, T item) => throw new NotImplementedException();
	public void RemoveAt(int index) => throw new NotImplementedException();

	public T this[int index]
	{
		get => List[index];
		set => throw new NotImplementedException();
	}
}
