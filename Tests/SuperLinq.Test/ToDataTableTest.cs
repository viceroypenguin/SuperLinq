using System.Collections;
using System.Data;

namespace Test;

public class ToDataTableTest
{
	private class TestObject
	{
		public int _keyField;
		public Guid? _aNullableGuidField;

		public string AString { get; }
		public decimal? ANullableDecimal { get; }
		public object Unreadable { set => throw new NotImplementedException(); }

		public object this[int index]
		{
			get => new object();
			set { }
		}


		public TestObject(int key)
		{
			_keyField = key;
			_aNullableGuidField = Guid.NewGuid();

			ANullableDecimal = key / 3;
			AString = "ABCDEFGHIKKLMNOPQRSTUVWXYSZ";
		}
	}

	private readonly IReadOnlyCollection<TestObject> _testObjects;

	public ToDataTableTest()
	{
		_testObjects = Enumerable
			.Range(0, 3)
			.Select(i => new TestObject(i))
			.ToArray();
	}

	[Fact]
	public void ToDataTableTableWithWrongColumnNames()
	{
		using var dt = new DataTable();
		_ = dt.Columns.Add("Test");

		using var xs = _testObjects.AsTestingSequence();
		_ = Assert.Throws<ArgumentException>(() =>
			xs.ToDataTable(dt));
	}

	[Fact]
	public void ToDataTableTableWithWrongColumnDataType()
	{
		using var dt = new DataTable();
		_ = dt.Columns.Add("AString", typeof(int));

		using var xs = _testObjects.AsTestingSequence();
		_ = Assert.Throws<ArgumentException>(() =>
			xs.ToDataTable(dt, t => t.AString));
	}

	[Fact]
	public void ToDataTableMemberExpressionMethod()
	{
		using var xs = _testObjects.AsTestingSequence();
		_ = Assert.Throws<ArgumentException>(() =>
			xs.ToDataTable(t => t.ToString()!));
	}

	[Fact]
	public void ToDataTableMemberExpressionNonMember()
	{
		using var xs = _testObjects.AsTestingSequence();
		_ = Assert.Throws<ArgumentException>(() =>
			xs.ToDataTable(t => t.ToString()!.Length));
	}

	[Fact]
	public void ToDataTableMemberExpressionIndexer()
	{
		using var xs = _testObjects.AsTestingSequence();
		_ = Assert.Throws<ArgumentException>(() =>
			xs.ToDataTable(t => t[0]));
	}

	[Fact]
	public void ToDataTableSchemaInDeclarationOrder()
	{
		using var xs = _testObjects.AsTestingSequence();
		using var dt = xs.ToDataTable();

		// Assert properties first, then fields, then in declaration order

		Assert.Equal(nameof(TestObject.AString), dt.Columns[0].Caption);
		Assert.Equal(typeof(string), dt.Columns[0].DataType);

		Assert.Equal(nameof(TestObject.ANullableDecimal), dt.Columns[1].Caption);
		Assert.Equal(typeof(decimal), dt.Columns[1].DataType);

		Assert.Equal(nameof(TestObject._keyField), dt.Columns[2].Caption);
		Assert.Equal(typeof(int), dt.Columns[2].DataType);

		Assert.Equal(nameof(TestObject._aNullableGuidField), dt.Columns[3].Caption);
		Assert.Equal(typeof(Guid), dt.Columns[3].DataType);
		Assert.True(dt.Columns[3].AllowDBNull);

		Assert.Equal(4, dt.Columns.Count);
	}

	[Fact]
	public void ToDataTableContainsAllElements()
	{
		using var xs = _testObjects.AsTestingSequence();
		using var dt = xs.ToDataTable();
		Assert.Equal(_testObjects.Count, dt.Rows.Count);
	}

	[Fact]
	public void ToDataTableWithExpression()
	{
		using var xs = _testObjects.AsTestingSequence();
		using var dt = xs.ToDataTable(t => t.AString);

		Assert.Equal("AString", dt.Columns[0].Caption);
		Assert.Equal(typeof(string), dt.Columns[0].DataType);

		_ = Assert.Single(dt.Columns);
	}

	[Fact]
	public void ToDataTableWithSchema()
	{
		using var dt = new DataTable();
		var columns = dt.Columns;
		_ = columns.Add("Column1", typeof(int));
		_ = columns.Add("Value", typeof(string));
		_ = columns.Add("Column3", typeof(int));
		_ = columns.Add("Name", typeof(string));

		var vars = Environment.GetEnvironmentVariables()
			.Cast<DictionaryEntry>()
			.ToArray();
		using var xs = vars
			.Select(e => new { Name = e.Key.ToString()!, Value = e.Value!.ToString()! })
			.AsTestingSequence();

		_ = xs.ToDataTable(dt, e => e.Name, e => e.Value);

		var rows = dt.Rows.Cast<DataRow>().ToArray();
		Assert.Equal(vars.Length, rows.Length);
		Assert.Equal(vars.Select(e => e.Key).ToArray(), rows.Select(r => r["Name"]).ToArray());
		Assert.Equal(vars.Select(e => e.Value).ToArray(), rows.Select(r => r["Value"]).ToArray());
	}

	private readonly struct Point
	{
		public bool IsEmpty => X == 0 && Y == 0;
		public int X { get; }
		public int Y { get; }
		public Point(int x, int y) : this() { X = x; Y = y; }
	}

	[Fact]
	public void ToDataTableIgnoresStaticMembers()
	{
		using var xs = new[] { new Point(12, 34) }.AsTestingSequence();
		using var points = xs.ToDataTable();

		Assert.Equal(3, points.Columns.Count);

		DataColumn? x, y, empty;
		Assert.NotNull(x = points.Columns["X"]);
		Assert.NotNull(y = points.Columns["Y"]);
		Assert.NotNull(empty = points.Columns["IsEmpty"]);

		var row = points.Rows.Cast<DataRow>().Single();
		Assert.Equal(12, row[x!]);
		Assert.Equal(34, row[y!]);
		Assert.Equal(false, row[empty!]);
	}
}
