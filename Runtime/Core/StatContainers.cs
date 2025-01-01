using System;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats.Core
{
	public sealed class StatContainerAdd<T, TStat> : StatContainer<T, TStat> where T : struct, IStatModifierData<T> where TStat : IStat<T>
	{
		public StatContainerAdd(TStat stat) : base(stat) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override float Calculate(float currentValue)
		{
			return currentValue + Stat.FinalValue;
		}
	}

	public sealed class StatContainerMult<T, TStat> : StatContainer<T, TStat> where T : struct, IStatModifierData<T> where TStat : IStat<T>
	{
		public StatContainerMult(TStat stat) : base(stat) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override float Calculate(float currentValue)
		{
			return currentValue * Stat.FinalValue;
		}
	}

	public sealed class StatContainerMax<T, TStat> : StatContainer<T, TStat> where T : struct, IStatModifierData<T> where TStat : IStat<T>
	{
		public StatContainerMax(TStat stat) : base(stat) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override float Calculate(float currentValue)
		{
			return Math.Max(currentValue, Stat.FinalValue);
		}
	}

	public sealed class StatContainerMin<T, TStat> : StatContainer<T, TStat> where T : struct, IStatModifierData<T> where TStat : IStat<T>
	{
		public StatContainerMin(TStat stat) : base(stat) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override float Calculate(float currentValue)
		{
			return Math.Min(currentValue, Stat.FinalValue);
		}
	}
}