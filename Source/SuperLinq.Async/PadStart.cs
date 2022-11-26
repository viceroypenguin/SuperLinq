namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Pads a sequence with default values in the beginning if it is narrower (shorter
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
	/// var result = numbers.PadStart(5);
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>{ 0, 0, 123, 456, 789 }</c>.
	/// </example>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> is less than 0.</exception>
	public static IAsyncEnumerable<TSource?> PadStart<TSource>(this IAsyncEnumerable<TSource> source, int width)
	{
		return PadStart(source, width, default(TSource));
	}

	/// <summary>
	/// Pads a sequence with a given filler value in the beginning if it is narrower (shorter
	/// in length) than a given width.
	/// An additional parameter specifies the value to use for padding.
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
	/// var result = numbers.PadStart(5, -1);
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>{ -1, -1, 123, 456, 789 }</c>.
	/// </example>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> is less than 0.</exception>
	public static IAsyncEnumerable<TSource> PadStart<TSource>(this IAsyncEnumerable<TSource> source, int width, TSource padding)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(width, 0);
		return PadStartImpl(source, width, padding, paddingSelector: null);
	}

	/// <summary>
	/// Pads a sequence with a dynamic filler value in the beginning if it is narrower (shorter
	/// in length) than a given width.
	/// An additional parameter specifies the function to calculate padding.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">The sequence to pad.</param>
	/// <param name="width">The width/length below which to pad.</param>
	/// <param name="paddingSelector">
	/// Function to calculate padding given the index of the missing element.
	/// </param>
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
	/// var result = numbers.PadStart(6, i => -i);
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>{ 0, -1, -2, 123, 456, 789 }</c>.
	/// </example>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="paddingSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> is less than 0.</exception>
	public static IAsyncEnumerable<TSource> PadStart<TSource>(this IAsyncEnumerable<TSource> source, int width, Func<int, TSource> paddingSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(paddingSelector);
		Guard.IsGreaterThanOrEqualTo(width, 0);
		return PadStartImpl(source, width, padding: default, paddingSelector);
	}

	private static async IAsyncEnumerable<T> PadStartImpl<T>(
		IAsyncEnumerable<T> source, int width,
		T? padding, Func<int, T>? paddingSelector,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var array = new T[width];
		var count = 0;

		await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken))
		{
			for (; count < width && await e.MoveNextAsync(); count++)
				array[count] = e.Current;

			if (count == width)
			{
				for (var i = 0; i < count; i++)
					yield return array[i];

				while (await e.MoveNextAsync())
					yield return e.Current;

				yield break;
			}
		}

		var len = width - count;

		for (var i = 0; i < len; i++)
			yield return paddingSelector != null 
				? paddingSelector(i) 
				: Debug.AssertNotNull(padding);

		for (var i = 0; i < count; i++)
			yield return array[i];
	}
}
