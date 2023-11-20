#if !NET6_0_OR_GREATER
global using ArgumentNullException = SuperLinq.Exceptions.ArgumentNullException;
#endif

#if !NET8_0_OR_GREATER
global using ArgumentOutOfRangeException = SuperLinq.Exceptions.ArgumentOutOfRangeException;
#endif
