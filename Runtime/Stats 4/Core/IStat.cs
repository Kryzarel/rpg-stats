using System;
using System.Collections.Generic;

namespace Kryz.RPG.Stats4
{
	public interface IStat
	{
		float BaseValue { get; set; }
		float FinalValue { get; }
		IReadOnlyList<IStatModifierList> Modifiers { get; }

		void Clear();
		void ClearModifiers();
	}

	public interface IStat<T> : IStat where T : struct, IStatModifierMetaData
	{
		new IReadOnlyList<IReadOnlyStatModifierList<T>> Modifiers { get; }

		void AddModifier(float modifierValue, T metaData);
		bool RemoveModifier(float modifierValue, T metaData);
		int RemoveWhere(Func<float, T, bool> predicate);
	}
}