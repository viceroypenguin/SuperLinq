namespace SuperLinq;

/// <summary>
/// A utility class to easily compose a custom <see cref="IEqualityComparer{T}"/>
/// for <see cref="ValueTuple{T1}"/>s and <see cref="ValueTuple{T1, T2}"/>s.
/// </summary>
public static class ValueTupleEqualityComparer
{
	/// <summary>
	/// Creates a custom <see cref="IEqualityComparer{T}"/> for a <see cref="ValueTuple{T1}"/> based on custom comparers
	/// for each component.
	/// </summary>
	/// <typeparam name="T1">The type of the first element of the <see cref="ValueTuple{T1}"/></typeparam>
	/// <param name="comparer1">The custom comparer for <typeparamref name="T1"/>. If <see langword="null"/>, then <see
	/// cref="EqualityComparer{T}.Default" /> will be used.</param>
	/// <returns>An <see cref="IEqualityComparer{T}"/> that can be used to compare two <see cref="ValueTuple{T1}"/>
	/// using the provided comparers for each component.</returns>
	public static IEqualityComparer<ValueTuple<T1>> Create<T1>(
			IEqualityComparer<T1>? comparer1) =>
		comparer1 is null
			? EqualityComparer<ValueTuple<T1>>.Default
			: new ItemEqualityComparer<T1>(comparer1);

	private sealed class ItemEqualityComparer<T1> : IEqualityComparer<ValueTuple<T1>>
	{
		private readonly IEqualityComparer<T1> _comparer1;

		public ItemEqualityComparer(
			IEqualityComparer<T1>? comparer1)
		{
			_comparer1 = comparer1 ?? EqualityComparer<T1>.Default;
		}

		public bool Equals(ValueTuple<T1> x,
						   ValueTuple<T1> y)
			=> _comparer1.Equals(x.Item1, y.Item1);

		public int GetHashCode(ValueTuple<T1> obj) =>
			obj.Item1 is null ? 0 : _comparer1.GetHashCode(obj.Item1);
	}

	/// <summary>
	/// Creates a custom <see cref="IEqualityComparer{T}"/> for a <see cref="ValueTuple{T1, T2}"/> based on custom
	/// comparers for each component.
	/// </summary>
	/// <typeparam name="T1">The type of the first element of the <see cref="ValueTuple{T1, T2}"/></typeparam>
	/// <typeparam name="T2">The type of the second element of the <see cref="ValueTuple{T1, T2}"/></typeparam>
	/// <param name="comparer1">The custom comparer for <typeparamref name="T1"/>. If <see langword="null"/>, then <see
	/// cref="EqualityComparer{T}.Default" /> will be used.</param>
	/// <param name="comparer2">The custom comparer for <typeparamref name="T2"/>. If <see langword="null"/>, then <see
	/// cref="EqualityComparer{T}.Default" /> will be used.</param>
	/// <returns>An <see cref="IEqualityComparer{T}"/> that can be used to compare two <see cref="ValueTuple{T1, T2}"/>
	/// using the provided comparers for each component.</returns>
	public static IEqualityComparer<(T1, T2)> Create<T1, T2>(
			IEqualityComparer<T1>? comparer1,
			IEqualityComparer<T2>? comparer2) =>
		new ItemEqualityComparer<T1, T2>(
			comparer1,
			comparer2);

	private sealed class ItemEqualityComparer<T1, T2> : IEqualityComparer<(T1, T2)>
	{
		private readonly IEqualityComparer<T1> _comparer1;
		private readonly IEqualityComparer<T2> _comparer2;

		public ItemEqualityComparer(
			IEqualityComparer<T1>? comparer1,
			IEqualityComparer<T2>? comparer2)
		{
			_comparer1 = comparer1 ?? EqualityComparer<T1>.Default;
			_comparer2 = comparer2 ?? EqualityComparer<T2>.Default;
		}

		public bool Equals((T1, T2) x,
						   (T1, T2) y)
			=> _comparer1.Equals(x.Item1, y.Item1)
			&& _comparer2.Equals(x.Item2, y.Item2);

		public int GetHashCode((T1, T2) obj) =>
			HashCode.Combine(
				_comparer1.GetHashCode(obj.Item1!),
				_comparer2.GetHashCode(obj.Item2!));
	}
}
