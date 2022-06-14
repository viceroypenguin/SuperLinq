using System.Reflection;
using NUnit.Common;
using NUnitLite;

namespace Test;

static class Program
{
	static int Main(string[] args) =>
		new AutoRun(typeof(Program).GetTypeInfo().Assembly)
			.Execute(args, new ExtendedTextWrapper(Console.Out), Console.In);
}
