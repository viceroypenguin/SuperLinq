// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace SuperLinq.Benchmarks;

[SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable")]
public class TestDataBenchmark
{
	public static IEnumerable<LinqTestData> NoSpecialization()
	{
		yield return LinqTestData.s_iEnumerable;
	}

	public static IEnumerable<LinqTestData> CollectionSpecialization()
	{
		yield return LinqTestData.s_iEnumerable;
		yield return LinqTestData.s_iCollection;
	}

	public static IEnumerable<LinqTestData> CountSpecialization()
	{
		yield return LinqTestData.s_iEnumerable;
		yield return LinqTestData.s_range;
		yield return LinqTestData.s_iCollection;
	}

	public static IEnumerable<LinqTestData> ListSpecialization()
	{
		yield return LinqTestData.s_iEnumerable;
		yield return LinqTestData.s_array;
		yield return LinqTestData.s_list;
		yield return LinqTestData.s_iList;
	}

	public static IEnumerable<LinqTestData> FullSpecialization()
	{
		yield return LinqTestData.s_iEnumerable;
		yield return LinqTestData.s_iCollection;
		yield return LinqTestData.s_array;
		yield return LinqTestData.s_list;
		yield return LinqTestData.s_iList;
	}
}

public sealed class LinqTestData
{
	// this field is a const (not instance field) to avoid creating closures in tested LINQ
	internal const int Size = 100;

	private static readonly int[] s_arrayOf100Integers = Enumerable.Range(0, Size).ToArray();

	internal static readonly LinqTestData s_array = new(s_arrayOf100Integers);
	internal static readonly LinqTestData s_list = new(new List<int>(s_arrayOf100Integers));
	internal static readonly LinqTestData s_range = new(Enumerable.Range(0, Size));
	internal static readonly LinqTestData s_iEnumerable = new(new IEnumerableWrapper<int>(s_arrayOf100Integers));
	internal static readonly LinqTestData s_iList = new(new IListWrapper<int>(s_arrayOf100Integers));
	internal static readonly LinqTestData s_iCollection = new(new ICollectionWrapper<int>(s_arrayOf100Integers));
	internal static readonly LinqTestData s_iOrderedEnumerable = new(s_arrayOf100Integers.OrderBy(i => i)); // OrderBy() returns IOrderedEnumerable (OrderedEnumerable is internal)

	private LinqTestData(IEnumerable<int> collection) => Collection = collection;

	internal IEnumerable<int> Collection { get; }

	public override string ToString()
	{
		if (ReferenceEquals(this, s_range)) // RangeIterator is a private type
			return "Range";

		return Collection switch
		{
			int[] => "Array",
			List<int> => "List",
			IList<int> => "IList",
			ICollection<int> => "ICollection",
			IOrderedEnumerable<int> => "IOrderedEnumerable",
			_ => "IEnumerable",
		};
	}

	private sealed class IEnumerableWrapper<T>(T[] array) : IEnumerable<T>
	{
		public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)array).GetEnumerator();
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => ((IEnumerable<T>)array).GetEnumerator();
	}

	private sealed class ICollectionWrapper<T>(T[] array) : ICollection<T>
	{
		private readonly T[] _array = array;

		public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();

		public int Count => _array.Length;
		public bool IsReadOnly => true;
		public bool Contains(T item) => Array.IndexOf(_array, item) >= 0;
		public void CopyTo(T[] array, int arrayIndex) => _array.CopyTo(array, arrayIndex);

		public void Add(T item) => throw new NotSupportedException();
		public void Clear() => throw new NotSupportedException();
		public bool Remove(T item) => throw new NotSupportedException();
	}

	private sealed class IListWrapper<T>(T[] array) : IList<T>
	{
		private readonly T[] _array = array;

		public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();

		public int Count => _array.Length;
		public bool IsReadOnly => true;
		public T this[int index]
		{
			get => _array[index];
			set => throw new NotSupportedException();
		}
		public bool Contains(T item) => Array.IndexOf(_array, item) >= 0;
		public void CopyTo(T[] array, int arrayIndex) => _array.CopyTo(array, arrayIndex);
		public int IndexOf(T item) => Array.IndexOf(_array, item);

		public void Add(T item) => throw new NotSupportedException();
		public void Clear() => throw new NotSupportedException();
		public bool Remove(T item) => throw new NotSupportedException();
		public void Insert(int index, T item) => throw new NotSupportedException();
		public void RemoveAt(int index) => throw new NotSupportedException();
	}
}
