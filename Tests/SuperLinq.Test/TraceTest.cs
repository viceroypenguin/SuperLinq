using System.Diagnostics;
using System.Globalization;

namespace Test;

// Keep testing `Trace` for now
#pragma warning disable CS0618

public class TraceTest
{
	[Fact]
	public void TraceSequence()
	{
		var trace = CaptureTrace(() =>
		{
			using var seq = "the quick brown fox".Split().AsTestingSequence();
			seq
				.Trace()
				.Consume();
		});
		trace.AssertSequenceEqual("the", "quick", "brown", "fox");
	}

	[Fact]
	public void TraceSequenceWithSomeNullElements()
	{
		var trace = CaptureTrace(() =>
		{
			using var seq = TestingSequence.Of<int?>(1, null, 2, null, 3);
			seq
				.Trace()
				.Consume();
		});
		trace.AssertSequenceEqual("1", string.Empty, "2", string.Empty, "3");
	}

	[Fact]
	public void TraceSequenceWithSomeNullReferences()
	{
		var trace = CaptureTrace(() =>
		{
			using var seq = TestingSequence.Of("the", null, "quick", null, "brown", null, "fox");
			seq
				.Trace()
				.Consume();
		});

		trace.AssertSequenceEqual("the", string.Empty, "quick", string.Empty, "brown", string.Empty, "fox");
	}

	[Fact]
	public void TraceSequenceWithFormatting()
	{
		var trace = CaptureTrace(delegate
		{
			using (new CurrentThreadCultureScope(CultureInfo.InvariantCulture))
			{
				using var seq = TestingSequence.Of(1234, 5678);
				seq
					.Trace("{0:N0}")
					.Consume();
			}
		});

		trace.AssertSequenceEqual("1,234", "5,678");
	}

	[Fact]
	public void TraceSequenceWithFormatter()
	{
		var trace = CaptureTrace(delegate
		{
			using var seq = TestingSequence.Of<int?>(1234, null, 5678);
			seq
				.Trace(n => n.HasValue ? n.Value.ToString("N0", CultureInfo.InvariantCulture) : "#NULL")
				.Consume();
		});

		trace.AssertSequenceEqual("1,234", "#NULL", "5,678");
	}

	private static IEnumerable<string> Lines(string str)
	{
		using (var e = Core(string.IsNullOrEmpty(str)
					 ? TextReader.Null
					 : new StringReader(str)))
		{
			while (e.MoveNext())
				yield return e.Current;
		}

		static IEnumerator<string> Core(TextReader reader)
		{
			Debug.Assert(reader != null);
			string? line;
			while ((line = reader.ReadLine()) != null)
				yield return line;
		}
	}

	private static IEnumerable<string> CaptureTrace(Action action)
	{
		var writer = new StringWriter();
		var listener = new TextWriterTraceListener(writer);
		_ = Trace.Listeners.Add(listener);
		try
		{
			action();
			Trace.Flush();
		}
		finally
		{
			Trace.Listeners.Remove(listener);
		}
		return Lines(writer.ToString());
	}
}
