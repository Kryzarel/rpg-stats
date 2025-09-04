using System;
using System.Collections.Generic;
using Kryz.Utils;

namespace Kryz.RPG.Stats.Core
{
	public abstract class SimpleStat<T> : IStat<T> where T : struct, IStatModifierData<T>
	{
		protected readonly PooledList<StatModifier<T>> modifiers = new();

		private float baseValue;
		private float finalValue;

		public event Action? OnValueChanged;

		public float BaseValue { get => baseValue; set => SetBaseValue(value); }
		public float FinalValue => finalValue;

		public IReadOnlyList<StatModifier<T>> Modifiers => modifiers;

		IReadOnlyList<IStat<T>> IStat<T>.Stats => Array.Empty<IStat<T>>();
		IReadOnlyList<IStat> IStat.Stats => Array.Empty<IStat<T>>();
		IReadOnlyList<IReadOnlyStat<T>> IReadOnlyStat<T>.Stats => Array.Empty<IStat<T>>();
		IReadOnlyList<IReadOnlyStat> IReadOnlyStat.Stats => Array.Empty<IStat<T>>();

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

			float previousValue = finalValue;
			finalValue = AddOperation(baseValue, finalValue, modifier);

			if (finalValue != previousValue)
			{
				OnValueChanged?.Invoke();
			}
		}

		public bool RemoveModifier(StatModifier<T> modifier)
		{
			if (Remove(modifier))
			{
				float previousValue = finalValue;
				finalValue = RemoveOperation(baseValue, finalValue, modifier);

				if (finalValue != previousValue)
				{
					OnValueChanged?.Invoke();
				}
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

			float previousValue = finalValue;

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

			if (finalValue != previousValue)
			{
				OnValueChanged?.Invoke();
			}
			return result;
		}

		public void Clear()
		{
			modifiers.Clear();

			float previousValue = finalValue;
			finalValue = baseValue;

			if (finalValue != previousValue)
			{
				OnValueChanged?.Invoke();
			}
		}

		private void SetBaseValue(float value)
		{
			float previousBaseValue = baseValue;
			baseValue = value;

			float previousValue = finalValue;
			finalValue = ChangeBaseValue(previousBaseValue, baseValue, finalValue);

			if (finalValue != previousValue)
			{
				OnValueChanged?.Invoke();
			}
		}
	}
}