namespace Test;

public class ToArrayTests
{
	[Fact]
	public void ToArrayWithSmallerAmountOfElementsThanLength()
	{
		const int Length = 2;
		var enumerable = Enumerable.Range(0, 1);

		var result = enumerable.ToArray(Length);

		Assert.Equal(enumerable, result);
	}

	[Theory]
	[InlineData(3)]
	[InlineData(5)]
	public void ToArrayWithHigherAmountOfElementsThanLength(int dataLength)
	{
		const int Length = 2;
		var enumerable = Enumerable.Range(0, dataLength);

		var result = enumerable.ToArray(Length);

		Assert.Equal(enumerable, result);
	}

	[Fact]
	public void ToArrayWithLengthSmallerThan0()
	{
		const int Length = -1;
		var enumerable = Enumerable.Range(0, 1);

		Assert.Throws<ArgumentOutOfRangeException>(() => enumerable.ToArray(Length));
	}
}
