namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Creates a <see cref="List{T}" /> from <see cref="IEnumerable{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of list elements.</typeparam>
	/// <param name="source">The <see cref="IEnumerable{T}"/> to create <see cref="List{T}" /> from.</param >
	/// <param name="length">Expected length of source.</param>
	/// <returns>
	/// A <see cref="List{T}"/> that contains elements from the input sequence
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is <see langword="null"/>.</exception>
	public static List<T> ToList<T>(this IEnumerable<T> source, int length)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(length, 0);

		var resultList = new List<T>(length);
		resultList.AddRange(source);
		return resultList;
	}
}
