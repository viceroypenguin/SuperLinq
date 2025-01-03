using System.Collections;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace SuperLinq.Tests;

[SuppressMessage("Design", "MA0150:Do not call the default object.ToString explicitly")]
public sealed class ToDataTableTest
{
	private sealed class TestObject(int key)
	{
		public int KeyField = key;
		public Guid? ANullableGuidField = Guid.NewGuid();

		public string AString { get; } = "ABCDEFGHIKKLMNOPQRSTUVWXYSZ";
		public decimal? ANullableDecimal { get; } = key / 3;
		public object Unreadable { set => throw new NotSupportedException(); }

		public object this[int index]
		{
			get => new();
			set { }
		}
	}

	private readonly TestObject[] _testObjects = Enumerable
		.Range(0, 3)
		.Select(i => new TestObject(i))
		.ToArray();

	[Test]
	public void ToDataTableTableWithWrongColumnNames()
	{
		using var dt = new DataTable();
		_ = dt.Columns.Add("Test");

		using var xs = _testObjects.AsTestingSequence();
		_ = Assert.Throws<ArgumentException>(() =>
			xs.ToDataTable(dt));
	}

	[Test]
	public void ToDataTableTableWithWrongColumnDataType()
	{
		using var dt = new DataTable();
		_ = dt.Columns.Add("AString", typeof(int));

		using var xs = _testObjects.AsTestingSequence();
		_ = Assert.Throws<ArgumentException>(() =>
			xs.ToDataTable(dt, t => t.AString));
	}

	[Test]
	public void ToDataTableMemberExpressionMethod()
	{
		using var xs = _testObjects.AsTestingSequence();
		_ = Assert.Throws<ArgumentException>(() =>
			xs.ToDataTable(t => t.ToString()!));
	}

	[Test]
	public void ToDataTableMemberExpressionNonMember()
	{
		using var xs = _testObjects.AsTestingSequence();
		_ = Assert.Throws<ArgumentException>(() =>
			xs.ToDataTable(t => t.ToString()!.Length));
	}

	[Test]
	public void ToDataTableMemberExpressionIndexer()
	{
		using var xs = _testObjects.AsTestingSequence();
		_ = Assert.Throws<ArgumentException>(() =>
			xs.ToDataTable(t => t[0]));
	}

	[Test]
	public void ToDataTableSchemaInDeclarationOrder()
	{
		using var xs = _testObjects.AsTestingSequence();
		using var dt = xs.ToDataTable();

		// Assert properties first, then fields, then in declaration order

		Assert.Equal(nameof(TestObject.AString), dt.Columns[0].Caption);
		Assert.Equal(typeof(string), dt.Columns[0].DataType);

		Assert.Equal(nameof(TestObject.ANullableDecimal), dt.Columns[1].Caption);
		Assert.Equal(typeof(decimal), dt.Columns[1].DataType);

		Assert.Equal(nameof(TestObject.KeyField), dt.Columns[2].Caption);
		Assert.Equal(typeof(int), dt.Columns[2].DataType);

		Assert.Equal(nameof(TestObject.ANullableGuidField), dt.Columns[3].Caption);
		Assert.Equal(typeof(Guid), dt.Columns[3].DataType);
		Assert.True(dt.Columns[3].AllowDBNull);

		Assert.Equal(4, dt.Columns.Count);
	}

	[Test]
	public void ToDataTableContainsAllElements()
	{
		using var xs = _testObjects.AsTestingSequence();
		using var dt = xs.ToDataTable();
		Assert.Equal(_testObjects.Length, dt.Rows.Count);
	}

	[Test]
	public void ToDataTableWithExpression()
	{
		using var xs = _testObjects.AsTestingSequence();
		using var dt = xs.ToDataTable(t => t.AString);

		Assert.Equal("AString", dt.Columns[0].Caption);
		Assert.Equal(typeof(string), dt.Columns[0].DataType);

		_ = Assert.Single(dt.Columns);
	}

	[Test]
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
		public Point(int x, int y) : this() => (X, Y) = (x, y);
	}

	[Test]
	public void ToDataTableIgnoresStaticMembers()
	{
		using var xs = new[] { new Point(12, 34) }.AsTestingSequence();
		using var points = xs.ToDataTable();

		Assert.Equal(3, points.Columns.Count);

		var (x, y, empty) = (
			points.Columns["X"],
			points.Columns["Y"],
			points.Columns["IsEmpty"]
		);

		Assert.NotNull(points.Columns["X"]);
		Assert.NotNull(points.Columns["Y"]);
		Assert.NotNull(points.Columns["IsEmpty"]);

		var row = points.Rows.Cast<DataRow>().Single();
		Assert.Equal(12, row[x!]);
		Assert.Equal(34, row[y!]);
		Assert.Equal(false, row[empty!]);
	}
}
