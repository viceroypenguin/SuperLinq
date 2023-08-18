// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Generates a sequence of non-overlapping adjacent buffers over the source sequence.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	///	</typeparam>
	/// <param name="source">
	///	    Source sequence.
	///	</param>
	/// <param name="count">
	///	    Number of elements for allocated buffers.
	///	</param>
	/// <returns>
	///	    Sequence of buffers containing source sequence elements.
	///	</returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	///	</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than or equal to <c>0</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    A chunk can contain fewer elements than <paramref name="count"/>, specifically the final buffer of <paramref
	///     name="source"/>.
	/// </para>
	/// <para>
	///	    This method is a synonym for <see cref="Batch{TSource}(IEnumerable{TSource}, int)"/>.
	/// </para>
	/// <para>
	///	    Returned subsequences are buffered, but the overall operation is streamed.<br/>
	/// </para>
	/// </remarks>
	public static IEnumerable<IList<TSource>> Buffer<TSource>(this IEnumerable<TSource> source, int count)
	{
		return Batch(source, count);
	}

	/// <summary>
	///	    Generates a sequence of buffers over the source sequence, with specified length and possible overlap.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="count">
	///	    Number of elements for allocated buffers.
	/// </param>
	/// <param name="skip">
	///	    Number of elements to skip between the start of consecutive buffers.
	/// </param>
	/// <returns>
	///	    Sequence of buffers containing source sequence elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> or <paramref name="skip"/> is less than or equal to <c>0</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    A chunk can contain fewer elements than <paramref name="count"/>, specifically the final buffer(s) of
	///     <paramref name="source"/>.
	/// </para>
	/// <para>
	///	    Returned subsequences are buffered, but the overall operation is streamed.<br/>
	/// </para>
	/// </remarks>
	public static IEnumerable<IList<TSource>> Buffer<TSource>(this IEnumerable<TSource> source, int count, int skip)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThan(count, 0);
		Guard.IsGreaterThan(skip, 0);

		return Core(source, count, skip);

		static IEnumerable<IList<TSource>> Core(IEnumerable<TSource> source, int count, int skip)
		{
			var lists = new Queue<List<TSource>>();

			var i = 0;
			foreach (var el in source)
			{
				if (i++ % skip == 0)
					lists.Enqueue(new());

				foreach (var l in lists)
					l.Add(el);

				if (lists.Count > 0 && lists.Peek().Count == count)
					yield return lists.Dequeue();
			}

			while (lists.Count > 0)
				yield return lists.Dequeue();
		}
	}
}
