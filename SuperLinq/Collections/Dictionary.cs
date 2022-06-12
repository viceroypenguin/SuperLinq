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
	readonly System.Collections.Generic.Dictionary<TKey, TValue> _dict;
	(bool, TValue) _null;

	public Dictionary(IEqualityComparer<TKey> comparer)
	{
		_dict = new System.Collections.Generic.Dictionary<TKey, TValue>(comparer);
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
