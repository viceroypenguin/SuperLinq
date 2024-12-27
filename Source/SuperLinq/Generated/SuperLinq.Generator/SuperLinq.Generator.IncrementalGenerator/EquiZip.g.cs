namespace SuperLinq;

#nullable enable

public static partial class SuperEnumerable
{
	/// <summary>
	/// <para>
	///	    Applies a specified function to the corresponding elements of second sequences, producing a sequence of the
	///     results.
	/// </para>
	/// <para>
	///	    The resulting sequence has the same length as the input sequences. If the input sequences are of different
	///     lengths, an exception is thrown.
	/// </para>
	/// </summary>
	/// <typeparam name = "TResult">
	///	    The type of the elements of the result sequence.
	/// </typeparam>
	/// <param name = "resultSelector">
	///	    A projection function that combines elements from all of the sequences.
	/// </param>
	/// <returns>
	///	    A sequence of elements returned by <paramref name = "resultSelector"/>.
	/// </returns>
	/// <remarks>
	///	    This method uses deferred execution and stream its results.
	/// </remarks>
	/// <exception cref = "global::System.ArgumentNullException">
	///	    <paramref name = "resultSelector"/> or any of the input sequences is <see langword = "null"/>.
	/// </exception>
	/// <exception cref = "global::System.InvalidOperationException">
	///	    Any of the input sequences are shorter than the others.
	/// </exception>
	/// <typeparam name = "TFirst">
	///		The type of the elements of <paramref name = "first" />.
	/// </typeparam>
	/// <param name = "first">
	///		The first sequence of elements.
	/// </param>
	/// <typeparam name = "TSecond">
	///		The type of the elements of <paramref name = "second" />.
	/// </typeparam>
	/// <param name = "second">
	///		The second sequence of elements.
	/// </param>
	public static global::System.Collections.Generic.IEnumerable<TResult> EquiZip<TFirst, TSecond, TResult>(this 
		global::System.Collections.Generic.IEnumerable<TFirst> first,
		global::System.Collections.Generic.IEnumerable<TSecond> second,
		global::System.Func<TFirst, TSecond, TResult> resultSelector
	)
	{
		ArgumentNullException.ThrowIfNull(first);
		ArgumentNullException.ThrowIfNull(second);

		ArgumentNullException.ThrowIfNull(resultSelector);

		if (
			first is global::System.Collections.Generic.IList<TFirst> list1&&
			second is global::System.Collections.Generic.IList<TSecond> list2)
		{
			return new EquiZipIterator<TFirst, TSecond, TResult>(
				list1,
				list2,
				resultSelector
			);
		}

		return Core(
			first,
			second,
			resultSelector
		);

		static global::System.Collections.Generic.IEnumerable<TResult> Core(
			global::System.Collections.Generic.IEnumerable<TFirst> first,
			global::System.Collections.Generic.IEnumerable<TSecond> second,
			global::System.Func<TFirst, TSecond, TResult> resultSelector
		)
		{
			using var e1 = first.GetEnumerator();
			using var e2 = second.GetEnumerator();

			while (true)
			{
				if (!e1.MoveNext())
				{
					if (e2.MoveNext()
					)
					{
						ThrowHelper.ThrowInvalidOperationException("First sequence too short.");
					}

					yield break;
				}

				if (!e2.MoveNext())
					ThrowHelper.ThrowInvalidOperationException(
						"Second sequence too short.");

				yield return resultSelector(
					e1.Current,
					e2.Current
				);
			}
		}
	}

	/// <summary>
	///	    Joins the corresponding elements of second sequences, producing a sequence of tuples containing them.
	/// </summary>
	/// <returns>
	///	    A sequence of <see cref="global::System.ValueTuple{T1,T2 }" /> 
	///	    containing corresponding elements from each of the sequences.
	/// </returns>
	/// <remarks>
	///	    This method uses deferred execution and stream its results.
	/// </remarks>
	/// <exception cref = "global::System.ArgumentNullException">
	/// Any of the input sequences is null.
	///	    </exception>
	/// <exception cref = "global::System.InvalidOperationException">
	///	    Any of the input sequences are shorter than the others.
	/// </exception>
	/// <typeparam name="TFirst">
	///     The type of the elements of <paramref name="first" />.
	/// </typeparam>
	/// <param name="first">
	///     The first sequence of elements.
	/// </param>
	/// <typeparam name="TSecond">
	///     The type of the elements of <paramref name="second" />.
	/// </typeparam>
	/// <param name="second">
	///     The second sequence of elements.
	/// </param>
	public static global::System.Collections.Generic.IEnumerable<(TFirst,TSecond)> 
		EquiZip<TFirst,TSecond>(this
			global::System.Collections.Generic.IEnumerable<TFirst> first,
			global::System.Collections.Generic.IEnumerable<TSecond> second
	) => EquiZip(first, second, global::System.ValueTuple.Create);

