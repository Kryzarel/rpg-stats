using System.Collections.Generic;

namespace Kryz.RPG.Stats.Core
{
	public interface IReadOnlyStat
	{
		float BaseValue { get; }
		float FinalValue { get; }

		IReadOnlyList<IReadOnlyStat> Stats { get; }
	}

	public interface IReadOnlyStat<T> : IReadOnlyStat where T : struct, IStatModifierData<T>
	{
		new IReadOnlyList<IReadOnlyStat<T>> Stats { get; }

		IReadOnlyList<StatModifier<T>> Modifiers { get; }
	}
}