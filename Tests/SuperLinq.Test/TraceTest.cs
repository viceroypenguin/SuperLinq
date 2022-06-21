using System.Diagnostics;
using System.Globalization;

namespace Test;

public class TraceTest
{
	[Fact]
	public void TraceSequence()
	{
		var trace = Lines(CaptureTrace(() => "the quick brown fox".Split().Trace().Consume()));
		trace.AssertSequenceEqual("the", "quick", "brown", "fox");
	}

	[Fact]
	public void TraceSequenceWithSomeNullElements()
	{
		var trace = Lines(CaptureTrace(() => new int?[] { 1, null, 2, null, 3 }.Trace().Consume()));
		trace.AssertSequenceEqual("1", string.Empty, "2", string.Empty, "3");
	}

	[Fact]
	public void TraceSequenceWithSomeNullReferences()
	{
		var trace = Lines(CaptureTrace(() => new[] { "the", null, "quick", null, "brown", null, "fox" }.Trace().Consume()));

		trace.AssertSequenceEqual("the", string.Empty, "quick", string.Empty, "brown", string.Empty, "fox");
	}

	[Fact]
	public void TraceSequenceWithFormatting()
	{
		var trace = Lines(CaptureTrace(delegate
		{
			using (new CurrentThreadCultureScope(CultureInfo.InvariantCulture))
				new[] { 1234, 5678 }.Trace("{0:N0}").Consume();
		}));

		trace.AssertSequenceEqual("1,234", "5,678");
	}

	[Fact]
	public void TraceSequenceWithFormatter()
	{
		var trace = Lines(CaptureTrace(delegate
		{
			var formatter = CultureInfo.InvariantCulture;
			new int?[] { 1234, null, 5678 }.Trace(n => n.HasValue
													   ? n.Value.ToString("N0", formatter)
													   : "#NULL")
										   .Consume();
		}));

		trace.AssertSequenceEqual("1,234", "#NULL", "5,678");
	}

	static IEnumerable<string> Lines(string str)
	{
		using (var e = _(string.IsNullOrEmpty(str)
					 ? TextReader.Null
					 : new StringReader(str)))
		{
			while (e.MoveNext())
				yield return e.Current;
		}

		static IEnumerator<string> _(TextReader reader)
		{
			Debug.Assert(reader != null);
			string? line;
			while ((line = reader.ReadLine()) != null)
				yield return line;
		}
	}

	static string CaptureTrace(Action action)
	{
		var writer = new StringWriter();
		var listener = new TextWriterTraceListener(writer);
		Trace.Listeners.Add(listener);
		try
		{
			action();
			Trace.Flush();
		}
		finally
		{
			Trace.Listeners.Remove(listener);
		}
		return writer.ToString();
	}
}