	private sealed class EquiZipIterator<T1, T2, TResult> : ListIterator<TResult>
	{
		private readonly global::System.Collections.Generic.IList<T1> _list1;
		private readonly global::System.Collections.Generic.IList<T2> _list2;
		private readonly global::System.Func<T1, T2, TResult> _resultSelector;
		
		public EquiZipIterator(
			global::System.Collections.Generic.IList<T1> first,
			global::System.Collections.Generic.IList<T2> second,
			global::System.Func<T1, T2, TResult> resultSelector
		)
		{
			_list1 = first;
			_list2 = second;
			_resultSelector	= resultSelector;
		}

		public override int Count
		{
			get
			{
				var count = _list1.Count;

				if (_list2.Count != count)
				{
					ThrowHelper.ThrowInvalidOperationException(
						(count < _list2.Count ? "First" : "Second") + " sequence too short.");
				}

				return count;
			}
		}

		protected override IEnumerable<TResult> GetEnumerable()
		{
			var cnt = (uint)Count;
			for (var i = 0; i < cnt; i++)
			{
				yield return _resultSelector(
					_list1[i], 
					_list2[i]
				);
			}
		}

		protected override TResult ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			return _resultSelector(
				_list1[index], 
				_list2[index]
			);
		}
	}

	/// <summary>
	/// <para>
	///	    Applies a specified function to the corresponding elements of second sequences, producing a sequence of the
	///     results.
	/// </para>
	/// <para>
	///	    The resulting sequence has the same length as the input sequences. If the input sequences are of different
	///     lengths, an exception is thrown.
	/// </para>
	/// </summary>
	/// <typeparam name = "TResult">
	///	    The type of the elements of the result sequence.
	/// </typeparam>
	/// <param name = "resultSelector">
	///	    A projection function that combines elements from all of the sequences.
	/// </param>
	/// <returns>
	///	    A sequence of elements returned by <paramref name = "resultSelector"/>.
	/// </returns>
	/// <remarks>
	///	    This method uses deferred execution and stream its results.
	/// </remarks>
	/// <exception cref = "global::System.ArgumentNullException">
	///	    <paramref name = "resultSelector"/> or any of the input sequences is <see langword = "null"/>.
	/// </exception>
	/// <exception cref = "global::System.InvalidOperationException">
	///	    Any of the input sequences are shorter than the others.
	/// </exception>
	/// <typeparam name = "TFirst">
	///		The type of the elements of <paramref name = "first" />.
	/// </typeparam>
	/// <param name = "first">
	///		The first sequence of elements.
	/// </param>
	/// <typeparam name = "TSecond">
	///		The type of the elements of <paramref name = "second" />.
	/// </typeparam>
	/// <param name = "second">
	///		The second sequence of elements.
	/// </param>
	/// <typeparam name = "TThird">
	///		The type of the elements of <paramref name = "third" />.
	/// </typeparam>
	/// <param name = "third">
	///		The third sequence of elements.
	/// </param>
	public static global::System.Collections.Generic.IEnumerable<TResult> EquiZip<TFirst, TSecond, TThird, TResult>(this 
		global::System.Collections.Generic.IEnumerable<TFirst> first,
		global::System.Collections.Generic.IEnumerable<TSecond> second,
		global::System.Collections.Generic.IEnumerable<TThird> third,
		global::System.Func<TFirst, TSecond, TThird, TResult> resultSelector
	)
	{
		ArgumentNullException.ThrowIfNull(first);
		ArgumentNullException.ThrowIfNull(second);
		ArgumentNullException.ThrowIfNull(third);

		ArgumentNullException.ThrowIfNull(resultSelector);

		if (
			first is global::System.Collections.Generic.IList<TFirst> list1&&
			second is global::System.Collections.Generic.IList<TSecond> list2&&
			third is global::System.Collections.Generic.IList<TThird> list3)
		{
			return new EquiZipIterator<TFirst, TSecond, TThird, TResult>(
				list1,
				list2,
				list3,
				resultSelector
			);
		}

		return Core(
			first,
			second,
			third,
			resultSelector
		);

		static global::System.Collections.Generic.IEnumerable<TResult> Core(
			global::System.Collections.Generic.IEnumerable<TFirst> first,
			global::System.Collections.Generic.IEnumerable<TSecond> second,
			global::System.Collections.Generic.IEnumerable<TThird> third,
			global::System.Func<TFirst, TSecond, TThird, TResult> resultSelector
		)
		{
			using var e1 = first.GetEnumerator();
			using var e2 = second.GetEnumerator();
			using var e3 = third.GetEnumerator();

			while (true)
			{
				if (!e1.MoveNext())
				{
					if (e2.MoveNext()
						|| e3.MoveNext()
					)
					{
						ThrowHelper.ThrowInvalidOperationException("First sequence too short.");
					}

					yield break;
				}

				if (!e2.MoveNext())
					ThrowHelper.ThrowInvalidOperationException(
						"Second sequence too short.");
				if (!e3.MoveNext())
					ThrowHelper.ThrowInvalidOperationException(
						"Third sequence too short.");

				yield return resultSelector(
					e1.Current,
					e2.Current,
					e3.Current
				);
			}
		}
	}

	/// <summary>
	///	    Joins the corresponding elements of second sequences, producing a sequence of tuples containing them.
	/// </summary>
	/// <returns>
	///	    A sequence of <see cref="global::System.ValueTuple{T1,T2,T3 }" /> 
	///	    containing corresponding elements from each of the sequences.
	/// </returns>
	/// <remarks>
	///	    This method uses deferred execution and stream its results.
	/// </remarks>
	/// <exception cref = "global::System.ArgumentNullException">
	/// Any of the input sequences is null.
	///	    </exception>
	/// <exception cref = "global::System.InvalidOperationException">
	///	    Any of the input sequences are shorter than the others.
	/// </exception>
	/// <typeparam name="TFirst">
	///     The type of the elements of <paramref name="first" />.
	/// </typeparam>
	/// <param name="first">
	///     The first sequence of elements.
	/// </param>
	/// <typeparam name="TSecond">
	///     The type of the elements of <paramref name="second" />.
	/// </typeparam>
	/// <param name="second">
	///     The second sequence of elements.
	/// </param>
	/// <typeparam name="TThird">
	///     The type of the elements of <paramref name="third" />.
	/// </typeparam>
	/// <param name="third">
	///     The third sequence of elements.
	/// </param>
	public static global::System.Collections.Generic.IEnumerable<(TFirst,TSecond,TThird)> 
		EquiZip<TFirst,TSecond,TThird>(this
			global::System.Collections.Generic.IEnumerable<TFirst> first,
			global::System.Collections.Generic.IEnumerable<TSecond> second,
			global::System.Collections.Generic.IEnumerable<TThird> third
	) => EquiZip(first, second, third, global::System.ValueTuple.Create);

	private sealed class EquiZipIterator<T1, T2, T3, TResult> : ListIterator<TResult>
	{
		private readonly global::System.Collections.Generic.IList<T1> _list1;
		private readonly global::System.Collections.Generic.IList<T2> _list2;
		private readonly global::System.Collections.Generic.IList<T3> _list3;
		private readonly global::System.Func<T1, T2, T3, TResult> _resultSelector;
		
		public EquiZipIterator(
			global::System.Collections.Generic.IList<T1> first,
			global::System.Collections.Generic.IList<T2> second,
			global::System.Collections.Generic.IList<T3> third,
			global::System.Func<T1, T2, T3, TResult> resultSelector
		)
		{
			_list1 = first;
			_list2 = second;
			_list3 = third;
			_resultSelector	= resultSelector;
		}

		public override int Count
		{
			get
			{
				var count = _list1.Count;

				if (_list2.Count != count)
				{
					ThrowHelper.ThrowInvalidOperationException(
						(count < _list2.Count ? "First" : "Second") + " sequence too short.");
				}
				if (_list3.Count != count)
				{
					ThrowHelper.ThrowInvalidOperationException(
						(count < _list3.Count ? "First" : "Third") + " sequence too short.");
				}

				return count;
			}
		}

		protected override IEnumerable<TResult> GetEnumerable()
		{
			var cnt = (uint)Count;
			for (var i = 0; i < cnt; i++)
			{
				yield return _resultSelector(
					_list1[i], 
					_list2[i], 
					_list3[i]
				);
			}
		}

		protected override TResult ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			return _resultSelector(
				_list1[index], 
				_list2[index], 
				_list3[index]
			);
		}
	}

	/// <summary>
	/// <para>
	///	    Applies a specified function to the corresponding elements of second sequences, producing a sequence of the
	///     results.
	/// </para>
	/// <para>
	///	    The resulting sequence has the same length as the input sequences. If the input sequences are of different
	///     lengths, an exception is thrown.
	/// </para>
	/// </summary>
	/// <typeparam name = "TResult">
	///	    The type of the elements of the result sequence.
	/// </typeparam>
	/// <param name = "resultSelector">
	///	    A projection function that combines elements from all of the sequences.
	/// </param>
	/// <returns>
	///	    A sequence of elements returned by <paramref name = "resultSelector"/>.
	/// </returns>
	/// <remarks>
	///	    This method uses deferred execution and stream its results.
	/// </remarks>
	/// <exception cref = "global::System.ArgumentNullException">
	///	    <paramref name = "resultSelector"/> or any of the input sequences is <see langword = "null"/>.
	/// </exception>
	/// <exception cref = "global::System.InvalidOperationException">
	///	    Any of the input sequences are shorter than the others.
	/// </exception>
	/// <typeparam name = "TFirst">
	///		The type of the elements of <paramref name = "first" />.
	/// </typeparam>
	/// <param name = "first">
	///		The first sequence of elements.
	/// </param>
	/// <typeparam name = "TSecond">
	///		The type of the elements of <paramref name = "second" />.
	/// </typeparam>
	/// <param name = "second">
	///		The second sequence of elements.
	/// </param>
	/// <typeparam name = "TThird">
	///		The type of the elements of <paramref name = "third" />.
	/// </typeparam>
	/// <param name = "third">
	///		The third sequence of elements.
	/// </param>
	/// <typeparam name = "TFourth">
	///		The type of the elements of <paramref name = "fourth" />.
	/// </typeparam>
	/// <param name = "fourth">
	///		The fourth sequence of elements.
	/// </param>
	public static global::System.Collections.Generic.IEnumerable<TResult> EquiZip<TFirst, TSecond, TThird, TFourth, TResult>(this 
		global::System.Collections.Generic.IEnumerable<TFirst> first,
		global::System.Collections.Generic.IEnumerable<TSecond> second,
		global::System.Collections.Generic.IEnumerable<TThird> third,
		global::System.Collections.Generic.IEnumerable<TFourth> fourth,
		global::System.Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector
	)
	{
		ArgumentNullException.ThrowIfNull(first);
		ArgumentNullException.ThrowIfNull(second);
		ArgumentNullException.ThrowIfNull(third);
		ArgumentNullException.ThrowIfNull(fourth);

		ArgumentNullException.ThrowIfNull(resultSelector);

		if (
			first is global::System.Collections.Generic.IList<TFirst> list1&&
			second is global::System.Collections.Generic.IList<TSecond> list2&&
			third is global::System.Collections.Generic.IList<TThird> list3&&
			fourth is global::System.Collections.Generic.IList<TFourth> list4)
		{
			return new EquiZipIterator<TFirst, TSecond, TThird, TFourth, TResult>(
				list1,
				list2,
				list3,
				list4,
				resultSelector
			);
		}

		return Core(
			first,
			second,
			third,
			fourth,
			resultSelector
		);

		static global::System.Collections.Generic.IEnumerable<TResult> Core(
			global::System.Collections.Generic.IEnumerable<TFirst> first,
			global::System.Collections.Generic.IEnumerable<TSecond> second,
			global::System.Collections.Generic.IEnumerable<TThird> third,
			global::System.Collections.Generic.IEnumerable<TFourth> fourth,
			global::System.Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector
		)
		{
			using var e1 = first.GetEnumerator();
			using var e2 = second.GetEnumerator();
			using var e3 = third.GetEnumerator();
			using var e4 = fourth.GetEnumerator();

			while (true)
			{
				if (!e1.MoveNext())
				{
					if (e2.MoveNext()
						|| e3.MoveNext()
						|| e4.MoveNext()
					)
					{
						ThrowHelper.ThrowInvalidOperationException("First sequence too short.");
					}

					yield break;
				}

				if (!e2.MoveNext())
					ThrowHelper.ThrowInvalidOperationException(
						"Second sequence too short.");
				if (!e3.MoveNext())
					ThrowHelper.ThrowInvalidOperationException(
						"Third sequence too short.");
				if (!e4.MoveNext())
					ThrowHelper.ThrowInvalidOperationException(
						"Fourth sequence too short.");

				yield return resultSelector(
					e1.Current,
					e2.Current,
					e3.Current,
					e4.Current
				);
			}
		}
	}

	/// <summary>
	///	    Joins the corresponding elements of second sequences, producing a sequence of tuples containing them.
	/// </summary>
	/// <returns>
	///	    A sequence of <see cref="global::System.ValueTuple{T1,T2,T3,T4 }" /> 
	///	    containing corresponding elements from each of the sequences.
	/// </returns>
	/// <remarks>
	///	    This method uses deferred execution and stream its results.
	/// </remarks>
	/// <exception cref = "global::System.ArgumentNullException">
	/// Any of the input sequences is null.
	///	    </exception>
	/// <exception cref = "global::System.InvalidOperationException">
	///	    Any of the input sequences are shorter than the others.
	/// </exception>
	/// <typeparam name="TFirst">
	///     The type of the elements of <paramref name="first" />.
	/// </typeparam>
	/// <param name="first">
	///     The first sequence of elements.
	/// </param>
	/// <typeparam name="TSecond">
	///     The type of the elements of <paramref name="second" />.
	/// </typeparam>
	/// <param name="second">
	///     The second sequence of elements.
	/// </param>
	/// <typeparam name="TThird">
	///     The type of the elements of <paramref name="third" />.
	/// </typeparam>
	/// <param name="third">
	///     The third sequence of elements.
	/// </param>
	/// <typeparam name="TFourth">
	///     The type of the elements of <paramref name="fourth" />.
	/// </typeparam>
	/// <param name="fourth">
	///     The fourth sequence of elements.
	/// </param>
	public static global::System.Collections.Generic.IEnumerable<(TFirst,TSecond,TThird,TFourth)> 
		EquiZip<TFirst,TSecond,TThird,TFourth>(this
			global::System.Collections.Generic.IEnumerable<TFirst> first,
			global::System.Collections.Generic.IEnumerable<TSecond> second,
			global::System.Collections.Generic.IEnumerable<TThird> third,
			global::System.Collections.Generic.IEnumerable<TFourth> fourth
	) => EquiZip(first, second, third, fourth, global::System.ValueTuple.Create);

	private sealed class EquiZipIterator<T1, T2, T3, T4, TResult> : ListIterator<TResult>
	{
		private readonly global::System.Collections.Generic.IList<T1> _list1;
		private readonly global::System.Collections.Generic.IList<T2> _list2;
		private readonly global::System.Collections.Generic.IList<T3> _list3;
		private readonly global::System.Collections.Generic.IList<T4> _list4;
		private readonly global::System.Func<T1, T2, T3, T4, TResult> _resultSelector;
		
		public EquiZipIterator(
			global::System.Collections.Generic.IList<T1> first,
			global::System.Collections.Generic.IList<T2> second,
			global::System.Collections.Generic.IList<T3> third,
			global::System.Collections.Generic.IList<T4> fourth,
			global::System.Func<T1, T2, T3, T4, TResult> resultSelector
		)
		{
			_list1 = first;
			_list2 = second;
			_list3 = third;
			_list4 = fourth;
			_resultSelector	= resultSelector;
		}

		public override int Count
		{
			get
			{
				var count = _list1.Count;

				if (_list2.Count != count)
				{
					ThrowHelper.ThrowInvalidOperationException(
						(count < _list2.Count ? "First" : "Second") + " sequence too short.");
				}
				if (_list3.Count != count)
				{
					ThrowHelper.ThrowInvalidOperationException(
						(count < _list3.Count ? "First" : "Third") + " sequence too short.");
				}
				if (_list4.Count != count)
				{
					ThrowHelper.ThrowInvalidOperationException(
						(count < _list4.Count ? "First" : "Fourth") + " sequence too short.");
				}

				return count;
			}
		}

		protected override IEnumerable<TResult> GetEnumerable()
		{
			var cnt = (uint)Count;
			for (var i = 0; i < cnt; i++)
			{
				yield return _resultSelector(
					_list1[i], 
					_list2[i], 
					_list3[i], 
					_list4[i]
				);
			}
		}

		protected override TResult ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			return _resultSelector(
				_list1[index], 
				_list2[index], 
				_list3[index], 
				_list4[index]
			);
		}
	}

}
