using System;

namespace Kryz.RPG.Stats4
{
	public readonly struct StatContainer<T, TStat> where T : struct, IStatModifierData where TStat : IStat<T>
	{
		public readonly TStat Stat;

		private readonly Func<float, TStat, float> ApplyFunc;

		public StatContainer(TStat stat, Func<float, TStat, float> applyFunc)
		{
			Stat = stat;
			ApplyFunc = applyFunc;
		}

		public float Apply(float outerStatValue)
		{
			return ApplyFunc(outerStatValue, Stat);
		}
	}
}