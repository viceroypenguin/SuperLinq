﻿namespace SuperLinq.Tests;

internal static class BreakingCollection
{
	public static BreakingCollection<T> AsBreakingCollection<T>(this IEnumerable<T> source) => new(source);
}

internal class BreakingCollection<T> : BreakingSequence<T>, ICollection<T>, IDisposableEnumerable<T>
{
	protected readonly IList<T> List;

	public BreakingCollection(params T[] values) : this((IList<T>)values) { }
	public BreakingCollection(IEnumerable<T> source) => List = source.ToList();
	public BreakingCollection(IList<T> list) => List = list;

	public int Count => List.Count;

	public void Add(T item) => Assert.Fail("LINQ Operators should not be calling this method.");
	public void Clear() => Assert.Fail("LINQ Operators should not be calling this method.");
	public bool Contains(T item) => List.Contains(item);
	public bool Remove(T item) { Assert.Fail("LINQ Operators should not be calling this method."); return false; }
	public bool IsReadOnly => true;

	public int CopyCount { get; private set; }
	public virtual void CopyTo(T[] array, int arrayIndex)
	{
		List.CopyTo(array, arrayIndex);
		CopyCount++;
	}

	public void Dispose() { }
}
