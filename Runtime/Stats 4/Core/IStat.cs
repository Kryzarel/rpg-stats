using System;
using System.Collections.Generic;

namespace Kryz.RPG.Stats4
{
	public interface IReadOnlyStat
	{
		float BaseValue { get; }
		float FinalValue { get; }
		IReadOnlyList<float> ModifierValues { get; }

		event Action<IReadOnlyStat, float> OnValueChanged;
	}

	public interface IReadOnlyStat<T> : IReadOnlyStat where T : struct, IStatModifierData
	{
		IReadOnlyList<T> ModifierDatas { get; }
	}

	public interface IStat : IReadOnlyStat
	{
		new float BaseValue { get; set; }

		void Clear();
		void ClearModifiers();
	}

	public interface IStat<T> : IStat, IReadOnlyStat<T> where T : struct, IStatModifierData
	{
		void AddModifier(float modifierValue, T data);
		bool RemoveModifier(float modifierValue, T data);
		int RemoveWhere<TMatch>(TMatch match) where TMatch : IStatModifierMatch<T>;
	}
}