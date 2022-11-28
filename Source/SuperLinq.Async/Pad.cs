namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Pads a sequence with default values if it is narrower (shorter
	/// in length) than a given width.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">The sequence to pad.</param>
	/// <param name="width">The width/length below which to pad.</param>
	/// <returns>
	/// Returns a sequence that is at least as wide/long as the width/length
	/// specified by the <paramref name="width"/> parameter.
	/// </returns>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// int[] numbers = { 123, 456, 789 };
	/// var result = numbers.Pad(5);
	/// ]]></code>
	/// The <c>result</c> variable, when iterated over, will yield
	/// 123, 456, 789 and two zeroes, in turn.
	/// </example>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> is less than 0.</exception>
	public static IAsyncEnumerable<TSource?> Pad<TSource>(this IAsyncEnumerable<TSource> source, int width)
	{
		return Pad(source, width, padding: default);
	}

	/// <summary>
	/// Pads a sequence with a given filler value if it is narrower (shorter
	/// in length) than a given width.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">The sequence to pad.</param>
	/// <param name="width">The width/length below which to pad.</param>
	/// <param name="padding">The value to use for padding.</param>
	/// <returns>
	/// Returns a sequence that is at least as wide/long as the width/length
	/// specified by the <paramref name="width"/> parameter.
	/// </returns>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// int[] numbers = { 123, 456, 789 };
	/// var result = numbers.Pad(5, -1);
	/// ]]></code>
	/// The <c>result</c> variable, when iterated over, will yield
	/// 123, 456, and 789 followed by two occurrences of -1, in turn.
	/// </example>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> is less than 0.</exception>
	public static IAsyncEnumerable<TSource> Pad<TSource>(this IAsyncEnumerable<TSource> source, int width, TSource padding)
	{
		return Pad(source, width, paddingSelector: _ => padding);
	}

	/// <summary>
	/// Pads a sequence with a dynamic filler value if it is narrower (shorter
	/// in length) than a given width.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">The sequence to pad.</param>
	/// <param name="width">The width/length below which to pad.</param>
	/// <param name="paddingSelector">Function to calculate padding.</param>
	/// <returns>
	/// Returns a sequence that is at least as wide/long as the width/length
	/// specified by the <paramref name="width"/> parameter.
	/// </returns>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// int[] numbers = { 0, 1, 2 };
	/// var result = numbers.Pad(5, i => -i);
	/// ]]></code>
	/// The <c>result</c> variable, when iterated over, will yield
	/// 0, 1, 2, -3 and -4, in turn.
	/// </example>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="paddingSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> is less than 0.</exception>
	public static IAsyncEnumerable<TSource> Pad<TSource>(this IAsyncEnumerable<TSource> source, int width, Func<int, TSource> paddingSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(paddingSelector);
		Guard.IsGreaterThanOrEqualTo(width, 0);

		return _(source, width, paddingSelector);

		static async IAsyncEnumerable<TSource> _(
			IAsyncEnumerable<TSource> source, int width,
			Func<int, TSource> paddingSelector,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var count = 0;
			await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				yield return item;
				count++;
			}

			while (count < width)
			{
				yield return paddingSelector(count);
				count++;
			}
		}
	}
}
