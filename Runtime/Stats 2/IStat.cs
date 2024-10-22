using System.Collections.Generic;

namespace Kryz.RPG.Stats2
{
	public interface IStat
	{
		float BaseValue { get; set; }
		float FinalValue { get; }
	}

	public interface IStat<T> : IStat where T : struct, IStatModifier
	{
		IReadOnlyList<IReadOnlyStatModifierList<T>> Modifiers { get; }

		bool TryAddModifier(int listIndex, T modifier);
		bool TryRemoveModifier(int listIndex, T modifier);
		int RemoveModifiersFromSource(object source);
	}
}