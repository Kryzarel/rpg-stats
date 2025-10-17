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
		private float currentValue;
		private int modifiersCount;

		public event Action? OnValueChanged;

		public float BaseValue { get => baseValue; set => SetBaseValue(value); }
		public float FinalValue => GetFinalValue();
		public int ModifiersCount => modifiersCount;

		IReadOnlyList<IStat<T>> IStat<T>.Stats => Array.Empty<IStat<T>>();
		IReadOnlyList<IStat> IStat.Stats => Array.Empty<IStat<T>>();
		IReadOnlyList<IReadOnlyStat<T>> IReadOnlyStat<T>.Stats => Array.Empty<IStat<T>>();
		IReadOnlyList<IReadOnlyStat> IReadOnlyStat.Stats => Array.Empty<IStat<T>>();

		protected SimpleStat(float baseValue = 0)
		{
			this.baseValue = baseValue;
			currentValue = baseValue;
		}

		public void AddModifier(StatModifier<T> modifier)
		{
			Add(modifier);
			bool isDirty = AddOperation(modifier, baseValue, currentValue, out float newValue);
			CheckValueChanged(isDirty, newValue);
		}

		public bool RemoveModifier(StatModifier<T> modifier)
		{
			if (Remove(modifier))
			{
				bool isDirty = RemoveOperation(modifier, baseValue, currentValue, out float newValue);
				CheckValueChanged(isDirty, newValue);
				return true;
			}
			return false;
		}

		public int RemoveAllModifiers<TEquatable>(TEquatable match) where TEquatable : IEquatable<StatModifier<T>>
		{
			int removedCount = 0;
			bool isDirty = false;
			float newValue = currentValue;
			int keysCount = modifiers.Count;

			StatModifier<T>[] keys = ArrayPool<StatModifier<T>>.Shared.Rent(keysCount);
			modifiers.Keys.CopyTo(keys, 0);

			for (int i = 0; i < keysCount; i++)
			{
				StatModifier<T> modifier = keys[i];
				if (match.Equals(modifier) && modifiers.Remove(modifier, out int count))
				{
					for (int j = 0; j < count; j++)
					{
						isDirty |= RemoveOperation(modifier, baseValue, newValue, out newValue);
					}
					removedCount += count;
				}
			}

			Array.Clear(keys, 0, keysCount); // Clear only the used portion of the array
			ArrayPool<StatModifier<T>>.Shared.Return(keys);

			modifiersCount -= removedCount;
			CheckValueChanged(isDirty, newValue);
			return removedCount;
		}

		public void Clear()
		{
			modifiers.Clear();
			modifiersCount = 0;
			ClearCachedValues(baseValue);
			CheckValueChanged(false, baseValue);
		}

		public void GetModifiers(IList<StatModifier<T>> results)
		{
			foreach (KeyValuePair<StatModifier<T>, int> item in modifiers)
			{
				for (int i = 0; i < item.Value; i++)
				{
					results.Add(item.Key);
				}
			}
		}

		protected abstract bool AddOperation(StatModifier<T> modifier, float baseValue, float currentValue, out float newValue);
		protected abstract bool RemoveOperation(StatModifier<T> modifier, float baseValue, float currentValue, out float newValue);
		protected abstract bool SetBaseValue(float newBaseValue, float oldBaseValue, float currentValue, out float newValue);

		protected virtual void ClearCachedValues(float baseValue) { }

		private void Add(StatModifier<T> modifier)
		{
			modifiers.TryGetValue(modifier, out int count);
			modifiers[modifier] = count + 1;
			modifiersCount++;
		}

		private bool Remove(StatModifier<T> modifier)
		{
			if (modifiers.TryGetValue(modifier, out int count))
			{
				modifiersCount--;

				if (count > 1)
				{
					modifiers[modifier] = count - 1;
					return true;
				}
				return modifiers.Remove(modifier);
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetBaseValue(float value)
		{
			if (baseValue != value)
			{
				bool isDirty = SetBaseValue(value, baseValue, currentValue, out float newValue);
				baseValue = value;
				CheckValueChanged(isDirty, newValue);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private float GetFinalValue()
		{
			if (isDirty)
			{
				isDirty = false;
				currentValue = baseValue;
				ClearCachedValues(baseValue);

				foreach (KeyValuePair<StatModifier<T>, int> item in modifiers)
				{
					for (int i = 0; i < item.Value; i++)
					{
						AddOperation(item.Key, baseValue, currentValue, out currentValue);
					}
				}
			}
			return currentValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckValueChanged(bool newIsDirty, float newValue)
		{
			if (newIsDirty || currentValue != newValue)
			{
				isDirty |= newIsDirty;
				currentValue = newValue;
				OnValueChanged?.Invoke();
			}
		}
	}
}