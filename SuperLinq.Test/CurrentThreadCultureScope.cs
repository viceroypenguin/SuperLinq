using System.Globalization;

namespace SuperLinq.Test;

sealed class CurrentThreadCultureScope : Scope<CultureInfo>
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

	static void Install(CultureInfo value)
	{
#if NET451
            System.Threading.Thread.CurrentThread.CurrentCulture = value;
#else
		CultureInfo.CurrentCulture = value;
#endif
	}
}
