using System;
using System.Linq;

namespace Kryz.RPG.Stats4
{
	public abstract class Stat<T> : IStat<T> where T : struct, IStatModifierData<T>
	{
		protected readonly StatContainer<T, IStat<T>>[] statContainers;

		private float baseValue;
		private float finalValue;

		public event Action<IReadOnlyStat, float>? OnValueChanged;

		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
		public float FinalValue => finalValue;
		public int ModifiersCount => statContainers.Select(c => c.Stat.ModifiersCount).Sum();

		protected Stat(float baseValue = 0, params StatContainer<T, IStat<T>>[] statContainers)
		{
			this.statContainers = statContainers;
			this.baseValue = baseValue;
			CalculateFinalValue();

			// for (int i = 0; i < statContainers.Length; i++)
			// {
			// 	statContainers[i].Stat.OnValueChanged += OnStatValueChanged;
			// }
		}

		// private void OnStatValueChanged(IReadOnlyStat stat, float value)
		// {
		// 	CalculateFinalValue();
		// }

		protected void CalculateFinalValue()
		{
			finalValue = baseValue;
			for (int i = 0; i < statContainers.Length; i++)
			{
				StatContainer<T, IStat<T>> container = statContainers[i];
				finalValue = container.Operation.Calculate(finalValue, container.Stat);
			}
		}

		protected void AddModifier(int listIndex, StatModifier<T> modifier)
		{
			statContainers[listIndex].Stat.AddModifier(modifier);
			CalculateFinalValue();
		}

		protected bool RemoveModifier(int listIndex, StatModifier<T> modifier)
		{
			if (statContainers[listIndex].Stat.RemoveModifier(modifier))
			{
				CalculateFinalValue();
				return true;
			}
			return false;
		}

		public int RemoveWhere<TMatch>(TMatch match) where TMatch : IStatModifierMatch<T>
		{
			int removedCount = 0;
			for (int i = 0; i < statContainers.Length; i++)
			{
				removedCount += statContainers[i].Stat.RemoveWhere(match);
			}

			if (removedCount > 0)
			{
				CalculateFinalValue();
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

		public float GetModifierValue(int index) => GetModifier(index).Value;

		private (int, int) FindIndices(int index)
		{
			int i = 0;
			for (i = 0; i < statContainers.Length; i++)
			{
				int count = statContainers[i].Stat.ModifiersCount;
				if (index < count) break;
				index -= count;
			}
			return (i, index);
		}
	}
}