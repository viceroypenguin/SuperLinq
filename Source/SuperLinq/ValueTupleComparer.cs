using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

/// <summary>
/// A utility class to easily compose a custom <see cref="IComparer{T}"/>
/// for <see cref="ValueTuple{T1, T2}"/>s.
/// </summary>
internal static class ValueTupleComparer
{
	/// <summary>
	/// Creates a custom <see cref="IComparer{T}"/> for a <see cref="ValueTuple{T1, T2}"/>
	/// based on custom comparers for each component.
	/// </summary>
	/// <typeparam name="T1">The type of the first element of the <see cref="ValueTuple{T1, T2}"/></typeparam>
	/// <typeparam name="T2">The type of the second element of the <see cref="ValueTuple{T1, T2}"/></typeparam>
	/// <param name="comparer1">The custom comparer for <typeparamref name="T1"/>. If <see langword="null"/>, then <see cref="Comparer{T}.Default" /> will be used.</param>
	/// <param name="comparer2">The custom comparer for <typeparamref name="T2"/>. If <see langword="null"/>, then <see cref="Comparer{T}.Default" /> will be used.</param>
	/// <returns>An <see cref="IComparer{T}"/> that can be used to compare two <see cref="ValueTuple{T1, T2}"/> using the provided comparers for each component.</returns>
	public static IComparer<(T1, T2)> Create<T1, T2>(
			IComparer<T1>? comparer1,
			IComparer<T2>? comparer2) =>
		new Comparer<T1, T2>(
			comparer1,
			comparer2);

	private sealed class Comparer<T1, T2> : IComparer<(T1, T2)>
	{
		private readonly IComparer<T1> _comparer1;
		private readonly IComparer<T2> _comparer2;

		public Comparer(
			IComparer<T1>? comparer1,
			IComparer<T2>? comparer2)
		{
			_comparer1 = comparer1 ?? Comparer<T1>.Default;
			_comparer2 = comparer2 ?? Comparer<T2>.Default;
		}

		public int Compare([AllowNull] (T1, T2) x, [AllowNull] (T1, T2) y)
		{
			var result = _comparer1.Compare(x.Item1, y.Item1);
			return result != 0 ? result :
				_comparer2.Compare(x.Item2, y.Item2);
		}
	}
}
