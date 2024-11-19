using System.Collections.Generic;

namespace Kryz.RPG.Stats.Core
{
	public interface IReadOnlyStat
	{
		float BaseValue { get; }
		float FinalValue { get; }

		int ModifiersCount { get; }
		float GetModifierValue(int index);
	}

	public interface IReadOnlyStat<T> : IReadOnlyStat, IEnumerable<StatModifier<T>> where T : struct, IStatModifierData<T>
	{
		StatModifier<T> GetModifier(int index);
		new StatEnumerator<T> GetEnumerator();
	}

	public interface IStat : IReadOnlyStat
	{
		new float BaseValue { get; set; }

		void Clear();
		void ClearModifiers();
	}

	public interface IStat<T> : IStat, IReadOnlyStat<T> where T : struct, IStatModifierData<T>
	{
		void AddModifier(StatModifier<T> modifier);
		bool RemoveModifier(StatModifier<T> modifier);
		int RemoveWhere<TMatch>(TMatch match) where TMatch : IStatModifierMatch<T>;
	}
}