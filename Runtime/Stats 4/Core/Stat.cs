using System;

namespace Kryz.RPG.Stats4
{
	public abstract class Stat<T> : IStat<T> where T : struct, IStatModifierData<T>
	{
		protected readonly StatContainer<T>[] statContainers;
		private readonly float[] cachedValues;

		private float baseValue;
		private float finalValue;

		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
		public float FinalValue { get { CheckValueChanged(); return finalValue; } }
		public int ModifiersCount => SumCounts();

		public event Action<IReadOnlyStat, float>? OnValueChanged;

		protected Stat(float baseValue = 0, params StatContainer<T>[] statContainers)
		{
			cachedValues = new float[statContainers.Length];
			this.statContainers = statContainers;
			this.baseValue = baseValue;
			CalculateFinalValue();
		}

		private void CheckValueChanged()
		{
			for (int i = 0; i < cachedValues.Length; i++)
			{
				if (cachedValues[i] != statContainers[i].Stat.FinalValue)
				{
					CalculateFinalValue();
					break;
				}
			}
		}

		private void CalculateFinalValue()
		{
			finalValue = baseValue;
			for (int i = 0; i < statContainers.Length; i++)
			{
				StatContainer<T> container = statContainers[i];
				finalValue = container.Operation.Calculate(finalValue, container.Stat);
				cachedValues[i] = container.Stat.FinalValue;
			}
		}

		public int RemoveWhere<TMatch>(TMatch match) where TMatch : IStatModifierMatch<T>
		{
			int removedCount = 0;
			for (int i = 0; i < statContainers.Length; i++)
			{
				removedCount += statContainers[i].Stat.RemoveWhere(match);
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
			finalValue = baseValue;
			for (int i = 0; i < statContainers.Length; i++)
			{
				statContainers[i].Stat.ClearModifiers();
			}
		}

		public abstract void AddModifier(StatModifier<T> modifier);
		public abstract bool RemoveModifier(StatModifier<T> modifier);

		public StatModifier<T> GetModifier(int index)
		{
			(int i, int j) = FindIndices(index);
			return statContainers[i].Stat.GetModifier(j);
		}

		public float GetModifierValue(int index)
		{
			return GetModifier(index).Value;
		}

		private (int, int) FindIndices(int index)
		{
			int i;
			for (i = 0; i < statContainers.Length; i++)
			{
				int count = statContainers[i].Stat.ModifiersCount;
				if (index < count) break;
				index -= count;
			}
			return (i, index);
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
	}
}