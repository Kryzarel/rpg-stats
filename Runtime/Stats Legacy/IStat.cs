using System.Collections.Generic;

namespace Kryz.RPG.StatsLegacy
{
	public interface IStat
	{
		float BaseValue { get; set; }
		float FinalValue { get; }

		void Clear();
		void ClearModifiers();
	}

	public interface IStat<T> : IStat where T : struct, IStatModifier
	{
		IReadOnlyList<T> Modifiers { get; }

		void AddModifier(T modifier);
		bool RemoveModifier(T modifier);
		int RemoveModifiersFromSource(object source);
	}
}