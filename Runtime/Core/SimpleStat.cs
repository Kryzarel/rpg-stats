using System;
using System.Collections;
using System.Collections.Generic;
using Kryz.Utils;

namespace Kryz.RPG.Stats.Core
{
	public abstract class SimpleStat<T> : IStat<T> where T : struct, IStatModifierData<T>
	{
		protected readonly PooledList<StatModifier<T>> modifiers = new();

		private float baseValue;
		private float finalValue;

		public float BaseValue { get => baseValue; set { float old = baseValue; baseValue = value; finalValue = ChangeBaseValue(old, baseValue, finalValue); } }
		public float FinalValue => finalValue;
		public int ModifiersCount => modifiers.Count;

		protected SimpleStat(float baseValue = 0)
		{
			this.baseValue = baseValue;
			finalValue = baseValue;
		}

		protected virtual void Add(StatModifier<T> modifier) => modifiers.Add(modifier);
		protected virtual bool Remove(StatModifier<T> modifier) => modifiers.Remove(modifier);

		protected abstract float AddOperation(float baseValue, float currentValue, StatModifier<T> modifier);
		protected abstract float RemoveOperation(float baseValue, float currentValue, StatModifier<T> modifier);
		protected abstract float ChangeBaseValue(float oldBaseValue, float newBaseValue, float currentValue);

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

		public int RemoveAll<TEquatable>(TEquatable match) where TEquatable : IEquatable<StatModifier<T>>
		{
			int freeIndex = 0; // the first free slot in the array
			int count = modifiers.Count;

			// Find the first item which needs to be removed.
			while (freeIndex < count && !match.Equals(modifiers[freeIndex])) freeIndex++;
			if (freeIndex >= count) return 0;

			int current = freeIndex + 1;
			while (current < count)
			{
				// Find the first item which needs to be kept.
				while (current < count)
				{
					StatModifier<T> modifier = modifiers[current];
					if (!match.Equals(modifier)) break;

					current++;
					finalValue = RemoveOperation(baseValue, finalValue, modifier);
				}

				if (current < count)
				{
					// copy item to the free slot.
					modifiers[freeIndex++] = modifiers[current++];
				}
			}

			int result = count - freeIndex;
			modifiers.RemoveRange(freeIndex, result);
			return result;
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