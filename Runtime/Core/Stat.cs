using System;
using System.Collections.Generic;

namespace Kryz.RPG.Stats.Core
{
	public abstract class Stat<T> : IStat<T> where T : struct, IStatModifierData<T>
	{
		protected readonly IStat<T>[] stats;
		private readonly float[] cachedValues;

		private float baseValue;
		private float finalValue;

		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(changed: true); } }
		public float FinalValue { get { CalculateFinalValue(); return finalValue; } }

		public IReadOnlyList<IStat<T>> Stats => stats;

		IReadOnlyList<IStat> IStat.Stats => stats;
		IReadOnlyList<IReadOnlyStat<T>> IReadOnlyStat<T>.Stats => stats;
		IReadOnlyList<IReadOnlyStat> IReadOnlyStat.Stats => stats;

		IReadOnlyList<StatModifier<T>> IReadOnlyStat<T>.Modifiers => Array.Empty<StatModifier<T>>();

		protected Stat(float baseValue = 0, params IStat<T>[] stats)
		{
			cachedValues = new float[stats.Length];
			this.stats = stats;
			this.baseValue = baseValue;
			CalculateFinalValue();
		}

		public abstract void AddModifier(StatModifier<T> modifier);
		public abstract bool RemoveModifier(StatModifier<T> modifier);
		protected abstract float CalculateFinalValue(float baseValue);

		private void CalculateFinalValue(bool changed = false)
		{
			for (int i = 0; i < cachedValues.Length; i++)
			{
				float value = stats[i].FinalValue;
				if (cachedValues[i] != value)
				{
					cachedValues[i] = value;
					changed = true;
				}
			}

			if (changed)
			{
				finalValue = CalculateFinalValue(baseValue);
			}
		}

		public int RemoveAll<TMatch>(TMatch match) where TMatch : IEquatable<StatModifier<T>>
		{
			int removedCount = 0;
			for (int i = 0; i < stats.Length; i++)
			{
				removedCount += stats[i].RemoveAll(match);
			}
			return removedCount;
		}

		public void Clear()
		{
			baseValue = 0;
			ClearModifiers();
		}

		public void ClearModifiers()
		{
			for (int i = 0; i < stats.Length; i++)
			{
				stats[i].ClearModifiers();
			}
			CalculateFinalValue(changed: true);
		}
	}
}