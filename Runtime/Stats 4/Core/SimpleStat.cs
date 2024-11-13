using System.Collections;
using System.Collections.Generic;

namespace Kryz.RPG.Stats4
{
	public abstract class SimpleStat<T> : IStat<T> where T : struct, IStatModifierData<T>
	{
		protected readonly List<StatModifier<T>> modifiers = new();

		private float baseValue;
		private float finalValue;

		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
		public float FinalValue => finalValue;
		public int ModifiersCount => modifiers.Count;

		protected SimpleStat(float baseValue = 0)
		{
			this.baseValue = baseValue;
			CalculateFinalValue();
		}

		protected abstract float AddOperation(float baseValue, float currentValue, StatModifier<T> modifier);
		protected abstract float RemoveOperation(float baseValue, float currentValue, StatModifier<T> modifier);

		protected virtual float CalculateFinalValue(float baseValue, float currentValue)
		{
			for (int i = 0; i < modifiers.Count; i++)
			{
				currentValue = AddOperation(baseValue, currentValue, modifiers[i]);
			}
			return currentValue;
		}

		protected void CalculateFinalValue()
		{
			finalValue = CalculateFinalValue(baseValue, baseValue);
		}

		public void AddModifier(StatModifier<T> modifier)
		{
			modifiers.Add(modifier);
			finalValue = AddOperation(baseValue, finalValue, modifier);
		}

		public bool RemoveModifier(StatModifier<T> modifier)
		{
			if (modifiers.Remove(modifier))
			{
				finalValue = RemoveOperation(baseValue, finalValue, modifier);
				return true;
			}
			return false;
		}

		public int RemoveWhere<TMatch>(TMatch match) where TMatch : IStatModifierMatch<T>
		{
			int removedCount = 0;
			for (int i = modifiers.Count - 1; i >= 0; i--)
			{
				StatModifier<T> modifier = modifiers[i];

				if (match.IsMatch(modifier))
				{
					modifiers.RemoveAt(i);
					finalValue = RemoveOperation(baseValue, finalValue, modifier);
					removedCount++;
				}
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
			modifiers.Clear();
		}

		public StatModifier<T> GetModifier(int index) => modifiers[index];
		public float GetModifierValue(int index) => modifiers[index].Value;

		public List<StatModifier<T>>.Enumerator GetEnumerator() => modifiers.GetEnumerator();
		StatEnumerator<T> IReadOnlyStat<T>.GetEnumerator() => new(this);
		IEnumerator<StatModifier<T>> IEnumerable<StatModifier<T>>.GetEnumerator() => ((IReadOnlyStat<T>)this).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IReadOnlyStat<T>)this).GetEnumerator();
	}
}