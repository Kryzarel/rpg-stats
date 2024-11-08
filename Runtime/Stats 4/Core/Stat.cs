using System;
using System.Collections.Generic;

namespace Kryz.RPG.Stats4
{
	public abstract class Stat<T> : IStat<T> where T : struct, IStatModifierData
	{
		protected readonly IStat<T>[] innerStats;

		private float baseValue;
		private float finalValue;

		public event Action<IReadOnlyStat, float>? OnValueChanged;

		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
		public float FinalValue => finalValue;

		public IReadOnlyList<float> ModifierValues => throw new System.NotImplementedException();
		public IReadOnlyList<T> ModifierDatas => throw new System.NotImplementedException();

		protected Stat(float baseValue = 0, params IStat<T>[] innerStats)
		{
			this.innerStats = innerStats;
			this.baseValue = baseValue;
			CalculateFinalValue();
		}

		protected void CalculateFinalValue()
		{
			finalValue = baseValue;
			for (int i = 0; i < innerStats.Length; i++)
			{
				// finalValue = modifierLists[i].Calculate(finalValue);
			}
		}

		protected void AddModifier(int listIndex, float modifierValue, T data)
		{
			innerStats[listIndex].AddModifier(modifierValue, data);
			CalculateFinalValue();
		}

		protected bool RemoveModifier(int listIndex, float modifierValue, T data)
		{
			if (innerStats[listIndex].RemoveModifier(modifierValue, data))
			{
				CalculateFinalValue();
				return true;
			}
			return false;
		}

		public int RemoveWhere<TMatch>(TMatch match) where TMatch : IStatModifierMatch<T>
		{
			int removedCount = 0;
			for (int i = 0; i < innerStats.Length; i++)
			{
				removedCount += innerStats[i].RemoveWhere(match);
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
			for (int i = 0; i < innerStats.Length; i++)
			{
				innerStats[i].Clear();
			}
		}

		public abstract void AddModifier(float modifierValue, T data);
		public abstract bool RemoveModifier(float modifierValue, T data);
	}
}