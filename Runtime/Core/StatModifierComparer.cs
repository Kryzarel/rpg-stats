using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats.Core
{
	public readonly struct StatModifierComparer<T> : IComparer<StatModifier<T>> where T : struct, IStatModifierData<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly int Compare(StatModifier<T> x, StatModifier<T> y) => x.CompareTo(y);
	}
}