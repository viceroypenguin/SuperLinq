using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace SuperLinq;

internal static class Fold
{
	public static SourceText Generate()
	{
		var sb = new StringBuilder();
		sb.Append(@"
namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{");

		var overloads =
			from i in Enumerable.Range(1, 16)
			let istr = i.ToString(CultureInfo.InvariantCulture)
			let i1str = (i + 1).ToString(CultureInfo.InvariantCulture)
			select new
			{
				Ts = string.Join(", ", Enumerable.Repeat("T", i)),
				Count = i,
				CountElements = istr + " " + (i == 1 ? "element" : "elements"),
				CountArg = istr,
				Count1Arg = i1str,
				FolderArgs = "folder" + istr + ": folder",
			};

		foreach (var e in overloads)
		{
			sb.Append($@"
    /// <summary>
    /// Returns the result of applying a function to a sequence of {e.CountElements}.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers
    /// as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name=""T"">Type of element in the source sequence</typeparam>
    /// <typeparam name=""TResult"">Type of the result</typeparam>
    /// <param name=""source"">The sequence of items to fold.</param>
    /// <param name=""folder"">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name=""folder""/>.</returns>
    /// <exception cref=""ArgumentNullException""><paramref name=""source""/> is null</exception>
    /// <exception cref=""ArgumentNullException""><paramref name=""folder""/> is null</exception>
    /// <exception cref=""InvalidOperationException""><paramref name=""source""/> does not contain exactly {e.CountElements}.</exception>
    public static async ValueTask<TResult> Fold<T, TResult>(this IAsyncEnumerable<T> source, Func<{e.Ts}, TResult> folder)
    {{
		source.ThrowIfNull();
		folder.ThrowIfNull();

		var elements = await source.Take({e.Count1Arg}).ToListAsync();
		if (elements.Count != {e.CountArg})
			throw new InvalidOperationException(
				$""Sequence contained an incorrect number of elements. (Expected: {e.CountArg}, Actual: {{elements.Count}})"");

		return folder(");

			for (var i = 0; i < e.Count; i++)
				sb.Append($@"
			elements[{i}]{(i == e.Count - 1 ? "" : ",")}");

			sb.Append(@"
		);
	}");
		}

		sb.Append(@"
}");

		return SourceText.From(sb.ToString(), Encoding.UTF8);
	}
}
