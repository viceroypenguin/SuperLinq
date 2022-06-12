using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace SuperLinq.Test;

[TestFixture]
public class OrderedMergeTest
{
	static IEnumerable<T> Seq<T>(params T[] values) => values;

	public static IEnumerable<ITestCaseData> TestData =
		from e in new[]
		{
				new
				{
					A = Seq(0, 2, 4),
					B = Seq(0, 1, 2, 3, 4),
					R = Seq(0, 1, 2, 3, 4)
				},
				new
				{
					A = Seq(0, 1, 2, 3, 4),
					B = Seq(0, 2, 4),
					R = Seq(0, 1, 2, 3, 4)
				},
				new
				{
					A = Seq(0, 2, 4),
					B = Seq(1, 3, 5),
					R = Seq(0, 1, 2, 3, 4, 5)
				},
				new
				{
					A = Seq<int>(),
					B = Seq(0, 1, 2),
					R = Seq(0, 1, 2)
				},
				new
				{
					A = Seq(0, 1, 2),
					B = Seq<int>(),
					R = Seq(0, 1, 2)
				}
		}
		select new TestCaseData(e.A.AsTestingSequence(), e.B.AsTestingSequence()).Returns(e.R);

	[Test, TestCaseSource(nameof(TestData))]
	public IEnumerable<int> OrderedMerge(IEnumerable<int> first,
		IEnumerable<int> second)
	{
		return first.OrderedMerge(second);
	}
}
