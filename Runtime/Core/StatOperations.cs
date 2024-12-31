using System;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats.Core
{
	public readonly struct AddOperation<T> : IStatOperation<T> where T : struct, IStatModifierData<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float Calculate(float outerStatValue, IStat<T> innerStat) => outerStatValue + innerStat.FinalValue;
	}

	public readonly struct MultOperation<T> : IStatOperation<T> where T : struct, IStatModifierData<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float Calculate(float outerStatValue, IStat<T> innerStat) => outerStatValue * innerStat.FinalValue;
	}

	public readonly struct MaxOperation<T> : IStatOperation<T> where T : struct, IStatModifierData<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float Calculate(float outerStatValue, IStat<T> innerStat) => Math.Max(outerStatValue, innerStat.FinalValue);
	}

	public readonly struct MinOperation<T> : IStatOperation<T> where T : struct, IStatModifierData<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float Calculate(float outerStatValue, IStat<T> innerStat) => Math.Min(outerStatValue, innerStat.FinalValue);
	}
}