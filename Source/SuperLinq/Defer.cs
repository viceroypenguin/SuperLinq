using System.Collections;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Creates an enumerable sequence based on an enumerable factory function.
	/// </summary>
	/// <typeparam name="TResult">Result sequence element type.</typeparam>
	/// <param name="enumerableFactory">Enumerable factory function.</param>
	/// <returns>Sequence that will invoke the enumerable factory upon iteration.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="enumerableFactory"/> is <see
	/// langword="null"/>.</exception>
	public static IEnumerable<TResult> Defer<TResult>(Func<IEnumerable<TResult>> enumerableFactory)
	{
		Guard.IsNotNull(enumerableFactory);

		return new DeferEnumerable<TResult>(enumerableFactory);
	}

	private sealed class DeferEnumerable<T> : IEnumerable<T>
	{
		private readonly Func<IEnumerable<T>> _factory;

		public DeferEnumerable(Func<IEnumerable<T>> factory)
		{
			_factory = factory;
		}

		public IEnumerator<T> GetEnumerator() =>
			_factory().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
