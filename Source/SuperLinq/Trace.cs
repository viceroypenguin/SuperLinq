namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Traces the elements of a source sequence for diagnostics.
	/// </summary>
	/// <typeparam name="TSource">Type of element in the source sequence</typeparam>
	/// <param name="source">Source sequence whose elements to trace.</param>
	/// <returns>
	/// Return the source sequence unmodified.
	/// </returns>
	/// <remarks>
	/// This a pass-through operator that uses deferred execution and
	/// streams the results.
	/// </remarks>

	public static IEnumerable<TSource> Trace<TSource>(this IEnumerable<TSource> source)
	{
		return Trace(source, (string?)null);
	}

	/// <summary>
	/// Traces the elements of a source sequence for diagnostics using
	/// custom formatting.
	/// </summary>
	/// <typeparam name="TSource">Type of element in the source sequence</typeparam>
	/// <param name="source">Source sequence whose elements to trace.</param>
	/// <param name="format">
	/// String to use to format the trace message. If null then the
	/// element value becomes the traced message.
	/// </param>
	/// <returns>
	/// Return the source sequence unmodified.
	/// </returns>
	/// <remarks>
	/// This a pass-through operator that uses deferred execution and
	/// streams the results.
	/// </remarks>

	public static IEnumerable<TSource> Trace<TSource>(this IEnumerable<TSource> source, string? format)
	{
		Guard.IsNotNull(source);

		return TraceImpl(source,
			string.IsNullOrEmpty(format)
			? (x => x?.ToString() ?? string.Empty)
			: (x => string.Format(provider: default, format, x)));
	}

	/// <summary>
	/// Traces the elements of a source sequence for diagnostics using
	/// a custom formatter.
	/// </summary>
	/// <typeparam name="TSource">Type of element in the source sequence</typeparam>
	/// <param name="source">Source sequence whose elements to trace.</param>
	/// <param name="formatter">Function used to format each source element into a string.</param>
	/// <returns>
	/// Return the source sequence unmodified.
	/// </returns>
	/// <remarks>
	/// This a pass-through operator that uses deferred execution and
	/// streams the results.
	/// </remarks>

	public static IEnumerable<TSource> Trace<TSource>(this IEnumerable<TSource> source, Func<TSource, string> formatter)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(formatter);
		return TraceImpl(source, formatter);
	}

	static IEnumerable<TSource> TraceImpl<TSource>(IEnumerable<TSource> source, Func<TSource, string> formatter)
	{
		return source
			.Pipe(x => System.Diagnostics.Trace.WriteLine(formatter(x)));
	}
}
