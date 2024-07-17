using System.Collections;
using System.Diagnostics;
#if NETCOREAPP
using System.Diagnostics.CodeAnalysis;
#endif
using System.Linq.Expressions;
using System.Reflection;
using CommunityToolkit.Diagnostics;
using Debug = System.Diagnostics.Debug;

namespace Test.Async;

public sealed class NullArgumentTest
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
			var tie = Assert.Throws<TargetInvocationException>(() =>
				method.Invoke(null, args));

			var e = tie.InnerException;
			Assert.NotNull(e);

			var ane = Assert.IsAssignableFrom<ArgumentNullException>(e);
			Assert.Equal(paramName, ane.ParamName);
		});

	public static IEnumerable<object[]> GetCanBeNullInlineDatas() =>
		GetInlineDatas(canBeNull: true, inlineDataFactory: (method, args, paramName) => () =>
		{
			try
			{
				_ = method.Invoke(null, args);
			}
			catch (TargetInvocationException tie)
			{
				Assert.False(
					tie.InnerException is ArgumentNullException ane
					&& !string.Equals(ane.ParamName, paramName, StringComparison.Ordinal));
			}
		});

	private static readonly string[] s_skipMethods =
	[
		nameof(AsyncSuperEnumerable.CopyTo),
	];

	private static IEnumerable<object[]> GetInlineDatas(bool canBeNull, Func<MethodInfo, object[], string, Action> inlineDataFactory) =>
		from m in typeof(AsyncSuperEnumerable).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
		where !s_skipMethods.Contains(m.Name, StringComparer.Ordinal)
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

	private static readonly string[] s_joinMethods =
	[
		nameof(AsyncSuperEnumerable.InnerLoopJoin),
		nameof(AsyncSuperEnumerable.InnerHashJoin),
		nameof(AsyncSuperEnumerable.InnerMergeJoin),
		nameof(AsyncSuperEnumerable.LeftOuterLoopJoin),
		nameof(AsyncSuperEnumerable.LeftOuterHashJoin),
		nameof(AsyncSuperEnumerable.LeftOuterMergeJoin),
		nameof(AsyncSuperEnumerable.RightOuterHashJoin),
		nameof(AsyncSuperEnumerable.RightOuterMergeJoin),
		nameof(AsyncSuperEnumerable.FullOuterHashJoin),
		nameof(AsyncSuperEnumerable.FullOuterMergeJoin),
	];

	private static MethodInfo InstantiateMethod(MethodInfo definition)
	{
		if (!definition.IsGenericMethodDefinition) return definition;

		var typeArguments = definition.GetGenericArguments()
			.Select(t => InstantiateType(t.GetTypeInfo()))
			.ToArray();
		return definition.MakeGenericMethod(typeArguments);

		Type InstantiateType(TypeInfo typeParameter)
		{
			var constraints = typeParameter.GetGenericParameterConstraints();

			if (constraints.Length == 0)
			{
				return s_joinMethods.Contains(definition.Name, StringComparer.Ordinal)
					? typeof(string)
					: typeof(int);
			}

			if (constraints.Length == 1)
				return constraints.Single();

			if (constraints
					.Select(c => c.GetGenericTypeDefinition())
					.CollectionEqual(Seq(typeof(IEqualityComparer<>), typeof(IComparer<>))))
			{
				// for join methods
				return typeof(StringComparer);
			}

			throw new UnreachableException("NullArgumentTest.InstantiateType");
		}
	}

	private static bool IsReferenceType(ParameterInfo parameter) =>
		!parameter.ParameterType.GetTypeInfo().IsValueType;

	private static bool ParameterCanBeNull(ParameterInfo parameter)
	{
		var type = parameter.ParameterType.GetTypeInfo();
		type = type.IsGenericType ? type.GetGenericTypeDefinition().GetTypeInfo() : type;

		if (Seq(typeof(IEqualityComparer<>), typeof(IComparer<>))
				.Select(t => t.GetTypeInfo())
				.Contains(type))
		{
			return true;
		}

		return false;
	}

	private static object CreateInstance(Type type)
	{
		if (type == typeof(int)) return 7; // int is used as size/length/range etc. avoid ArgumentOutOfRange for '0'.
		if (type == typeof(string)) return "";
		if (type == typeof(StringComparer)) return StringComparer.Ordinal;
		if (type == typeof(TaskScheduler)) return TaskScheduler.Default;
		if (type == typeof(IDisposable)) return new Disposable();
		if (type == typeof(IAsyncDisposable)) return new AsyncDisposable();
		if (type.IsArray) return Array.CreateInstance(type.GetElementType()!, 0);
		if (type.GetTypeInfo().IsValueType || HasDefaultConstructor(type)) return Activator.CreateInstance(type)!;
		if (typeof(Delegate).IsAssignableFrom(type)) return CreateDelegateInstance(type);
		if (typeof(Task).IsAssignableFrom(type)) return CreateTaskInstance(type);

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
		Expression body = invoke.ReturnType == typeof(void)
			? Expression.Default(invoke.ReturnType)
			: Expression.Constant(CreateInstance(invoke.ReturnType)); // requires >= .NET 4.0
		var lambda = Expression.Lambda(type, body, parameters);
		return lambda.Compile();
	}

	private static object CreateTaskInstance(Type type)
	{
		var value = CreateInstance(type.GetGenericArguments()[0]);
		var method = typeof(Task).GetMethod("FromResult");
		var instantiation = method!.MakeGenericMethod(type.GetGenericArguments());
		return instantiation!.Invoke(null, [value,])!;
	}

	private static object CreateGenericInterfaceInstance(TypeInfo type)
	{
		Debug.Assert(type.IsGenericType && type.IsInterface);
		var name = type.Name[1..]; // Delete first character, i.e. the 'I' in IEnumerable
		var definition = typeof(GenericArgs).GetTypeInfo().GetNestedType(name);
		var instantiation = definition!.MakeGenericType(type.GetGenericArguments());
		return Activator.CreateInstance(instantiation)!;
	}

	private sealed class Disposable : IDisposable
	{
		public void Dispose() { }
	}

	private sealed class AsyncDisposable : IAsyncDisposable
	{
		public ValueTask DisposeAsync() => default;
	}

	private static class EmptyEnumerable
	{
		public static IEnumerable Instance { get; } = new Enumerable();

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
		private sealed class Enumerator<T> : IEnumerator<T>
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

		public sealed class OrderedEnumerable<T> : Enumerable<T>, IOrderedEnumerable<T>
		{
			public IOrderedEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer, bool descending)
			{
				Guard.IsNotNull(keySelector);
				return this;
			}
		}

		private sealed class AsyncEnumerator<T> : IAsyncEnumerator<T>
		{
			public ValueTask<bool> MoveNextAsync() => new(result: false);
			public T Current { get; private set; } = default!;
			public ValueTask DisposeAsync() => default;
		}

		public class AsyncEnumerable<T> : IAsyncEnumerable<T>
		{
			public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new AsyncEnumerator<T>();
		}

		public sealed class OrderedAsyncEnumerable<T> : AsyncEnumerable<T>, IOrderedAsyncEnumerable<T>
		{
			public IOrderedAsyncEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer, bool descending)
			{
				Guard.IsNotNull(keySelector);
				return this;
			}

			public IOrderedAsyncEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, bool descending)
			{
				Guard.IsNotNull(keySelector);
				return this;
			}

			public IOrderedAsyncEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, bool descending)
			{
				Guard.IsNotNull(keySelector);
				return this;
			}
		}

		public sealed class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>
			where TKey : notnull
		{
			public TValue this[TKey key] { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
#if NETCOREAPP
			public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => throw new NotSupportedException();
#else
			public bool TryGetValue(TKey key, out TValue value) => throw new NotSupportedException();
#endif
			public ICollection<TKey> Keys => throw new NotSupportedException();
			public ICollection<TValue> Values => throw new NotSupportedException();
			public int Count => throw new NotSupportedException();
			public bool IsReadOnly => throw new NotSupportedException();
			public void Add(TKey key, TValue value) => throw new NotSupportedException();
			public void Add(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();
			public void Clear() => throw new NotSupportedException();
			public bool Contains(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();
			public bool ContainsKey(TKey key) => throw new NotSupportedException();
			public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw new NotSupportedException();
			public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => throw new NotSupportedException();
			IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
			public bool Remove(TKey key) => throw new NotSupportedException();
			public bool Remove(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();
		}

		public sealed class Comparer<T> : IComparer<T>
		{
			public int Compare(T? x, T? y) => -1;
		}

		public sealed class EqualityComparer<T> : IEqualityComparer<T>
		{
			public bool Equals(T? x, T? y) => false;
			public int GetHashCode(T obj) => 0;
		}
	}
}
