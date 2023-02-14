using System.Globalization;

namespace Test;

internal sealed class CurrentThreadCultureScope : Scope<CultureInfo>
{
	public CurrentThreadCultureScope(CultureInfo @new) :
		base(CultureInfo.CurrentCulture)
	{
		Install(@new);
	}

	protected override void Restore(CultureInfo old)
	{
		Install(old);
	}

	private static void Install(CultureInfo value)
	{
		CultureInfo.CurrentCulture = value;
	}
}
