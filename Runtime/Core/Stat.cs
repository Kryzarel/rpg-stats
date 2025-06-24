using System;
using System.Collections;
using System.Collections.Generic;

namespace Kryz.RPG.Stats.Core
{
	public abstract partial class Stat<T> : IStat<T> where T : struct, IStatModifierData<T>
	{
		protected readonly IStat<T>[] innerStats;
		private readonly float[] cachedValues;

		private float baseValue;
		private float finalValue;

		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(changed: true); } }
		public float FinalValue { get { CalculateFinalValue(); return finalValue; } }
		public int ModifiersCount => SumCounts();

		protected Stat(float baseValue = 0, params IStat<T>[] innerStats)
		{
			cachedValues = new float[innerStats.Length];
			this.innerStats = innerStats;
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
				float value = innerStats[i].FinalValue;
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
			for (int i = 0; i < innerStats.Length; i++)
			{
				removedCount += innerStats[i].RemoveAll(match);
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
			for (int i = 0; i < innerStats.Length; i++)
			{
				innerStats[i].ClearModifiers();
			}
			CalculateFinalValue(changed: true);
		}

		private int SumCounts()
		{
			int count = 0;
			for (int i = 0; i < innerStats.Length; i++)
			{
				count += innerStats[i].ModifiersCount;
			}
			return count;
		}

		private StatModifier<T> GetModifier(int index)
		{
			int i, j;
			for (i = 0, j = index; i < innerStats.Length; i++)
			{
				int count = innerStats[i].ModifiersCount;
				if (j < count) break;
				j -= count;
			}
			return innerStats[i][j];
		}

		float IReadOnlyStat.this[int index] => GetModifier(index).Value;
		public StatModifier<T> this[int index] => GetModifier(index);

		public Enumerator GetEnumerator() => new(innerStats);
		IReadOnlyStat<T>.Enumerator IReadOnlyStat<T>.GetEnumerator() => new(this);
		IEnumerator<StatModifier<T>> IEnumerable<StatModifier<T>>.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}