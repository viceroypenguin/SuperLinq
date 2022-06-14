using System.Diagnostics.CodeAnalysis;

namespace SuperLinq.Collections;

/// <summary>
/// A minimal <see cref="System.Collections.Generic.Dictionary{TKey,TValue}"/> wrapper that
/// allows null keys when <typeparamref name="TKey"/> is a
/// reference type.
/// </summary>
// Add members if and when needed to keep coverage.

sealed class Dictionary<TKey, TValue>
{
#pragma warning disable CS8714 // listen, we promise we're not going to stick a null in this dictionary...
	private readonly System.Collections.Generic.Dictionary<TKey, TValue> _dict;
#pragma warning restore CS8714
	private (bool, TValue) _null;

	public Dictionary(IEqualityComparer<TKey> comparer)
	{
		_dict = new(comparer);
		_null = default;
	}

	public TValue this[TKey key]
	{
		set
		{
			if (key is null)
				_null = (true, value);
			else
				_dict[key] = value;
		}
	}

	public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
	{
		if (key is null)
		{
			switch (_null)
			{
				case (true, var v):
					value = v;
					return true;
				case (false, _):
					value = default;
					return false;
			}
		}

		return _dict.TryGetValue(key, out value);
	}
}
