using System;
using System.Collections.Generic;
using Kryz.Utils;

namespace Kryz.RPG.Stats.Core
{
	public abstract class SimpleStat<T> : IStat<T> where T : struct, IStatModifierData<T>
	{
		private readonly Dictionary<StatModifier<T>, int> modifiers = new();

		private float baseValue;
		private float finalValue;

		public event Action? OnValueChanged;

		public float BaseValue { get => baseValue; set => SetBaseValue(value); }
		public float FinalValue => finalValue;

		public IReadOnlyList<StatModifier<T>> Modifiers => Array.Empty<StatModifier<T>>();

		IReadOnlyList<IStat<T>> IStat<T>.Stats => Array.Empty<IStat<T>>();
		IReadOnlyList<IStat> IStat.Stats => Array.Empty<IStat<T>>();
		IReadOnlyList<IReadOnlyStat<T>> IReadOnlyStat<T>.Stats => Array.Empty<IStat<T>>();
		IReadOnlyList<IReadOnlyStat> IReadOnlyStat.Stats => Array.Empty<IStat<T>>();

		protected SimpleStat(float baseValue = 0)
		{
			this.baseValue = baseValue;
			finalValue = baseValue;
		}

		protected virtual void Add(StatModifier<T> modifier)
		{
			modifiers.TryGetValue(modifier, out int count);
			modifiers[modifier] = count + 1;
		}

		protected virtual bool Remove(StatModifier<T> modifier)
		{
			if (modifiers.TryGetValue(modifier, out int count))
			{
				if (count > 1)
				{
					modifiers[modifier] = count - 1;
					return true;
				}
				return modifiers.Remove(modifier);
			}
			return false;
		}

		protected virtual int RemoveAll<TEquatable>(float baseValue, float currentValue, TEquatable match, out float finalValue) where TEquatable : IEquatable<StatModifier<T>>
		{
			using PooledList<StatModifier<T>> keys = PooledList<StatModifier<T>>.Rent(modifiers.Count);
			keys.AddRange(modifiers.Keys);
			int removedCount = 0;

			for (int i = 0; i < keys.Count; i++)
			{
				StatModifier<T> mod = keys[i];
				if (match.Equals(mod) && modifiers.Remove(mod, out int count))
				{
					removedCount += count;

					for (int j = 0; j < count; j++)
					{
						currentValue = RemoveOperation(baseValue, currentValue, mod);
					}
				}
			}

			finalValue = currentValue;
			return removedCount;
		}

		protected abstract float AddOperation(float baseValue, float currentValue, StatModifier<T> modifier);
		protected abstract float RemoveOperation(float baseValue, float currentValue, StatModifier<T> modifier);
		protected abstract float SetBaseValue(float oldBaseValue, float newBaseValue, float currentValue);

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

		public int RemoveAllModifiers<TEquatable>(TEquatable match) where TEquatable : IEquatable<StatModifier<T>>
		{
			float previousValue = finalValue;
			int removedCount = RemoveAll(baseValue, finalValue, match, out finalValue);

			if (finalValue != previousValue)
			{
				OnValueChanged?.Invoke();
			}
			return removedCount;
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
			finalValue = SetBaseValue(previousBaseValue, baseValue, finalValue);

			if (finalValue != previousValue)
			{
				OnValueChanged?.Invoke();
			}
		}
	}
}