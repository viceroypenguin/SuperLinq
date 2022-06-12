using System.Globalization;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SuperLinq.Experimental;

namespace SuperLinq.Test;
using static FuncModule;

[TestFixture]
public class AggregateTest
{
	public static IEnumerable<ITestCaseData> AccumulatorsTestSource(string name, int count) =>

		/* Generates an invocation as follows for 2 accumulators:

			Enumerable.Range(1, count)
					  .Shuffle()
					  .Aggregate(0, (s, e) => s + e,
								 0, (s, e) => s + e,
								 (sum1, sum2) => new[] { sum1, sum2 });
		*/

		from source in new[] { Enumerable.Range(1, count).Shuffle() }
		let sum = source.Sum()
		from m in typeof(SuperEnumerable).GetMethods(BindingFlags.Public | BindingFlags.Static)
		where m.Name == nameof(SuperEnumerable.Aggregate)
		   && m.IsGenericMethodDefinition
		select new
		{
			Source = source,
			Expectation = sum,
			Instantiation = m.MakeGenericMethod(Enumerable.Repeat(typeof(int), m.GetGenericArguments().Length - 1)
											 .Append(typeof(int[])) // TResult
											 .ToArray()),
		}
		into m
		let rst = m.Instantiation.GetParameters().Last().ParameterType
		select new
		{
			m.Instantiation,
			m.Source,
			m.Expectation,
			AccumulatorCount = (m.Instantiation.GetParameters().Length - 2 /* source + resultSelector */) / 2 /* seed + accumulator */,
			ResultSelectorType = rst,
			Parameters =
				rst.GetMethod("Invoke")
				   .GetParameters()
				   .Select(p => Expression.Parameter(p.ParameterType))
				   .ToArray(),
		}
		into m
		let resultSelector =
			Expression.Lambda(m.ResultSelectorType,
							  Expression.NewArrayInit(typeof(int), m.Parameters),
							  m.Parameters)
					  .Compile()
		let accumulator = Func((int s, int n) => s + n)
		select new
		{
			Name = $"{name}({m.AccumulatorCount})",
			Method = m.Instantiation,
			Args =
				Enumerable.Repeat(m.Source, 1)
						  .Concat(from pairs in Enumerable.Repeat(new object[] { /* seed */ 0, accumulator }, m.AccumulatorCount)
								  from pair in pairs
								  select pair)
						  .Append(resultSelector)
						  .ToArray(),
			Expectation =
				Enumerable.Repeat(m.Expectation, m.AccumulatorCount)
						  .ToArray(),
		}
		into t
		select new TestCaseData(t.Method, t.Args).SetName(t.Name).Returns(t.Expectation);

	[TestCaseSource(nameof(AccumulatorsTestSource), new object[] { nameof(Accumulators), 10 })]
	public object Accumulators(MethodInfo method, object[] args) =>
		method.Invoke(null, args);

	[Test]
	public void SevenUniqueAccumulators()
	{
		var result =
			Enumerable
				.Range(1, 10)
				.Shuffle()
				.Select(n => new { Num = n, Str = n.ToString(CultureInfo.InvariantCulture) })
				.Aggregate(
					0, (s, e) => s + e.Num,
					0, (s, e) => e.Num % 2 == 0 ? s + e.Num : s,
					0, (s, _) => s + 1,
					(int?)null, (s, e) => s is { } n ? Math.Min(n, e.Num) : e.Num,
					(int?)null, (s, e) => s is { } n ? Math.Max(n, e.Num) : e.Num,
					new HashSet<int>(), (s, e) => { s.Add(e.Str.Length); return s; },
					new List<(int Num, string Str)>(), (s, e) => { s.Add((e.Num, e.Str)); return s; },
					(sum, esum, count, min, max, lengths, items) => new
					{
						Sum = sum,
						EvenSum = esum,
						Count = count,
						Average = (double)sum / count,
						Min = min is { } mn ? mn : throw new InvalidOperationException(),
						Max = max is { } mx ? mx : throw new InvalidOperationException(),
						UniqueLengths = lengths,
						Items = items,
					}
				);

		Assert.That(result.Sum, Is.EqualTo(55));
		Assert.That(result.EvenSum, Is.EqualTo(30));
		Assert.That(result.Count, Is.EqualTo(10));
		Assert.That(result.Average, Is.EqualTo(5.5));
		Assert.That(result.Min, Is.EqualTo(1));
		Assert.That(result.Max, Is.EqualTo(10));
		result.UniqueLengths.OrderBy(n => n).AssertSequenceEqual(1, 2);
		result.Items
			  .Select(e => new { e.Num, e.Str })
			  .OrderBy(e => e.Num)
			  .AssertSequenceEqual(new { Num = 1, Str = "1" },
								   new { Num = 2, Str = "2" },
								   new { Num = 3, Str = "3" },
								   new { Num = 4, Str = "4" },
								   new { Num = 5, Str = "5" },
								   new { Num = 6, Str = "6" },
								   new { Num = 7, Str = "7" },
								   new { Num = 8, Str = "8" },
								   new { Num = 9, Str = "9" },
								   new { Num = 10, Str = "10" });
	}

	[Test]
	public void SevenUniqueAccumulatorComprehensions()
	{
		var result =
			Enumerable
				.Range(1, 10)
				.Shuffle()
				.Select(n => new { Num = n, Str = n.ToString(CultureInfo.InvariantCulture) })
				.Aggregate(
					s => s.Sum(e => e.Num),
					s => s.Select(e => e.Num).Where(n => n % 2 == 0).Sum(),
					s => s.Count(),
					s => s.Min(e => e.Num),
					s => s.Max(e => e.Num),
					s => s.Select(e => e.Str.Length).Distinct().ToArray(),
					s => s.ToArray(),
					(sum, esum, count, min, max, lengths, items) => new
					{
						Sum = sum,
						EvenSum = esum,
						Count = count,
						Average = (double)sum / count,
						Min = min,
						Max = max,
						UniqueLengths = lengths,
						Items = items,
					}
				);

		Assert.That(result.Sum, Is.EqualTo(55));
		Assert.That(result.EvenSum, Is.EqualTo(30));
		Assert.That(result.Count, Is.EqualTo(10));
		Assert.That(result.Average, Is.EqualTo(5.5));
		Assert.That(result.Min, Is.EqualTo(1));
		Assert.That(result.Max, Is.EqualTo(10));
		result.UniqueLengths.OrderBy(n => n).AssertSequenceEqual(1, 2);
		result.Items
			  .OrderBy(e => e.Num)
			  .AssertSequenceEqual(new { Num = 1, Str = "1" },
								   new { Num = 2, Str = "2" },
								   new { Num = 3, Str = "3" },
								   new { Num = 4, Str = "4" },
								   new { Num = 5, Str = "5" },
								   new { Num = 6, Str = "6" },
								   new { Num = 7, Str = "7" },
								   new { Num = 8, Str = "8" },
								   new { Num = 9, Str = "9" },
								   new { Num = 10, Str = "10" });
	}

	[Test(Description = "https://github.com/morelinq/MoreLINQ/issues/616")]
	public void Issue616()
	{
		var (first, last) =
			Enumerable.Range(1, 10)
					  .Aggregate(ds => ds.FirstAsync(),
								 ds => ds.LastAsync(),
								 ValueTuple.Create);

		Assert.That(first, Is.EqualTo(1));
		Assert.That(last, Is.EqualTo(10));
	}
}
