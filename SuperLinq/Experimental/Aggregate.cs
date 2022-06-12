using SuperLinq.Reactive;

namespace SuperLinq.Experimental;

static partial class ExperimentalEnumerable
{
	static IDisposable SubscribeSingle<T, TResult>(Func<IObservable<T>, IObservable<TResult>> aggregatorSelector, IObservable<T> subject, (bool Defined, TResult)[] r, string paramName)
	{
		var aggregator = aggregatorSelector(subject);
		return ReferenceEquals(aggregator, subject)
			 ? throw new ArgumentException("Aggregator cannot be identical to the source.", paramName)
			 : aggregator.Subscribe(s =>
				 r[0] = r[0].Defined
					 ? throw new InvalidOperationException(
						 $"Aggregator supplied for parameter \"{paramName}\" produced multiple results when only one is allowed.")
					 : (true, s));
	}

	static T GetAggregateResult<T>((bool Defined, T Value) result, string paramName) =>
		!result.Defined
		? throw new InvalidOperationException($"Aggregator supplied for parameter \"{paramName}\" has an undefined result.")
		: result.Value;
}
