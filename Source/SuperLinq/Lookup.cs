using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

/// <summary>
/// A <see cref="ILookup{TKey, TElement}"/> implementation that preserves insertion order
/// </summary>
/// <remarks>
/// This implementation preserves insertion order of keys and elements within each <see
/// cref="IEnumerable{T}"/>. Copied and modified from
/// <c><a href="https://github.com/dotnet/runtime/blob/v7.0.0/src/libraries/System.Linq/src/System/Linq/Lookup.cs">Lookup.cs</a></c>
/// </remarks>
[DebuggerDisplay("Count = {Count}")]
[ExcludeFromCodeCoverage]
internal sealed class Lookup<TKey, TElement> : ILookup<TKey, TElement>
{
	private readonly IEqualityComparer<TKey> _comparer;
	private Grouping[] _groupings;
	private Grouping? _lastGrouping;

	internal static Lookup<TKey, TElement> Create<TSource>(
		IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		Func<TSource, TElement> elementSelector,
		IEqualityComparer<TKey>? comparer
	)
	{
		Debug.Assert(source is not null);
		Debug.Assert(keySelector is not null);
		Debug.Assert(elementSelector is not null);

		var lookup = new Lookup<TKey, TElement>(comparer);

		foreach (var item in source)
		{
			var grouping = Debug.AssertNotNull(lookup.GetGrouping(keySelector(item), create: true));
			grouping.Add(elementSelector(item));
		}

		return lookup;
	}

	internal static Lookup<TKey, TElement> Create(
		IEnumerable<TElement> source,
		Func<TElement, TKey> keySelector,
		IEqualityComparer<TKey>? comparer
	)
	{
		Debug.Assert(source is not null);
		Debug.Assert(keySelector is not null);

		var lookup = new Lookup<TKey, TElement>(comparer);

		foreach (var item in source)
		{
			var grouping = Debug.AssertNotNull(lookup.GetGrouping(keySelector(item), create: true));
			grouping.Add(item);
		}

		return lookup;
	}

	internal static Lookup<TKey, TElement> CreateForJoin(
		IEnumerable<TElement> source,
		Func<TElement, TKey> keySelector,
		IEqualityComparer<TKey>? comparer
	)
	{
		var lookup = new Lookup<TKey, TElement>(comparer);

		foreach (var item in source)
		{
			if (keySelector(item) is { } key)
			{
				var grouping = Debug.AssertNotNull(lookup.GetGrouping(key, create: true));
				grouping.Add(item);
			}
		}

		return lookup;
	}

	private Lookup(IEqualityComparer<TKey>? comparer)
	{
		_comparer = comparer ?? EqualityComparer<TKey>.Default;
		_groupings = new Grouping[7];
	}

	public int Count { get; private set; }

	public IEnumerable<TElement> this[TKey key]
	{
		get
		{
			var grouping = GetGrouping(key, create: false);
			return grouping ?? Enumerable.Empty<TElement>();
		}
	}

	public bool Contains(TKey key) => GetGrouping(key, create: false) is not null;

	public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
	{
		var g = _lastGrouping;
		if (g is not null)
		{
			do
			{
				g = Debug.AssertNotNull(g._next);
				yield return g;
			}
			while (g != _lastGrouping);
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private int InternalGetHashCode(TKey key) =>
		// Handle comparer implementations that throw when passed null
		key is null ? 0 : _comparer.GetHashCode(key) & 0x7FFFFFFF;

	private Grouping? GetGrouping(TKey key, bool create)
	{
		var hashCode = InternalGetHashCode(key);
		for (var g = _groupings[hashCode % _groupings.Length]; g is not null; g = g._hashNext)
		{
			if (g.HashCode == hashCode && _comparer.Equals(g.Key, key))
				return g;
		}

		if (create)
		{
			if (Count == _groupings.Length)
				Resize();

			var index = hashCode % _groupings.Length;
			var g = new Grouping(key, hashCode)
			{
				_hashNext = _groupings[index],
			};

			_groupings[index] = g;
			if (_lastGrouping is null)
			{
				g._next = g;
			}
			else
			{
				g._next = _lastGrouping._next;
				_lastGrouping._next = g;
			}

			_lastGrouping = g;
			Count++;
			return g;
		}

		return null;
	}

	private void Resize()
	{
		var newSize = checked((Count * 2) + 1);
		var newGroupings = new Grouping[newSize];
		var g = Debug.AssertNotNull(_lastGrouping);
		do
		{
			g = Debug.AssertNotNull(g._next);
			var index = g.HashCode % newSize;
			g._hashNext = newGroupings[index];
			newGroupings[index] = g;
		}
		while (g != _lastGrouping);

		_groupings = newGroupings;
	}

	// Modified from:
	// https://github.com/dotnet/runtime/blob/v7.0.0/src/libraries/System.Linq/src/System/Linq/Grouping.cs#L48-L141
	[DebuggerDisplay("Key = {Key}")]
	[ExcludeFromCodeCoverage]
	private sealed class Grouping : IGrouping<TKey, TElement>, IList<TElement>
	{
		internal TElement[] _elements;
		internal int _count;
		internal Grouping? _hashNext;
		internal Grouping? _next;

		internal Grouping(TKey key, int hashCode)
		{
			Key = key;
			HashCode = hashCode;
			_elements = new TElement[1];
		}

		internal void Add(TElement element)
		{
			if (_elements.Length == _count)
				Array.Resize(ref _elements, checked(_count * 2));

			_elements[_count] = element;
			_count++;
		}

		internal void Trim()
		{
			if (_elements.Length != _count)
				Array.Resize(ref _elements, _count);
		}

		public IEnumerator<TElement> GetEnumerator()
		{
			for (var i = 0; i < _count; i++)
				yield return _elements[i];
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public TKey Key { get; }
		public int HashCode { get; }

		int ICollection<TElement>.Count => _count;

		bool ICollection<TElement>.IsReadOnly => true;

		bool ICollection<TElement>.Contains(TElement item) => Array.IndexOf(_elements, item, 0, _count) >= 0;

		void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex) =>
			Array.Copy(_elements, 0, array, arrayIndex, _count);

		int IList<TElement>.IndexOf(TElement item) => Array.IndexOf(_elements, item, 0, _count);

		TElement IList<TElement>.this[int index]
		{
			get
			{
				ArgumentOutOfRangeException.ThrowIfNegative(index);
				ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, _count);
				return _elements[index];
			}

			set => ThrowHelper.ThrowNotSupportedException();
		}

		void ICollection<TElement>.Add(TElement item) => ThrowHelper.ThrowNotSupportedException();
		void ICollection<TElement>.Clear() => ThrowHelper.ThrowNotSupportedException();
		bool ICollection<TElement>.Remove(TElement item) => ThrowHelper.ThrowNotSupportedException<bool>();
		void IList<TElement>.Insert(int index, TElement item) => ThrowHelper.ThrowNotSupportedException();
		void IList<TElement>.RemoveAt(int index) => ThrowHelper.ThrowNotSupportedException();
	}
}
