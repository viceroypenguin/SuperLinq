using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace SuperLinq;

internal static class Aggregate
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

		void ForEachArgument(int i, Func<int, string, string> builder)
		{
			for (var j = 0; j < i; j++)
				sb.Append(builder(j + 1, args[j].ordinal));
		}

		static string BuildArgumentString(int i, Func<int, string> builder) =>
			string.Join(", ", Enumerable.Range(0, i).Select(j => builder(j + 1)));

		for (var i = 2; i <= args.Length; i++)
		{
			var (ordinal, arity) = args[i - 1];

			sb.Append($@"
    /// <summary>
    /// Applies {arity} accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name=""T"">The type of elements in <paramref name=""source""/>.</typeparam>
    /// <typeparam name=""TResult"">The type of the accumulated result.</typeparam>
    /// <param name=""source"">The source sequence</param>
    /// <param name=""resultSelector"">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <returns>The value returned by <paramref name=""resultSelector""/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>");

			ForEachArgument(i, (j, ordinal) => $@"
    /// <typeparam name=""TAccumulate{j}"">The type of the {ordinal} accumulator value.</typeparam>
	/// <param name=""seed{j}"">The seed value for the {ordinal} accumulator.</param>
	/// <param name=""accumulator{j}"">The {ordinal} accumulator.</param>");

			sb.Append($@"
    public static TResult Aggregate<T, {BuildArgumentString(i, j => $"TAccumulate{j}")}, TResult>(
        this IEnumerable<T> source,");

			ForEachArgument(i, (j, ordinal) => $@"
        TAccumulate{j} seed{j}, Func<TAccumulate{j}, T, TAccumulate{j}> accumulator{j},");

			sb.Append($@"
        Func<{BuildArgumentString(i, j => $"TAccumulate{j}")}, TResult> resultSelector)
	{{
		Guard.IsNotNull(source);");

			ForEachArgument(i, (j, ordinal) => $@"
        Guard.IsNotNull(accumulator{j});");

			sb.Append($@"
        Guard.IsNotNull(resultSelector);

		foreach (var item in source)
		{{");

			ForEachArgument(i, (j, ordinal) => $@"
			seed{j} = accumulator{j}(seed{j}, item);");

			sb.Append($@"
		}}

		return resultSelector({BuildArgumentString(i, j => $"seed{j}")});
	}}");
		}

		sb.Append($@"
}}");

		return SourceText.From(sb.ToString(), Encoding.UTF8);
	}
}
