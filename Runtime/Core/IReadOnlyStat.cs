using System;
using System.Collections.Generic;

namespace Kryz.RPG.Stats.Core
{
	public interface IReadOnlyStat
	{
		event Action? OnValueChanged;

		IReadOnlyList<IReadOnlyStat> Stats { get; }

		float BaseValue { get; }
		float FinalValue { get; }
	}

	public interface IReadOnlyStat<T> : IReadOnlyStat where T : struct, IStatModifierData<T>
	{
		new IReadOnlyList<IReadOnlyStat<T>> Stats { get; }

		IReadOnlyList<StatModifier<T>> Modifiers { get; }
	}
}