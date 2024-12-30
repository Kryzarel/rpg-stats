using System.Collections;
using System.Collections.Generic;

namespace Kryz.RPG.Stats.Core
{
	public abstract partial class Stat<T> : IStat<T> where T : struct, IStatModifierData<T>
	{
		protected readonly StatContainer<T>[] statContainers;
		private readonly float[] cachedValues;

		private float baseValue;
		private float finalValue;

		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
		public float FinalValue { get { CheckValueChanged(); return finalValue; } }
		public int ModifiersCount => SumCounts();

		protected Stat(float baseValue = 0, params StatContainer<T>[] statContainers)
		{
			cachedValues = new float[statContainers.Length];
			this.statContainers = statContainers;
			this.baseValue = baseValue;
			CheckValueChanged();
		}

		public abstract void AddModifier(StatModifier<T> modifier);
		public abstract bool RemoveModifier(StatModifier<T> modifier);

		protected virtual float CalculateFinalValue(float baseValue)
		{
			float currentValue = baseValue;
			for (int i = 0; i < statContainers.Length; i++)
			{
				StatContainer<T> container = statContainers[i];
				currentValue = container.Operation.Calculate(currentValue, container.Stat);
			}
			return currentValue;
		}

		protected void CalculateFinalValue()
		{
			finalValue = CalculateFinalValue(baseValue);
		}

		private void CheckValueChanged()
		{
			bool changed = false;
			for (int i = 0; i < cachedValues.Length; i++)
			{
				float value = statContainers[i].Stat.FinalValue;
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

		public int RemoveWhere<TMatch>(TMatch match) where TMatch : IStatModifierMatch<T>
		{
			int removedCount = 0;
			for (int i = 0; i < statContainers.Length; i++)
			{
				removedCount += statContainers[i].Stat.RemoveWhere(match);
			}
			// finalValue = CalculateFinalValue(baseValue);
			return removedCount;
		}

		public void Clear()
		{
			baseValue = 0;
			ClearModifiers();
		}

		public void ClearModifiers()
		{
			finalValue = baseValue;
			for (int i = 0; i < statContainers.Length; i++)
			{
				statContainers[i].Stat.ClearModifiers();
			}
		}

		private int SumCounts()
		{
			int count = 0;
			for (int i = 0; i < statContainers.Length; i++)
			{
				count += statContainers[i].Stat.ModifiersCount;
			}
			return count;
		}

		private StatModifier<T> GetModifier(int index)
		{
			int i, j;
			for (i = 0, j = index; i < statContainers.Length; i++)
			{
				int count = statContainers[i].Stat.ModifiersCount;
				if (j < count) break;
				j -= count;
			}
			return statContainers[i].Stat[j];
		}

		float IReadOnlyStat.this[int index] => GetModifier(index).Value;
		public StatModifier<T> this[int index] => GetModifier(index);

		public Enumerator GetEnumerator() => new(statContainers);
		IReadOnlyStat<T>.Enumerator IReadOnlyStat<T>.GetEnumerator() => new(this);
		IEnumerator<StatModifier<T>> IEnumerable<StatModifier<T>>.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}