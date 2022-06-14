using System.Collections;

namespace Test;

static class ReadOnlyCollection
{
	public static IReadOnlyCollection<T> From<T>(params T[] items) =>
		new ListCollection<T[], T>(items);

	sealed class ListCollection<TList, T> : IReadOnlyCollection<T>
		where TList : IList<T>
	{
		readonly TList _list;

		public ListCollection(TList list) => _list = list;

		public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int Count => _list.Count;
	}
}
