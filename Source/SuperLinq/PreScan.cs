using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Performs a pre-scan (exclusive prefix sum) on a sequence of elements.
	/// </summary>
	/// <remarks>
	/// <para>
	/// An exclusive prefix sum returns an equal-length sequence where the
	/// N-th element is the sum of the first N-1 input elements (the first
	/// element is a special case, it is set to the identity). More
	/// generally, the pre-scan allows any commutative binary operation,
	/// not just a sum.
	/// </para>
	/// <para>
	/// The inclusive version of PreScan is <see cref="ScanEx{TSource}"/>.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams its result.
	/// </para>
	/// <example>
	/// <code><![CDATA[
	/// int[] values = { 1, 2, 3, 4 };
	/// var prescan = values.PreScan((a, b) => a + b, 0);
	/// var scan = values.Scan((a, b) => a + b);
	/// ]]></code>
	/// <c>prescan</c> will yield <c>{ 0, 1, 3, 6 }</c>, while <c>scan</c>
	/// will yield <c>{ 1, 3, 6, 10 }</c>. This shows the relationship 
	/// between the inclusive and exclusive prefix sum.
	/// </example>
	/// </remarks>
	/// <typeparam name="TSource">Type of elements in source sequence</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="transformation">Transformation operation</param>
	/// <param name="identity">Identity element (see remarks)</param>
	/// <returns>The scanned sequence</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="transformation"/> is null</exception>
	public static IEnumerable<TSource> PreScan<TSource>(
		this IEnumerable<TSource> source,
		Func<TSource, TSource, TSource> transformation,
		TSource identity)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(transformation);

		if (source is ICollection<TSource> coll)
			return new PreScanIterator<TSource>(coll, transformation, identity);

		return PreScanCore(source, transformation, identity);
	}

	private static IEnumerable<TSource> PreScanCore<TSource>(IEnumerable<TSource> source, Func<TSource, TSource, TSource> transformation, TSource identity)
	{
		var aggregator = identity;
		using var e = source.GetEnumerator();

		if (!e.MoveNext())
			yield break;

		yield return aggregator;
		var current = e.Current;

		while (e.MoveNext())
		{
			aggregator = transformation(aggregator, current);
			yield return aggregator;
			current = e.Current;
		}
	}

	private class PreScanIterator<T> : CollectionIterator<T>
	{
		private readonly ICollection<T> _source;
		private readonly Func<T, T, T> _transformation;
		private readonly T _identity;

		public PreScanIterator(ICollection<T> source, Func<T, T, T> transformation, T identity)
		{
			_source = source;
			_transformation = transformation;
			_identity = identity;
		}

		public override int Count => _source.Count;

		[ExcludeFromCodeCoverage]
		protected override IEnumerable<T> GetEnumerable() =>
			PreScanCore(_source, _transformation, _identity);

		public override void CopyTo(T[] array, int arrayIndex)
		{
			var (sList, b, cnt) = _source is IList<T> s
				? (s, 0, s.Count)
				: (array, arrayIndex, SuperEnumerable.CopyTo(_source, array, arrayIndex));

			var i = 0;
			var state = _identity;

			for (; i < cnt - 1; i++)
			{
				var nextState = sList[b + i];
				array[arrayIndex + i] = state;
				state = _transformation(state, nextState);
			}

			array[arrayIndex + cnt - 1] = state;
		}
	}
}
