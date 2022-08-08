using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace SuperLinq;

internal static class Cartesian
{
	public static SourceText Generate()
	{
		var sb = new StringBuilder();
		sb.Append(@"
namespace SuperLinq;

public static partial class SuperEnumerable
{");

		var args = new[]
		{
			(ordinal: "first"  , arity: "one"   ),
			(ordinal: "second" , arity: "two"   ),
			(ordinal: "third"  , arity: "three" ),
			(ordinal: "fourth" , arity: "four"  ),
			(ordinal: "fifth"  , arity: "five"  ),
			(ordinal: "sixth"  , arity: "six"   ),
			(ordinal: "seventh", arity: "seven" ),
			(ordinal: "eighth" , arity: "eight" ),
		};

		for (var i = 2; i <= args.Length; i++)
		{
			void ForEachArgument(Func<int, string, string> builder)
			{
				for (var j = 0; j < i; j++)
					sb.Append(builder(j + 1, args[j].ordinal));
			}

			string BuildArgumentString(Func<int, string, string> builder) =>
				string.Join(", ", Enumerable.Range(0, i).Select(j => builder(j + 1, args[j].ordinal)));

			var (ordinal, arity) = args[i - 1];

			sb.Append($@"
    /// <summary>
    /// Returns the Cartesian product of {arity} sequences by enumerating all
    /// possible combinations of one item from each sequence, and applying
    /// a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name=""TResult"">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name=""resultSelector"">A projection function that combines
    /// elements from all of the sequences.</param>
    /// <returns>A sequence of elements returned by
    /// <paramref name=""resultSelector""/>.</returns>
    /// <remarks>
    /// <para>
    /// The method returns items in the same order as a nested foreach
    /// loop, but all sequences except for <paramref name=""first""/> are
    /// cached when iterated over. The cache is then re-used for any
    /// subsequent iterations.</para>
    /// <para>
    /// This method uses deferred execution and stream its results.</para>
    /// </remarks>");

			ForEachArgument((j, o) => $@"
    /// <typeparam name=""T{j}"">
    /// The type of the elements of <paramref name=""{o}""/>.</typeparam>
    /// <param name=""{o}"">The {o} sequence of elements.</param>");

			sb.Append($@"
    public static IEnumerable<TResult> Cartesian<{BuildArgumentString((j, _) => $"T{j}")}, TResult>(this ");

			ForEachArgument((j, o) => $@"
		IEnumerable<T{j}> {o},");

			sb.Append($@"
		Func<{BuildArgumentString((j, _) => $"T{j}")}, TResult> resultSelector)
    {{");

			ForEachArgument((j, o) => $@"
        Guard.IsNotNull({o});");

			sb.Append($@"
		Guard.IsNotNull(resultSelector);

        return _({BuildArgumentString((_, o) => o)}, resultSelector);

		static IEnumerable<TResult> _(");

			ForEachArgument((j, o) => $@"
			IEnumerable<T{j}> {o},");

			sb.Append($@"
			Func<{BuildArgumentString((j, _) => $"T{j}")}, TResult> resultSelector)
		{{");

			ForEachArgument((j, o) => $@"
            using var {o}Memo = {o}.Memoize();");

			ForEachArgument((j, o) => $@"
            foreach (var item{j} in {o}Memo)");

			sb.Append($@"
                yield return resultSelector({BuildArgumentString((j, _) => $"item{j}")});
		}}
    }}");
		}

		sb.Append($@"
}}");

		return SourceText.From(sb.ToString(), Encoding.UTF8);
	}
}
