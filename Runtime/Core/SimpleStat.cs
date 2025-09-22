using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats.Core
{
	public abstract class SimpleStat<T> : IStat<T> where T : struct, IStatModifierData<T>
	{
		private readonly Dictionary<StatModifier<T>, int> modifiers = new();

		private bool isDirty;
		private float baseValue;
		private float finalValue;

		public event Action? OnValueChanged;

		public float BaseValue { get => baseValue; set => SetBaseValue(value); }
		public float FinalValue => GetFinalValue();

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

		private void Add(StatModifier<T> modifier)
		{
			modifiers.TryGetValue(modifier, out int count);
			modifiers[modifier] = count + 1;
		}

		private bool Remove(StatModifier<T> modifier)
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

		protected abstract bool AddOperation(StatModifier<T> modifier, float baseValue, float currentValue, out float finalValue);
		protected abstract bool RemoveOperation(StatModifier<T> modifier, float baseValue, float currentValue, out float finalValue);
		protected abstract bool SetBaseValue(float newBaseValue, float oldBaseValue, float currentValue, out float finalValue);

		public void AddModifier(StatModifier<T> modifier)
		{
			Add(modifier);

			bool newIsDirty = AddOperation(modifier, baseValue, finalValue, out float newFinalValue);

			if (newIsDirty || finalValue != newFinalValue)
			{
				isDirty |= newIsDirty;
				finalValue = newFinalValue;
				OnValueChanged?.Invoke();
			}
		}

		public bool RemoveModifier(StatModifier<T> modifier)
		{
			if (Remove(modifier))
			{
				bool newIsDirty = RemoveOperation(modifier, baseValue, finalValue, out float newFinalValue);

				if (newIsDirty || finalValue != newFinalValue)
				{
					isDirty |= newIsDirty;
					finalValue = newFinalValue;
					OnValueChanged?.Invoke();
				}
				return true;
			}
			return false;
		}

		public int RemoveAllModifiers<TEquatable>(TEquatable match) where TEquatable : IEquatable<StatModifier<T>>
		{
			int removedCount = 0;
			bool newIsDirty = false;
			float newFinalValue = finalValue;

			int modifiersCount = modifiers.Count;
			StatModifier<T>[] keys = ArrayPool<StatModifier<T>>.Shared.Rent(modifiersCount);
			modifiers.Keys.CopyTo(keys, 0);

			for (int i = 0; i < modifiersCount; i++)
			{
				StatModifier<T> modifier = keys[i];
				if (match.Equals(modifier) && modifiers.Remove(modifier, out int count))
				{
					for (int j = 0; j < count; j++)
					{
						newIsDirty |= RemoveOperation(modifier, baseValue, newFinalValue, out newFinalValue);
					}
					removedCount += count;
				}
			}

			if (newIsDirty || finalValue != newFinalValue)
			{
				isDirty |= newIsDirty;
				finalValue = newFinalValue;
				OnValueChanged?.Invoke();
			}

			ArrayPool<StatModifier<T>>.Shared.Return(keys, clearArray: true);
			return removedCount;
		}

		public void Clear()
		{
			modifiers.Clear();

			if (finalValue != baseValue)
			{
				finalValue = baseValue;
				OnValueChanged?.Invoke();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetBaseValue(float value)
		{
			bool newIsDirty = SetBaseValue(value, baseValue, finalValue, out float newFinalValue);

			baseValue = value;

			if (newIsDirty || finalValue != newFinalValue)
			{
				isDirty |= newIsDirty;
				finalValue = newFinalValue;
				OnValueChanged?.Invoke();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private float GetFinalValue()
		{
			if (isDirty)
			{
				isDirty = false;

				finalValue = baseValue;
				foreach (KeyValuePair<StatModifier<T>, int> item in modifiers)
				{
					for (int i = 0; i < item.Value; i++)
					{
						AddOperation(item.Key, baseValue, finalValue, out finalValue);
					}
				}
			}
			return finalValue;
		}
	}
}