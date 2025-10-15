using System.Collections;

namespace SuperLinq.Tests;

internal static class ReadOnlyCollection
{
	public static IReadOnlyCollection<T> From<T>(params T[] items) =>
		new ListCollection<T[], T>(items);

	private sealed class ListCollection<TList, T>(
		TList list
	) : IReadOnlyCollection<T>
		where TList : IList<T>
	{
		private readonly TList _list = list;

		public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int Count => _list.Count;
	}
}
