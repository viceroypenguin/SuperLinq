using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Test;

public class NullArgumentTest
{
	[Theory, MemberData(nameof(GetNotNullInlineDatas))]
	public void NotNull(Action inlineData) =>
		inlineData();

	[Theory, MemberData(nameof(GetCanBeNullInlineDatas))]
	public void CanBeNull(Action inlineData) =>
		inlineData();

	public static IEnumerable<object[]> GetNotNullInlineDatas() =>
		GetInlineDatas(canBeNull: false, inlineDataFactory: (method, args, paramName) => () =>
		{
			Exception? e = null;

			try
			{
				method.Invoke(null, args);
			}
			catch (TargetInvocationException tie)
			{
				e = tie.InnerException;
			}

			Assert.NotNull(e);
			Assert.IsAssignableFrom<ArgumentNullException>(e);
			var ane = (ArgumentNullException)e!;
			Assert.Equal(paramName, ane.ParamName);
		});

	public static IEnumerable<object[]> GetCanBeNullInlineDatas() =>
		GetInlineDatas(canBeNull: true, inlineDataFactory: (method, args, _) => () => method.Invoke(null, args));

	private static readonly string[] s_skipMethods =
	{
		nameof(SuperEnumerable.GetShortestPath),
		nameof(SuperEnumerable.GetShortestPathCost),
		nameof(SuperEnumerable.GetShortestPaths),
	};

	private static IEnumerable<object[]> GetInlineDatas(bool canBeNull, Func<MethodInfo, object[], string, Action> inlineDataFactory) =>
		from m in typeof(SuperEnumerable).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
		where !s_skipMethods.Contains(m.Name)
		from t in CreateInlineDatas(m, canBeNull, inlineDataFactory)
		select t;

	private static IEnumerable<object[]> CreateInlineDatas(MethodInfo methodDefinition, bool canBeNull, Func<MethodInfo, object[], string, Action> inlineDataFactory)
	{
		var method = InstantiateMethod(methodDefinition);
		var parameters = method.GetParameters().ToList();

		return from param in parameters
			   where IsReferenceType(param) && ParameterCanBeNull(param) == canBeNull
			   let arguments = parameters.Select(p => p == param ? null : CreateInstance(p.ParameterType)).ToArray()
			   let InlineData = inlineDataFactory(method, arguments, param.Name!)
			   let testName = GetTestName(methodDefinition, param)
			   select new object[] { InlineData };
	}

	private static string GetTestName(MethodInfo definition, ParameterInfo parameter) =>
		$"{definition.Name}: '{parameter.Name}' ({parameter.Position});\n{definition}";

	private static MethodInfo InstantiateMethod(MethodInfo definition)
	{
		if (!definition.IsGenericMethodDefinition) return definition;

		var typeArguments = definition.GetGenericArguments().Select(t => InstantiateType(t.GetTypeInfo())).ToArray();
		return definition.MakeGenericMethod(typeArguments);
	}

	private static Type InstantiateType(TypeInfo typeParameter)
	{
		var constraints = typeParameter.GetGenericParameterConstraints();

		if (constraints.Length == 0) return typeof(int);
		if (constraints.Length == 1) return constraints.Single();

		throw new NotImplementedException("NullArgumentTest.InstantiateType");
	}

	private static bool IsReferenceType(ParameterInfo parameter) =>
		!parameter.ParameterType.GetTypeInfo().IsValueType;

	private static bool ParameterCanBeNull(ParameterInfo parameter)
	{
		var nullableTypes =
			from t in new[] { typeof(IEqualityComparer<>), typeof(IComparer<>) }
			select t.GetTypeInfo();

		var nullableParameters = new[]
		{
			nameof(SuperEnumerable.Trace) + ".format",
		};

		var type = parameter.ParameterType.GetTypeInfo();
		type = type.IsGenericType ? type.GetGenericTypeDefinition().GetTypeInfo() : type;
		var param = parameter.Member.Name + "." + parameter.Name;

		return nullableTypes.Contains(type) || nullableParameters.Contains(param, StringComparer.OrdinalIgnoreCase);
	}

	private static object CreateInstance(Type type)
	{
		if (type == typeof(int)) return 7; // int is used as size/length/range etc. avoid ArgumentOutOfRange for '0'.
		if (type == typeof(string)) return "";
		if (type == typeof(TaskScheduler)) return TaskScheduler.Default;
		if (type.IsArray) return Array.CreateInstance(type.GetElementType()!, 0);
		if (type.GetTypeInfo().IsValueType || HasDefaultConstructor(type)) return Activator.CreateInstance(type)!;
		if (typeof(Delegate).IsAssignableFrom(type)) return CreateDelegateInstance(type);

		var typeInfo = type.GetTypeInfo();

		return typeInfo.IsGenericType
				? CreateGenericInterfaceInstance(typeInfo)
				: EmptyEnumerable.Instance;
	}

	private static bool HasDefaultConstructor(Type type) =>
		type.GetConstructor(Type.EmptyTypes) != null;

	private static Delegate CreateDelegateInstance(Type type)
	{
		var invoke = type.GetMethod("Invoke");
		var parameters = invoke!.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name));
		var body = Expression.Default(invoke.ReturnType); // requires >= .NET 4.0
		var lambda = Expression.Lambda(type, body, parameters);
		return lambda.Compile();
	}

	private static object CreateGenericInterfaceInstance(TypeInfo type)
	{
		Debug.Assert(type.IsGenericType && type.IsInterface);
		var name = type.Name[1..]; // Delete first character, i.e. the 'I' in IEnumerable
		var definition = typeof(GenericArgs).GetTypeInfo().GetNestedType(name);
		var instantiation = definition!.MakeGenericType(type.GetGenericArguments());
		return Activator.CreateInstance(instantiation)!;
	}

	private static class EmptyEnumerable
	{
		public static readonly IEnumerable Instance = new Enumerable();

		private sealed class Enumerable : IEnumerable
		{
			public IEnumerator GetEnumerator() => new Enumerator();

			private sealed class Enumerator : IEnumerator
			{
				public bool MoveNext() => false;
				object IEnumerator.Current => throw new InvalidOperationException();
				public void Reset() { }
			}
		}
	}

	private static class GenericArgs
	{
		private class Enumerator<T> : IEnumerator<T>
		{
			public bool MoveNext() => false;
			public T Current { get; private set; } = default!;
			object? IEnumerator.Current => Current;
			public void Reset() { }
			public void Dispose() { }
		}

		public class Enumerable<T> : IEnumerable<T>
		{
			public IEnumerator<T> GetEnumerator() => new Enumerator<T>();
			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		public class OrderedEnumerable<T> : Enumerable<T>, IOrderedEnumerable<T>
		{
			public IOrderedEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer, bool descending)
			{
				if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
				return this;
			}
		}

		public class Comparer<T> : IComparer<T>
		{
			public int Compare(T? x, T? y) => -1;
		}

		public class EqualityComparer<T> : IEqualityComparer<T>
		{
			public bool Equals(T? x, T? y) => false;
			public int GetHashCode(T obj) => 0;
		}
	}
}
