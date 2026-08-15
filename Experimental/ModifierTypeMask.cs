using System;

namespace Experimental
{
	[Flags]
	public enum ModifierTypeMask
	{
		None = 0,
		All = ~0,

		Flat = 1 << ModifierType.Flat,
		PercentAdd = 1 << ModifierType.PercentAdd,
		PercentMul = 1 << ModifierType.PercentMul,
		Min = 1 << ModifierType.Min,
		Max = 1 << ModifierType.Max,
	}
}