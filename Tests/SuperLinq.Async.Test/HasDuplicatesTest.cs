namespace Test.Async;

public class HasDuplicatesTest
{
	[Fact]
	public async Task When_Asking_For_Duplicates_On_Sequence_Without_Duplicates_Then_False_Is_Returned()
	{
		var asyncSequence = AsyncSuperEnumerable.From(
			async () =>
			{
				await Task.Delay(100);
				return "FirstElement";
			},
			async () =>
			{
				await Task.Delay(200);
				return "SecondElement";
			},
			async () =>
			{
				await Task.Delay(300);
				return "ThirdElement";
			});

		var hasDuplicates = await asyncSequence.HasDuplicates();

		Assert.False(hasDuplicates);
	}

	[Fact]
	public async Task When_Asking_For_Duplicates_On_Sequence_Projection_Without_Duplicates_Then_False_Is_Returned()
	{
		var dummyClasses = AsyncSuperEnumerable.From(
			async () =>
			{
				await Task.Delay(100);
				return new DummyClass("FirstElement");
			},
			async () =>
			{
				await Task.Delay(200);
				return new DummyClass("SecondElement");
			},
			async () =>
			{
				await Task.Delay(300);
				return new DummyClass("ThirdElement");
			});

		var hasDuplicates = await dummyClasses.HasDuplicates(x => x.ComparableString);

		Assert.False(hasDuplicates);
	}

	[Fact]
	public async Task When_Asking_For_Duplicates_On_Sequence_With_Duplicates_Then_True_Is_Returned()
	{
		var asyncSequence = AsyncSuperEnumerable.From(
			async () =>
			{
				await Task.Delay(100);
				return "FirstElement";
			},
			async () =>
			{
				await Task.Delay(200);
				return "DUPLICATED_STRING";
			},
			async () =>
			{
				await Task.Delay(300);
				return "DUPLICATED_STRING";
			},
			async () =>
			{
				await Task.Delay(400);
				return "ThirdElement";
			});

		var hasDuplicates = await asyncSequence.HasDuplicates();

		Assert.True(hasDuplicates);
	}

	[Fact]
	public async Task When_Asking_For_Duplicates_On_Sequence_Projection_With_Duplicates_Then_True_Is_Returned()
	{
		var dummyClasses = AsyncSuperEnumerable.From(
			async () =>
			{
				await Task.Delay(100);
				return new DummyClass("FirstElement");
			},
			async () =>
			{
				await Task.Delay(200);
				return new DummyClass("DUPLICATED_STRING");
			},
			async () =>
			{
				await Task.Delay(300);
				return new DummyClass("DUPLICATED_STRING");
			},
			async () =>
			{
				await Task.Delay(400);
				return new DummyClass("ThirdElement");
			});

		var hasDuplicates = await dummyClasses.HasDuplicates(x => x.ComparableString);

		Assert.True(hasDuplicates);
	}

	[Fact]
	public async Task When_Asking_For_Duplicates_On_Sequence_With_Duplicates_Then_It_Does_Not_Iterate_Unnecessary_On_Elements()
	{
		var asyncSequence = AsyncSuperEnumerable.From(
			async () =>
			{
				await Task.Delay(100);
				return "FirstElement";
			},
			async () =>
			{
				await Task.Delay(200);
				return "DUPLICATED_STRING";
			},
			async () =>
			{
				await Task.Delay(300);
				return "DUPLICATED_STRING";
			},
			async () =>
			{
				await Task.Delay(400);
				throw new TestException();
			});

		var record = await Record.ExceptionAsync(async () => await asyncSequence.HasDuplicates());

		Assert.Null(record);
	}

	[Fact]
	public async Task When_Asking_For_Duplicates_On_Sequence_Projection_With_Duplicates_Then_It_Does_Not_Iterate_Unnecessary_On_Elements()
	{
		var dummyClasses = AsyncSuperEnumerable.From(
			async () =>
			{
				await Task.Delay(100);
				return new DummyClass("FirstElement");
			},
			async () =>
			{
				await Task.Delay(200);
				return new DummyClass("DUPLICATED_STRING");
			},
			async () =>
			{
				await Task.Delay(300);
				return new DummyClass("DUPLICATED_STRING");
			},
			async () =>
			{
				await Task.Delay(400);
				throw new TestException();
			});

		var record = await Record.ExceptionAsync(async () => await dummyClasses.HasDuplicates(x => x.ComparableString));

		Assert.Null(record);
	}

	[Fact]
	public async Task When_Asking_Duplicates_Then_It_Is_Executed_Right_Away()
	{
		_ = await Assert.ThrowsAsync<TestException>(async () => _ = await new AsyncBreakingSequence<string>().HasDuplicates());
	}

	[Fact]
	public async Task When_Asking_Duplicates_On_Sequence_Projection_Then_It_Is_Executed_Right_Away()
	{
		_ = await Assert.ThrowsAsync<TestException>(async () => _ = await new AsyncBreakingSequence<DummyClass>().HasDuplicates(x => x.ComparableString));
	}

