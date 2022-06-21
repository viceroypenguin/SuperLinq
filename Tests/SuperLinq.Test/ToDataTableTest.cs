using System.Collections;
using System.Data;

namespace Test;

public class ToDataTableTest
{
	class TestObject
	{
		public int KeyField;
		public Guid? ANullableGuidField;

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
			KeyField = key;
			ANullableGuidField = Guid.NewGuid();

			ANullableDecimal = key / 3;
			AString = "ABCDEFGHIKKLMNOPQRSTUVWXYSZ";
		}
	}


	readonly IReadOnlyCollection<TestObject> _testObjects;


	public ToDataTableTest()
	{
		_testObjects = Enumerable.Range(0, 3)
								 .Select(i => new TestObject(i))
								 .ToArray();
	}

	[Fact]
	public void ToDataTableTableWithWrongColumnNames()
	{
		var dt = new DataTable();
		dt.Columns.Add("Test");

		Assert.Throws<ArgumentException>(() =>
			_testObjects.ToDataTable(dt));
	}

	[Fact]
	public void ToDataTableTableWithWrongColumnDataType()
	{
		var dt = new DataTable();
		dt.Columns.Add("AString", typeof(int));

		Assert.Throws<ArgumentException>(() =>
			_testObjects.ToDataTable(dt, t => t.AString));
	}

	[Fact]
	public void ToDataTableMemberExpressionMethod()
	{
		Assert.Throws<ArgumentException>(() =>
			_testObjects.ToDataTable(t => t.ToString()!));
	}


	[Fact]
	public void ToDataTableMemberExpressionNonMember()
	{
		Assert.Throws<ArgumentException>(() =>
			_testObjects.ToDataTable(t => t.ToString()!.Length));
	}

	[Fact]
	public void ToDataTableMemberExpressionIndexer()
	{
		Assert.Throws<ArgumentException>(() =>
			_testObjects.ToDataTable(t => t[0]));
	}

	[Fact]
	public void ToDataTableSchemaInDeclarationOrder()
	{
		var dt = _testObjects.ToDataTable();

		// Assert properties first, then fields, then in declaration order

		Assert.Equal("AString", dt.Columns[0].Caption);
		Assert.Equal(typeof(string), dt.Columns[0].DataType);

		Assert.Equal("ANullableDecimal", dt.Columns[1].Caption);
		Assert.Equal(typeof(decimal), dt.Columns[1].DataType);

		Assert.Equal("KeyField", dt.Columns[2].Caption);
		Assert.Equal(typeof(int), dt.Columns[2].DataType);

		Assert.Equal("ANullableGuidField", dt.Columns[3].Caption);
		Assert.Equal(typeof(Guid), dt.Columns[3].DataType);
		Assert.True(dt.Columns[3].AllowDBNull);

		Assert.Equal(4, dt.Columns.Count);
	}

	[Fact]
	public void ToDataTableContainsAllElements()
	{
		var dt = _testObjects.ToDataTable();
		Assert.Equal(_testObjects.Count, dt.Rows.Count);
	}

	[Fact]
	public void ToDataTableWithExpression()
	{
		var dt = _testObjects.ToDataTable(t => t.AString);

		Assert.Equal("AString", dt.Columns[0].Caption);
		Assert.Equal(typeof(string), dt.Columns[0].DataType);

		Assert.Single(dt.Columns);
	}

	[Fact]
	public void ToDataTableWithSchema()
	{
		var dt = new DataTable();
		var columns = dt.Columns;
		columns.Add("Column1", typeof(int));
		columns.Add("Value", typeof(string));
		columns.Add("Column3", typeof(int));
		columns.Add("Name", typeof(string));

		var vars = Environment.GetEnvironmentVariables()
							  .Cast<DictionaryEntry>()
							  .ToArray();

		vars.Select(e => new { Name = e.Key.ToString()!, Value = e.Value!.ToString()! })
			.ToDataTable(dt, e => e.Name, e => e.Value);

		var rows = dt.Rows.Cast<DataRow>().ToArray();
		Assert.Equal(vars.Length, rows.Length);
		Assert.Equal(vars.Select(e => e.Key).ToArray(), rows.Select(r => r["Name"]).ToArray());
		Assert.Equal(vars.Select(e => e.Value).ToArray(), rows.Select(r => r["Value"]).ToArray());
	}

	struct Point
	{
		public bool IsEmpty => X == 0 && Y == 0;
		public int X { get; }
		public int Y { get; }
		public Point(int x, int y) : this() { X = x; Y = y; }
	}

	[Fact]
	public void ToDataTableIgnoresStaticMembers()
	{
		var points = new[] { new Point(12, 34) }.ToDataTable();

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
