namespace SuperLinq.Tests;

public static class Combinatorics
{
	public static double Factorial(int n) =>
		Enumerable.Range(1, n)
			.Aggregate(1.0d, (a, b) => a * b);

	public static double Binomial(int n, int k) =>
		Factorial(n) / (Factorial(n - k) * Factorial(k));
}