	[Fact]
	public async Task When_Asking_For_Duplicates_On_Sequence_With_Custom_Always_True_Comparer_Then_True_Is_Returned()
	{
		var asyncSequence = AsyncSuperEnumerable.From(
			async () =>
			{
				await Task.Delay(100);
				return "FirstElement";
			},
			async () =>
			{
				await Task.Delay(200);
				return "SecondElement";
			},
			async () =>
			{
				await Task.Delay(300);
				return "ThirdElement";
			});

		var hasDuplicates = await asyncSequence.HasDuplicates(new DummyStringAlwaysTrueComparer());

		Assert.True(hasDuplicates);
	}

	[Fact]
	public async Task When_Asking_For_Duplicates_On_Sequence_Projection_With_Custom_Always_True_Comparer_Then_True_Is_Returned()
	{
		var dummyClasses = AsyncSuperEnumerable.From(
			async () =>
			{
				await Task.Delay(100);
				return new DummyClass("FirstElement");
			},
			async () =>
			{
				await Task.Delay(200);
				return new DummyClass("SecondElement");
			},
			async () =>
			{
				await Task.Delay(300);
				return new DummyClass("ThirdElement");
			});

		var hasDuplicates = await dummyClasses.HasDuplicates(x => x.ComparableString, new DummyStringAlwaysTrueComparer());

		Assert.True(hasDuplicates);
	}

	[Fact]
	public async Task When_Asking_For_Duplicates_On_None_Duplicates_Sequence_With_Custom_Always_True_Comparer_Then_True_Is_Returned()
	{
		var asyncSequence = AsyncSuperEnumerable.From(
			async () =>
			{
				await Task.Delay(100);
				return "FirstElement";
			},
			async () =>
			{
				await Task.Delay(200);
				return "SecondElement";
			},
			async () =>
			{
				await Task.Delay(300);
				return "ThirdElement";
			});

		var hasDuplicates = await asyncSequence.HasDuplicates(new DummyStringAlwaysTrueComparer());

		Assert.True(hasDuplicates);
	}

	[Fact]
	public async Task When_Asking_For_Duplicates_On_Sequence_With_Null_Comparer_Then_Default_Comparer_Is_Used_And_False_Is_Returned()
	{
		var asyncSequence = AsyncSuperEnumerable.From(
			async () =>
			{
				await Task.Delay(100);
				return "FirstElement";
			},
			async () =>
			{
				await Task.Delay(200);
				return "SecondElement";
			},
			async () =>
			{
				await Task.Delay(300);
				return "ThirdElement";
			});

		var hasDuplicates = await asyncSequence.HasDuplicates(null);

		Assert.False(hasDuplicates);
	}

	[Fact]
	public async Task When_Asking_For_Duplicates_On_Sequence_With_Custom_Always_False_Comparer_Then_False_Is_Returned()
	{
		var asyncSequence = AsyncSuperEnumerable.From(
			async () =>
			{
				await Task.Delay(100);
				return "FirstElement";
			},
			async () =>
			{
				await Task.Delay(200);
				return "SecondElement";
			},
			async () =>
			{
				await Task.Delay(300);
				return "ThirdElement";
			});

		var hasDuplicates = await asyncSequence.HasDuplicates(new DummyStringAlwaysFalseComparer());

		Assert.False(hasDuplicates);
	}

	[Fact]
	public async Task When_Asking_For_Duplicates_On_Multiple_Duplicates_Sequence_With_Custom_Always_False_Comparer_Then_False_Is_Returned()
	{
		var asyncSequence = AsyncSuperEnumerable.From(
			async () =>
			{
				await Task.Delay(100);
				return "DUPLICATED_STRING";
			},
			async () =>
			{
				await Task.Delay(200);
				return "DUPLICATED_STRING";
			},
			async () =>
			{
				await Task.Delay(300);
				return "DUPLICATED_STRING";
			});

		var hasDuplicates = await asyncSequence.HasDuplicates(new DummyStringAlwaysFalseComparer());

		Assert.False(hasDuplicates);
	}

	[Fact]
	public async Task When_Asking_For_Duplicates_On_Multiple_Duplicates_Sequence_Projection_With_Custom_Always_False_Comparer_Then_False_Is_Returned()
	{
		var asyncSequence = AsyncSuperEnumerable.From(
			async () =>
			{
				await Task.Delay(100);
				return new DummyClass("DUPLICATED_STRING");
			},
			async () =>
			{
				await Task.Delay(200);
				return new DummyClass("DUPLICATED_STRING");
			},
			async () =>
			{
				await Task.Delay(300);
				return new DummyClass("DUPLICATED_STRING");
			});

		var hasDuplicates = await asyncSequence.HasDuplicates(x => x.ComparableString, new DummyStringAlwaysFalseComparer());

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
