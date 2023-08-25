using System.Diagnostics.CodeAnalysis;

namespace SuperLinq.Collections;

/// <summary>
/// A minimal <see cref="Dictionary{TKey,TValue}"/> wrapper that
/// allows null keys when <typeparamref name="TKey"/> is a
/// reference type.
/// </summary>
internal sealed class NullKeyDictionary<TKey, TValue> : Dictionary<ValueTuple<TKey>, TValue>
{
	public NullKeyDictionary(IEqualityComparer<TKey>? comparer)
		: base(comparer: ValueTupleEqualityComparer.Create(comparer))
	{ }

	public NullKeyDictionary(int count)
		: base(count, comparer: ValueTupleEqualityComparer.Create<TKey>(default))
	{ }

	public TValue this[TKey key]
	{
		get => this[ValueTuple.Create(key)];
		set => this[ValueTuple.Create(key)] = value;
	}

	public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
		=> this.TryGetValue(ValueTuple.Create(key), out value);

	public bool Remove(TKey key)
		=> this.Remove(ValueTuple.Create(key));
}
