using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;

namespace Kryz.RPG.Stats.Core
{
	public abstract class SimpleStat<T> : IStat<T> where T : struct, IStatModifierData<T>
	{
		private readonly ArrayPool<StatModifier<T>> arrayPool;
		protected StatModifier<T>[] modifiers;
		protected int count;

		private float baseValue;
		private float finalValue;

		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
		public float FinalValue => finalValue;
		public int ModifiersCount => count;

		protected SimpleStat(float baseValue = 0, ArrayPool<StatModifier<T>>? arrayPool = null)
		{
			this.baseValue = baseValue;
			this.arrayPool = arrayPool ?? ArrayPool<StatModifier<T>>.Shared;
			modifiers = this.arrayPool.Rent(16);
			CalculateFinalValue();
		}

		protected virtual void Add(float baseValue, float currentValue, StatModifier<T> modifier) => Add(modifier);
		protected virtual bool Remove(float baseValue, float currentValue, StatModifier<T> modifier) => Remove(modifier);

		protected abstract float AddOperation(float baseValue, float currentValue, StatModifier<T> modifier);
		protected abstract float RemoveOperation(float baseValue, float currentValue, StatModifier<T> modifier);

		protected virtual float CalculateFinalValue(float baseValue, float currentValue)
		{
			for (int i = 0; i < count; i++)
			{
				currentValue = AddOperation(baseValue, currentValue, modifiers[i]);
			}
			return currentValue;
		}

		protected void Add(StatModifier<T> modifier)
		{
			if (count == modifiers.Length)
			{
				EnsureCapacity(count + 1);
			}
			modifiers[count++] = modifier;
		}

		protected void Insert(int index, StatModifier<T> modifier)
		{
			if (count == modifiers.Length)
			{
				EnsureCapacity(count + 1);
			}
			if (index < count)
			{
				Array.Copy(modifiers, index, modifiers, index + 1, count - index);
			}
			modifiers[index] = modifier;
			count++;
		}

		protected bool Remove(StatModifier<T> modifier)
		{
			int index = Array.IndexOf(modifiers, modifier, 0, count);
			if (index >= 0)
			{
				RemoveAt(index);
				return true;
			}
			return false;
		}

		protected void RemoveAt(int index)
		{
			if ((uint)index >= (uint)count)
			{
				throw new ArgumentOutOfRangeException(nameof(index), "Index must be less than the size of the collection");
			}

			count--;
			if (index < count)
			{
				Array.Copy(modifiers, index + 1, modifiers, index, count - index);
			}
			modifiers[count] = default;
		}

		protected void CalculateFinalValue()
		{
			finalValue = CalculateFinalValue(baseValue, baseValue);
		}

		public void AddModifier(StatModifier<T> modifier)
		{
			Add(baseValue, finalValue, modifier);
			finalValue = AddOperation(baseValue, finalValue, modifier);
		}

		public bool RemoveModifier(StatModifier<T> modifier)
		{
			if (Remove(baseValue, finalValue, modifier))
			{
				finalValue = RemoveOperation(baseValue, finalValue, modifier);
				return true;
			}
			return false;
		}

		public int RemoveWhere<TMatch>(TMatch match) where TMatch : IStatModifierMatch<T>
		{
			StatModifier<T>[] oldArray = modifiers;
			modifiers = arrayPool.Rent(modifiers.Length);

			int removedCount = 0;
			for (int i = 0, j = 0; i < count; i++)
			{
				StatModifier<T> modifier = oldArray[i];

				if (match.IsMatch(modifier))
				{
					finalValue = RemoveOperation(baseValue, finalValue, modifier);
					removedCount++;
				}
				else
				{
					modifiers[j++] = modifier;
				}
			}

			Array.Clear(oldArray, 0, count);
			arrayPool.Return(oldArray);
			count -= removedCount;

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
			Array.Clear(modifiers, 0, count);
			count = 0;
		}

		public void EnsureCapacity(int capacity)
		{
			if (Utils.CollectionExtensions.TryEnsureCapacity(modifiers.Length, capacity, out int newCapacity))
			{
				StatModifier<T>[] oldArray = modifiers;
				modifiers = arrayPool.Rent(newCapacity);
				Array.Copy(oldArray, modifiers, count);
				Array.Clear(oldArray, 0, count);
				arrayPool.Return(oldArray);
			}
		}

		public StatModifier<T> this[int index] => modifiers[index];
		float IReadOnlyStat.this[int index] => modifiers[index].Value;

		// public StatModifier<T>[].Enumerator GetEnumerator() => modifiers.GetEnumerator();
		IReadOnlyStat<T>.Enumerator IReadOnlyStat<T>.GetEnumerator() => new(this);
		IEnumerator<StatModifier<T>> IEnumerable<StatModifier<T>>.GetEnumerator() => ((IReadOnlyStat<T>)this).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IReadOnlyStat<T>)this).GetEnumerator();
	}
}