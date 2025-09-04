using System;
using System.Collections.Generic;

namespace Kryz.RPG.Stats.Core
{
	public interface IStat : IReadOnlyStat
	{
		new float BaseValue { get; set; }
		new IReadOnlyList<IStat> Stats { get; }

		void Clear();
		void ClearModifiers();
	}

	public interface IStat<T> : IStat, IReadOnlyStat<T> where T : struct, IStatModifierData<T>
	{
		new IReadOnlyList<IStat<T>> Stats { get; }

		void AddModifier(StatModifier<T> modifier);
		bool RemoveModifier(StatModifier<T> modifier);
		int RemoveAll<TEquatable>(TEquatable match) where TEquatable : IEquatable<StatModifier<T>>;
	}
}