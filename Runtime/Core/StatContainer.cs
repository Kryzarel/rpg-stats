using System;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats.Core
{
	public abstract class StatContainer<T> where T : struct, IStatModifierData<T>
	{
		public readonly IStat<T> Stat;

		public StatContainer(IStat<T> stat) => Stat = stat;

		public abstract float Calculate(float currentValue);
	}

	public abstract class StatContainer<T, TStat> : StatContainer<T> where T : struct, IStatModifierData<T> where TStat : IStat<T>
	{
		public new readonly TStat Stat;

		public StatContainer(TStat stat) : base(stat) => Stat = stat;
	}
}