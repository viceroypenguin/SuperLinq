#if !NETCOREAPP

namespace Test;

internal static class Future
{
	public static IEnumerable<(TFirst First, TSecond Scond)> Zip<TFirst, TSecond>(this IEnumerable<TFirst> first,
		IEnumerable<TSecond> second)
		=> first.Zip(second, (first, second) => (first, second));
}

#endif
