using System;
using System.Collections;
using System.Collections.Generic;
using Kryz.Collections;

namespace Kryz.RPG.Stats.Core
{
	public abstract class SimpleStat<T> : IStat<T> where T : struct, IStatModifierData<T>
	{
		protected PooledList<StatModifier<T>> modifiers = new();
		protected PooledList<StatModifier<T>> aux = new();

		private float baseValue;
		private float finalValue;

		public float BaseValue { get => baseValue; set { baseValue = value; finalValue = CalculateFinalValue(baseValue); } }
		public float FinalValue => finalValue;
		public int ModifiersCount => modifiers.Count;

		protected SimpleStat(float baseValue = 0)
		{
			this.baseValue = baseValue;
			finalValue = CalculateFinalValue(baseValue);
		}

		protected virtual void Add(StatModifier<T> modifier) => modifiers.Add(modifier);
		protected virtual bool Remove(StatModifier<T> modifier) => modifiers.Remove(modifier);

		protected abstract float AddOperation(float baseValue, float currentValue, StatModifier<T> modifier);
		protected abstract float RemoveOperation(float baseValue, float currentValue, StatModifier<T> modifier);

		protected virtual float CalculateFinalValue(float baseValue)
		{
			float currentValue = baseValue;
			for (int i = 0; i < modifiers.Count; i++)
			{
				currentValue = AddOperation(baseValue, currentValue, modifiers[i]);
			}
			return currentValue;
		}

		public void AddModifier(StatModifier<T> modifier)
		{
			Add(modifier);
			finalValue = AddOperation(baseValue, finalValue, modifier);
		}

		public bool RemoveModifier(StatModifier<T> modifier)
		{
			if (Remove(modifier))
			{
				finalValue = RemoveOperation(baseValue, finalValue, modifier);
				return true;
			}
			return false;
		}

		public int RemoveWhere<TMatch>(TMatch match) where TMatch : IEquatable<StatModifier<T>>
		{
			int count = modifiers.Count;
			aux.EnsureCapacity(count);

			for (int i = count - 1; i >= 0; i--)
			{
				StatModifier<T> modifier = modifiers[i];

				if (match.Equals(modifier))
				{
					finalValue = RemoveOperation(baseValue, finalValue, modifier);
				}
				else
				{
					aux.Add(modifier);
				}
			}

			(modifiers, aux) = (aux, modifiers);
			aux.Clear();

			int removedCount = modifiers.Count - count;
			return removedCount;
		}

		public void Clear()
		{
			baseValue = 0;
			ClearModifiers();
		}

		public void ClearModifiers()
		{
			modifiers.Clear();
			finalValue = baseValue;
		}

		public StatModifier<T> this[int index] => modifiers[index];
		float IReadOnlyStat.this[int index] => modifiers[index].Value;

		public PooledList<StatModifier<T>>.Enumerator GetEnumerator() => modifiers.GetEnumerator();
		IReadOnlyStat<T>.Enumerator IReadOnlyStat<T>.GetEnumerator() => new(this);
		IEnumerator<StatModifier<T>> IEnumerable<StatModifier<T>>.GetEnumerator() => ((IReadOnlyStat<T>)this).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IReadOnlyStat<T>)this).GetEnumerator();
	}
}