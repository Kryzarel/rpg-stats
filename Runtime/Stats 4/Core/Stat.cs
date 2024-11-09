using System;
using System.Collections;
using System.Collections.Generic;

namespace Kryz.RPG.Stats4
{
	public abstract class Stat<T> : IStat<T> where T : struct, IStatModifierData
	{
		private class MultiStatList : IReadOnlyList<float>
		{
			private readonly StatContainer<T, IStat<T>>[] statContainers;

			public MultiStatList(StatContainer<T, IStat<T>>[] statContainers)
			{
				this.statContainers = statContainers;
			}

			public float this[int index]
			{
				get
				{
					(int i, int j) = FindIndices(index);
					return statContainers[i].Stat.ModifierValues[j];
				}
			}

			public int Count => SumCounts();

			public IEnumerator<float> GetEnumerator()
			{
				foreach (StatContainer<T, IStat<T>> container in statContainers)
				{
					foreach (float value in container.Stat.ModifierValues)
					{
						yield return value;
					}
				}
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

			private int SumCounts()
			{
				int count = 0;
				for (int i = 0; i < statContainers.Length; i++)
				{
					count += statContainers[i].Stat.ModifierValues.Count;
				}
				return count;
			}

			private (int, int) FindIndices(int index)
			{
				int i = 0;
				for (i = 0; i < statContainers.Length; i++)
				{
					int count = statContainers[i].Stat.ModifierValues.Count;
					if (index < count) break;
					index -= count;
				}
				return (i, index);
			}
		}

		protected readonly StatContainer<T, IStat<T>>[] statContainers;

		private float baseValue;
		private float finalValue;

		public event Action<IReadOnlyStat, float>? OnValueChanged;

		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
		public float FinalValue => finalValue;

		public IReadOnlyList<float> ModifierValues => throw new System.NotImplementedException();
		public IReadOnlyList<T> ModifierDatas => throw new System.NotImplementedException();

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
				finalValue = statContainers[i].Apply(finalValue);
			}
		}

		protected void AddModifier(int listIndex, float modifierValue, T data)
		{
			statContainers[listIndex].Stat.AddModifier(modifierValue, data);
			CalculateFinalValue();
		}

		protected bool RemoveModifier(int listIndex, float modifierValue, T data)
		{
			if (statContainers[listIndex].Stat.RemoveModifier(modifierValue, data))
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

		public abstract void AddModifier(float modifierValue, T data);
		public abstract bool RemoveModifier(float modifierValue, T data);
	}
}