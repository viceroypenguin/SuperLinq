namespace SuperLinq
{
    public static partial class SuperEnumerable
    {
        /// <summary>
        /// Returns a sequence of a specified size of random elements from the
        /// original sequence.
        /// </summary>
        /// <typeparam name="T">The type of source sequence elements.</typeparam>
        /// <param name="source">
        /// The sequence from which to return random elements.</param>
        /// <param name="subsetSize">The size of the random subset to return.</param>
        /// <returns>
        /// A random sequence of elements in random order from the original
        /// sequence.</returns>

        public static IEnumerable<T> RandomSubset<T>(this IEnumerable<T> source, int subsetSize)
        {
            return RandomSubset(source, subsetSize, new Random());
        }

        /// <summary>
        /// Returns a sequence of a specified size of random elements from the
        /// original sequence. An additional parameter specifies a random
        /// generator to be used for the random selection algorithm.
        /// </summary>
        /// <typeparam name="T">The type of source sequence elements.</typeparam>
        /// <param name="source">
        /// The sequence from which to return random elements.</param>
        /// <param name="subsetSize">The size of the random subset to return.</param>
        /// <param name="rand">
        /// A random generator used as part of the selection algorithm.</param>
        /// <returns>
        /// A random sequence of elements in random order from the original
        /// sequence.</returns>

        public static IEnumerable<T> RandomSubset<T>(this IEnumerable<T> source, int subsetSize, Random rand)
        {
            Guard.IsNotNull(rand);
            Guard.IsNotNull(source);
            Guard.IsGreaterThanOrEqualTo(subsetSize, 0);

            return RandomSubsetImpl(source, rand, subsetSize);
        }

#pragma warning disable MA0050 // arguments validated in both callers
// this line below should fix this warning/error

        private static IEnumerable<T> RandomSubsetImpl<T>(IEnumerable<T> source, Random rand, int? subsetLength)
  #pragma warning restore MA0050      
        {
            		// The simplest and most efficient way to return a random subset is to perform
		// an in-place, partial Fisher-Yates shuffle of the sequence. While we could do
		// a full shuffle, it would be wasteful in the cases where subsetSize is shorter
		// than the length of the sequence.
		// See: http://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle

		// this block that follows should fix the error

            var array = source.ToArray(); // Move .ToArray() inside RandomSubsetImpl

            if (!subsetLength.HasValue)
            {
                subsetLength = array.Length;
            }

            if (array.Length < subsetLength || subsetLength < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(
                    nameof(subsetLength),
                    "Subset length must be non-negative and not exceed the source length.");
            }

            var m = 0;                // keeps track of count items shuffled
            var w = array.Length;     // upper bound of shrinking swap range
            var g = w - 1;            // used to compute the second swap index

            // perform in-place, partial Fisher-Yates shuffle
            while (m < subsetLength)
            {
                var k = g - rand.Next(w);
                (array[m], array[k]) = (array[k], array[m]);
                ++m;
                --w;
            }

            // yield the random subset as a new sequence
            for (var i = 0; i < subsetLength.Value; i++)
                yield return array[i];
        }
    }
}
