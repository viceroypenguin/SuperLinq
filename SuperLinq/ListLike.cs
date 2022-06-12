namespace SuperLinq;

/// <summary>
/// Represents an list-like (indexable) data structure.
/// </summary>
interface IListLike<out T>
{
	int Count { get; }
	T this[int index] { get; }
}

static class ListLike
{
	public static IListLike<T> ToListLike<T>(this IEnumerable<T> source)
		=> source.TryAsListLike() ?? new List<T>(source.ToList());

	public static IListLike<T>? TryAsListLike<T>(this IEnumerable<T> source) =>
		source switch
		{
			null => throw new ArgumentNullException(nameof(source)),
			IList<T> list => new List<T>(list),
			IReadOnlyList<T> list => new ReadOnlyList<T>(list),
			_ => null
		};

	sealed class List<T> : IListLike<T>
	{
		readonly IList<T> _list;
		public List(IList<T> list) => _list = list ?? throw new ArgumentNullException(nameof(list));
		public int Count => _list.Count;
		public T this[int index] => _list[index];
	}

	sealed class ReadOnlyList<T> : IListLike<T>
	{
		readonly IReadOnlyList<T> _list;
		public ReadOnlyList(IReadOnlyList<T> list) => _list = list ?? throw new ArgumentNullException(nameof(list));
		public int Count => _list.Count;
		public T this[int index] => _list[index];
	}
}
