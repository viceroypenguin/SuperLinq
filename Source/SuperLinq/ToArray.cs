namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Creates an array from <see cref="IEnumerable{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of list elements.</typeparam>
	/// <param name="source">The <see cref="IEnumerable{T}"/> to create array" /> from.</param >
	/// <param name="length">Expected length of source.</param>
	/// <returns>
	/// An array that contains elements from the input sequence
	/// </returns>
	public static T[] ToArray<T>(this IEnumerable<T> source, int length)
	{
		var resultArray = new T[length];
		var i = 0;
		foreach (var item in source)
		{
			if (i >= resultArray.Length)
			{
				resultArray = CreateTwiceBiggerArray(resultArray);
			}
			resultArray[i++] = item;
		}
		return i < resultArray.Length
			? resultArray.TrimArray(i) : resultArray;
	}

	private static T[] CreateTwiceBiggerArray<T>(T[] array)
	{
		var resultArray = new T[array.Length * 2];
		for (var i = 0; i < array.Length; i++)
		{
			resultArray[i] = array[i];
		}
		return resultArray;
	}

	private static T[] TrimArray<T>(this T[] array, int length)
	{
		var resultArray = new T[length];
		Array.Copy(array, resultArray, length);
		return resultArray;
	}
}
