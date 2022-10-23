namespace Test;

public class ToListTests
{
	[Fact]
	public void ToListWithSmallerAmountOfElementsThanLength()
	{
		const int Length = 2;
		var enumerable = Enumerable.Range(0, 1);

		var result = enumerable.ToList(Length);

		Assert.Equal(enumerable, result);
		Assert.Equal(Length, result.Capacity);
	}

	[Theory]
	[InlineData(3)]
	[InlineData(5)]
	public void ToListWithHigherAmountOfElementsThanLength(int dataLength)
	{
		const int Length = 2;
		var enumerable = Enumerable.Range(0, dataLength);

		var result = enumerable.ToList(Length);

		Assert.Equal(enumerable, result);
		Assert.True(result.Capacity > Length);
	}

	[Fact]
	public void ToListWithLengthEqualTo0()
	{
		const int Length = 0;
		var enumerable = Enumerable.Range(0, 1);

		var result = enumerable.ToList(Length);

		Assert.Equal(enumerable, result);
	}

	[Fact]
	public void ToListWithLengthSmallerThan0()
	{
		const int Length = -1;
		var enumerable = Enumerable.Range(0, 1);

		Assert.Throws<ArgumentOutOfRangeException>(() => enumerable.ToList(Length));
	}
}
