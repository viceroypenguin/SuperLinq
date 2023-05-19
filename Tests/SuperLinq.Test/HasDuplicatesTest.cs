namespace Test;

public class HasDuplicatesTest
{
	[Fact]
	public void When_Asking_For_Duplicates_On_Sequence_Without_Duplicates_Then_False_Is_Returned()
	{
		var stringArray = new[]
		{
			"FirstElement",
			"SecondElement",
			"ThirdElement"
		};

		var hasDuplicates = stringArray.HasDuplicates();

		Assert.False(hasDuplicates);
	}

	[Fact]
	public void When_Asking_For_Duplicates_On_Sequence_Projection_Without_Duplicates_Then_False_Is_Returned()
	{
		var dummyClasses = new DummyClass[]
		{
			new("FirstElement"),
			new("SecondElement"),
			new("ThirdElement")
		};

		var hasDuplicates = dummyClasses.HasDuplicates(x => x.ComparableString);

		Assert.False(hasDuplicates);
	}

	[Fact]
	public void When_Asking_For_Duplicates_On_Sequence_With_Duplicates_Then_True_Is_Returned()
	{
		var stringArray = new[]
		{
			"FirstElement",
			"DUPLICATED_STRING",
			"DUPLICATED_STRING",
			"ThirdElement"
		};

		var hasDuplicates = stringArray.HasDuplicates();

		Assert.True(hasDuplicates);
	}

	[Fact]
	public void When_Asking_For_Duplicates_On_Sequence_Projection_With_Duplicates_Then_True_Is_Returned()
	{
		var dummyClasses = new DummyClass[]
		{
			new("FirstElement"),
			new("DUPLICATED_STRING"),
			new("DUPLICATED_STRING"),
			new("ThirdElement")
		};

		var hasDuplicates = dummyClasses.HasDuplicates(x => x.ComparableString);

		Assert.True(hasDuplicates);
	}

	[Fact]
	public async void When_Asking_For_Duplicates_On_Sequence_With_Duplicates_Then_It_Does_Not_Iterate_Unnecessary_On_Elements()
	{
		var source = SuperEnumerable.From(() => "FirstElement",
			() => "DUPLICATED_STRING",
			() => "DUPLICATED_STRING",
			() => throw new TestException());

		var record = await Record.ExceptionAsync(() => Task.FromResult(source.HasDuplicates()));

		Assert.Null(record);
	}

	[Fact]
	public async void When_Asking_For_Duplicates_On_Sequence_Projection_With_Duplicates_Then_It_Does_Not_Iterate_Unnecessary_On_Elements()
	{
		var source = SuperEnumerable.From(() => new DummyClass("FirstElement"),
			() => new DummyClass("DUPLICATED_STRING"),
			() => new DummyClass("DUPLICATED_STRING"),
			() => throw new TestException());

		var record = await Record.ExceptionAsync(() => Task.FromResult(source.HasDuplicates(x => x.ComparableString)));

		Assert.Null(record);
	}

	[Fact]
	public void When_Asking_Duplicates_Then_It_Is_Executed_Right_Away()
	{
		_ = Assert.Throws<TestException>(() => _ = new BreakingSequence<string>().HasDuplicates());
	}

	[Fact]
	public void When_Asking_Duplicates_On_Sequence_Projection_Then_It_Is_Executed_Right_Away()
	{
		_ = Assert.Throws<TestException>(() => _ = new BreakingSequence<DummyClass>().HasDuplicates(x => x.ComparableString));
	}

	[Fact]
	public void When_Asking_For_Duplicates_On_Sequence_With_Custom_Always_True_Comparer_Then_True_Is_Returned()
	{
		var stringArray = new[]
		{
			"FirstElement",
			"SecondElement",
			"ThirdElement"
		};

		var hasDuplicates = stringArray.HasDuplicates(new DummyStringAlwaysTrueComparer());

		Assert.True(hasDuplicates);
	}

	[Fact]
	public void When_Asking_For_Duplicates_On_Sequence_Projection_With_Custom_Always_True_Comparer_Then_True_Is_Returned()
	{
		var dummyClasses = new DummyClass[]
		{
			new("FirstElement"),
			new("SecondElement"),
			new("ThirdElement")
		};

		var hasDuplicates = dummyClasses.HasDuplicates(x => x.ComparableString, new DummyStringAlwaysTrueComparer());

		Assert.True(hasDuplicates);
	}

	[Fact]
	public void When_Asking_For_Duplicates_On_None_Duplicates_Sequence_With_Custom_Always_True_Comparer_Then_True_Is_Returned()
	{
		var stringArray = new[]
		{
			"FirstElement",
			"SecondElement",
			"ThirdElement"
		};

		var hasDuplicates = stringArray.HasDuplicates(new DummyStringAlwaysTrueComparer());

		Assert.True(hasDuplicates);
	}

	[Fact]
	public void When_Asking_For_Duplicates_On_Sequence_With_Null_Comparer_Then_Default_Comparer_Is_Used_And_False_Is_Returned()
	{
		var stringArray = new[]
		{
			"FirstElement",
			"SecondElement",
			"ThirdElement"
		};

		var hasDuplicates = stringArray.HasDuplicates(null);

		Assert.False(hasDuplicates);
	}

	[Fact]
	public void When_Asking_For_Duplicates_On_Sequence_With_Custom_Always_False_Comparer_Then_False_Is_Returned()
	{
		var stringArray = new[]
		{
			"FirstElement",
			"SecondElement",
			"ThirdElement"
		};

		var hasDuplicates = stringArray.HasDuplicates(new DummyStringAlwaysFalseComparer());

		Assert.False(hasDuplicates);
	}

	[Fact]
	public void When_Asking_For_Duplicates_On_Multiple_Duplicates_Sequence_With_Custom_Always_False_Comparer_Then_False_Is_Returned()
	{
		var stringArray = new[]
		{
			"DUPLICATED_STRING",
			"DUPLICATED_STRING",
			"DUPLICATED_STRING"
		};

		var hasDuplicates = stringArray.HasDuplicates(new DummyStringAlwaysFalseComparer());

		Assert.False(hasDuplicates);
	}

	[Fact]
	public void When_Asking_For_Duplicates_On_Multiple_Duplicates_Sequence_Projection_With_Custom_Always_False_Comparer_Then_False_Is_Returned()
	{
		var dummyClasses = new DummyClass[]
		{
			new("DUPLICATED_STRING"),
			new("DUPLICATED_STRING"),
			new("DUPLICATED_STRING")
		};

		var hasDuplicates = dummyClasses.HasDuplicates(x => x.ComparableString, new DummyStringAlwaysFalseComparer());

		Assert.False(hasDuplicates);
	}

	private sealed class DummyClass
	{
		public string ComparableString { get; }

		public DummyClass(string comparableString) => ComparableString = comparableString;
	}

	private sealed class DummyStringAlwaysTrueComparer : IEqualityComparer<string>
	{
		public bool Equals(string? x, string? y) => true;

		public int GetHashCode(string obj) => 0;
	}

	private sealed class DummyStringAlwaysFalseComparer : IEqualityComparer<string>
	{
		public bool Equals(string? x, string? y) => false;

		public int GetHashCode(string obj) => 0;
	}
}
