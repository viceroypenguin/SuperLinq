﻿using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Appends elements in the sequence as rows of a given <see cref="DataTable"/> object.
	/// </summary>
	/// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <typeparam name="TTable"></typeparam>
	/// <param name="source">The source.</param>
	/// <param name="table"></param>
	/// <returns>
	/// A <see cref="DataTable"/> or subclass representing the source.
	/// </returns>
	/// <remarks>This operator uses immediate execution.</remarks>

	public static TTable ToDataTable<T, TTable>(this IEnumerable<T> source, TTable table)
		where TTable : DataTable
	{
		return ToDataTable(source, table, Array.Empty<Expression<Func<T, object>>>());
	}

	/// <summary>
	/// Appends elements in the sequence as rows of a given <see cref="DataTable"/>
	/// object with a set of lambda expressions specifying which members (property
	/// or field) of each element in the sequence will supply the column values.
	/// </summary>
	/// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">The source.</param>
	/// <param name="expressions">Expressions providing access to element members.</param>
	/// <returns>
	/// A <see cref="DataTable"/> representing the source.
	/// </returns>
	/// <remarks>This operator uses immediate execution.</remarks>

	public static DataTable ToDataTable<T>(this IEnumerable<T> source, params Expression<Func<T, object>>[] expressions)
	{
		return ToDataTable(source, new DataTable(), expressions);
	}

	/// <summary>
	/// Converts a sequence to a <see cref="DataTable"/> object.
	/// </summary>
	/// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">The source.</param>
	/// <returns>
	/// A <see cref="DataTable"/> representing the source.
	/// </returns>
	/// <remarks>This operator uses immediate execution.</remarks>

	public static DataTable ToDataTable<T>(this IEnumerable<T> source)
	{
		return ToDataTable(source, new DataTable(), Array.Empty<Expression<Func<T, object>>>());
	}

	/// <summary>
	/// Appends elements in the sequence as rows of a given <see cref="DataTable"/>
	/// object with a set of lambda expressions specifying which members (property
	/// or field) of each element in the sequence will supply the column values.
	/// </summary>
	/// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <typeparam name="TTable">The type of the input and resulting <see cref="DataTable"/> object.</typeparam>
	/// <param name="source">The source.</param>
	/// <param name="table">The <see cref="DataTable"/> type of object where to add rows</param>
	/// <param name="expressions">Expressions providing access to element members.</param>
	/// <returns>
	/// A <see cref="DataTable"/> or subclass representing the source.
	/// </returns>
	/// <remarks>This operator uses immediate execution.</remarks>

	public static TTable ToDataTable<T, TTable>(this IEnumerable<T> source, TTable table, params Expression<Func<T, object>>[] expressions)
		where TTable : DataTable
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(table);
		Guard.IsNotNull(expressions);

		var members = PrepareMemberInfos(expressions).ToArray();
		members = BuildOrBindSchema(table, members);
		var shredder = CreateShredder<T>(members);

		//
		// Builds rows out of elements in the sequence and
		// add them to the table.
		//

		table.BeginLoadData();

		try
		{
			foreach (var element in source)
			{
				var row = table.NewRow();
				row.ItemArray = shredder(element);
				table.Rows.Add(row);
			}
		}
		finally
		{
			table.EndLoadData();
		}

		return table;
	}

	private static IEnumerable<MemberInfo> PrepareMemberInfos<T>(ICollection<Expression<Func<T, object>>> expressions)
	{
		//
		// If no lambda expressions supplied then reflect them off the source element type.
		//

		if (expressions.Count == 0)
		{
			return from m in typeof(T).GetMembers(BindingFlags.Public | BindingFlags.Instance)
				   where m.MemberType == MemberTypes.Field
					  || (m is PropertyInfo { CanRead: true } p && p.GetIndexParameters().Length == 0)
				   select m;
		}

		//
		// Ensure none of the expressions is <see langword="null"/>.
		//

		if (expressions.Any(e => e == null))
			throw new ArgumentException("One of the supplied expressions was null.", nameof(expressions));
		try
		{
			return expressions.Select(GetAccessedMember);
		}
		catch (ArgumentException e)
		{
			throw new ArgumentException("One of the supplied expressions is not allowed.", nameof(expressions), e);
		}

		static MemberInfo GetAccessedMember(LambdaExpression lambda)
		{
			var body = lambda.Body;

			// If it's a field access, boxing was used, we need the field
			if (body.NodeType is ExpressionType.Convert or ExpressionType.ConvertChecked)
				body = ((UnaryExpression)body).Operand;

			// Check if the member expression is valid and is a "first level"
			// member access e.g. not a.b.c
			return body is MemberExpression memberExpression
				   && memberExpression.Expression?.NodeType == ExpressionType.Parameter
				 ? memberExpression.Member
				 : throw new ArgumentException($"Illegal expression: {lambda}", nameof(lambda));
		}
	}

	/// <remarks>
	/// The resulting array may contain null entries and those represent
	/// columns for which there is no source member supplying a value.
	/// </remarks>

	private static MemberInfo[] BuildOrBindSchema(DataTable table, MemberInfo[] members)
	{
		//
		// Retrieve member information needed to
		// build or validate the table schema.
		//

		var columns = table.Columns;

		var schemas = from m in members
					  let type = m.MemberType == MemberTypes.Property
							   ? ((PropertyInfo)m).PropertyType
							   : ((FieldInfo)m).FieldType
					  select new
					  {
						  Member = m,
						  Type = type.IsGenericType
								 && typeof(Nullable<>) == type.GetGenericTypeDefinition()
							   ? type.GetGenericArguments()[0]
							   : type,
						  Column = columns[m.Name],
					  };

		//
		// If the table has no columns then build the schema.
		// If it has columns then validate members against the columns
		// and re-order members to be aligned with column ordering.
		//

		if (columns.Count == 0)
		{
			columns.AddRange(schemas.Select(m => new DataColumn(m.Member.Name, m.Type)).ToArray());
		}
		else
		{
			members = new MemberInfo[columns.Count];

			foreach (var info in schemas)
			{
				var member = info.Member;
				var column = info.Column;

				if (column == null)
					throw new ArgumentException($"Column named '{member.Name}' is missing.", nameof(table));

				if (info.Type != column.DataType)
					throw new ArgumentException($"Column named '{member.Name}' has wrong data type. It should be {info.Type} when it is {column.DataType}.", nameof(table));

				members[column.Ordinal] = member;
			}
		}

		return members;
	}

	private static Func<T, object[]> CreateShredder<T>(IEnumerable<MemberInfo> members)
	{
		var parameter = Expression.Parameter(typeof(T), "e");

		//
		// It is valid for members sequence to have null entries, in
		// which case a null constant is emitted into the corresponding
		// row values array.
		//

		var initializers = members.Select(m => m != null
											   ? (Expression)CreateMemberAccessor(m)
											   : Expression.Constant(null, typeof(object)));

		var array = Expression.NewArrayInit(typeof(object), initializers);

		var lambda = Expression.Lambda<Func<T, object[]>>(array, parameter);

		return lambda.Compile();

		UnaryExpression CreateMemberAccessor(MemberInfo member)
		{
			var access = Expression.MakeMemberAccess(parameter, member);
			return Expression.Convert(access, typeof(object));
		}
	}
}
