using System.Collections.Generic;

namespace Kryz.RPG.Stats3
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
		IReadOnlyList<IReadOnlyStatModifierList<T>> Modifiers { get; }

		bool TryAddModifier(int listIndex, T modifier);
		bool TryRemoveModifier(int listIndex, T modifier);
		int RemoveModifiersFromSource(object source);
	}
}