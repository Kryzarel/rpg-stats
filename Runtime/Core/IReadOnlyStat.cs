using System.Collections.Generic;

namespace Kryz.RPG.Stats.Core
{
	public interface IReadOnlyStat
	{
		float BaseValue { get; }
		float FinalValue { get; }

		int ModifiersCount { get; }
		float this[int index] { get; }
	}

	public partial interface IReadOnlyStat<T> : IReadOnlyStat, IEnumerable<StatModifier<T>> where T : struct, IStatModifierData<T>
	{
		new StatModifier<T> this[int index] { get; }
		new Enumerator GetEnumerator();
	}
}