namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Returns an infinite sequence of random integers.
	/// </summary>
	/// <returns>
	///	    An infinite sequence of random integers
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	/// </para>
	/// </remarks>
	public static IEnumerable<int> Random()
	{
		return Random(s_randomInstance);
	}

	/// <summary>
	///	    Returns an infinite sequence of random integers using the supplied random number generator.
	/// </summary>
	/// <param name="rand">
	///	    Random generator used to produce random numbers
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="rand"/> is <see langword="null"/>.
	/// </exception>
	/// <returns>
	///	    An infinite sequence of random integers
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	/// </para>
	/// </remarks>
	public static IEnumerable<int> Random(Random rand)
	{
		Guard.IsNotNull(rand);

		return RandomImpl(rand, r => r.Next());
	}

	/// <summary>
	///	    Returns an infinite sequence of random integers between zero and a given maximum.
	/// </summary>
	/// <param name="maxValue">
	///		Exclusive upper bound for random values returned.
	/// </param>
	/// <returns>
	///	    An infinite sequence of random integers
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	/// </para>
	/// </remarks>
	public static IEnumerable<int> Random(int maxValue)
	{
		Guard.IsGreaterThanOrEqualTo(maxValue, 0);

		return Random(s_randomInstance, maxValue);
	}

	/// <summary>
	///	    Returns an infinite sequence of random integers between zero and a given maximum using the supplied random
	///     number generator.
	/// </summary>
	/// <param name="rand">
	///	    Random generator used to produce values
	/// </param>
	/// <param name="maxValue">
	///	    Exclusive upper bound for random values returned
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="rand"/> is <see langword="null"/>.
	/// </exception>
	/// <returns>
	///	    An infinite sequence of random integers
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	/// </para>
	/// </remarks>
	public static IEnumerable<int> Random(Random rand, int maxValue)
	{
		Guard.IsNotNull(rand);
		Guard.IsGreaterThanOrEqualTo(maxValue, 0);

		return RandomImpl(rand, r => r.Next(maxValue));
	}

	/// <summary>
	///	    Returns an infinite sequence of random integers between a given minimum and maximum.
	/// </summary>
	/// <param name="minValue">
	///		Inclusive lower bound for random values returned.
	/// </param>
	/// <param name="maxValue">
	///		Exclusive upper bound for random values returned.
	/// </param>
	/// <exception cref="ArgumentOutOfRangeException">
	///		<paramref name="minValue"/> is greater than <paramref name="maxValue"/>.
	/// </exception>
	/// <returns>
	///	    An infinite sequence of random integers
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	/// </para>
	/// </remarks>
	public static IEnumerable<int> Random(int minValue, int maxValue)
	{
		return Random(s_randomInstance, minValue, maxValue);
	}

	/// <summary>
	///	    Returns an infinite sequence of random integers between zero and a given maximum using the supplied random
	///     number generator.
	/// </summary>
	/// <param name="rand">
	///	    Random generator used to produce values
	/// </param>
	/// <param name="minValue">
	///		Inclusive lower bound for random values returned.
	/// </param>
	/// <param name="maxValue">
	///	    Exclusive upper bound for random values returned
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="rand"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///		<paramref name="minValue"/> is greater than <paramref name="maxValue"/>.
	/// </exception>
	/// <returns>
	///	    An infinite sequence of random integers
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	/// </para>
	/// </remarks>
	public static IEnumerable<int> Random(Random rand, int minValue, int maxValue)
	{
		Guard.IsNotNull(rand);
		Guard.IsLessThanOrEqualTo(minValue, maxValue);

		return RandomImpl(rand, r => r.Next(minValue, maxValue));
	}

	/// <summary>
	///	    Returns an infinite sequence of random double values between 0.0 and 1.0.
	/// </summary>
	/// <returns>
	///	    An infinite sequence of random doubles
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	/// </para>
	/// </remarks>
	public static IEnumerable<double> RandomDouble()
	{
		return RandomDouble(s_randomInstance);
	}

	/// <summary>
	///	    Returns an infinite sequence of random double values between 0.0 and 1.0 using the supplied random number generator.
	/// </summary>
	/// <param name="rand">
	///	    Random generator used to produce values
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="rand"/> is <see langword="null"/>.
	/// </exception>
	/// <returns>
	///	    An infinite sequence of random doubles
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	/// </para>
	/// </remarks>
	public static IEnumerable<double> RandomDouble(Random rand)
	{
		Guard.IsNotNull(rand);

		return RandomImpl(rand, r => r.NextDouble());
	}

	/// <summary>
	/// This is the underlying implementation that all random operators use to
	/// produce a sequence of random values.
	/// </summary>
	/// <typeparam name="T">The type of value returned (either Int32 or Double)</typeparam>
	/// <param name="rand">Random generators used to produce the sequence</param>
	/// <param name="nextValue">Generator function that actually produces the next value - specific to T</param>
	/// <returns>An infinite sequence of random numbers of type T</returns>
	private static IEnumerable<T> RandomImpl<T>(Random rand, Func<Random, T> nextValue)
	{
		while (true)
			yield return nextValue(rand);
	}

	private static readonly Random s_randomInstance =
#if NET6_0_OR_GREATER
		System.Random.Shared;
#else
		new GlobalRandom();

	/// <remarks>
	///	    <see cref="System.Random"/> is not thread-safe so the following implementation uses thread-local <see
	///     cref="System.Random"/> instances to create the illusion of a global <see cref="System.Random"/>
	///     implementation. For some background, see <a
	///     href="https://blogs.msdn.microsoft.com/pfxteam/2009/02/19/getting-random-numbers-in-a-thread-safe-way/">Getting
	///     random numbers in a thread-safe way</a>
	/// </remarks>
	private sealed class GlobalRandom : Random
	{
		private static int s_seed = Environment.TickCount;

		[ThreadStatic] private static Random? s_threadRandom;

		private static Random ThreadRandom => s_threadRandom ??= new Random(Interlocked.Increment(ref s_seed));

		public override int Next() => ThreadRandom.Next();
		public override int Next(int minValue, int maxValue) => ThreadRandom.Next(minValue, maxValue);
		public override int Next(int maxValue) => ThreadRandom.Next(maxValue);
		public override double NextDouble() => ThreadRandom.NextDouble();
		public override void NextBytes(byte[] buffer) => ThreadRandom.NextBytes(buffer);

		protected override double Sample()
		{
			// All the NextXXX calls are hijacked above to use the Random
			// instance allocated for the thread so no call from the base
			// class should ever end up here. If Random introduces new
			// virtual members in the future that call into Sample and
			// which end up getting used in the implementation of a
			// randomizing operator from the outer class then they will
			// need to be overriden.

			throw new NotSupportedException("Should never reach this code.");
		}
	}
#endif
}
