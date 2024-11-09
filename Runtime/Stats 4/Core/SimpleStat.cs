using System;
using System.Collections.Generic;

namespace Kryz.RPG.Stats4
{
	public abstract class SimpleStat<T> : IStat<T> where T : struct, IStatModifierData<T>
	{
		protected readonly List<StatModifier<T>> modifiers = new();

		private float baseValue;
		private float finalValue;

		public event Action<IReadOnlyStat, float>? OnValueChanged;

		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
		public float FinalValue => finalValue;
		public int ModifiersCount => modifiers.Count;

		protected SimpleStat(float baseValue = 0)
		{
			this.baseValue = baseValue;
			finalValue = baseValue;
		}

		protected void CalculateFinalValue()
		{
			float initialValue = finalValue;
			finalValue = baseValue;

			for (int i = 0; i < modifiers.Count; i++)
			{
				AddOperation(finalValue, modifiers[i]);
			}

			if (finalValue != initialValue)
			{
				OnValueChanged?.Invoke(this, finalValue);
			}
		}

		public void AddModifier(StatModifier<T> modifier)
		{
			modifiers.Add(modifier);

			float initialValue = finalValue;
			finalValue = AddOperation(finalValue, modifier);

			if (finalValue != initialValue)
			{
				OnValueChanged?.Invoke(this, finalValue);
			}
		}

		public bool RemoveModifier(StatModifier<T> modifier)
		{
			for (int i = modifiers.Count - 1; i >= 0; i--)
			{
				if (modifiers[i] == modifier)
				{
					modifiers.RemoveAt(i);

					float initialValue = finalValue;
					finalValue = RemoveOperation(finalValue, modifier);

					if (finalValue != initialValue)
					{
						OnValueChanged?.Invoke(this, finalValue);
					}
					return true;
				}
			}
			return false;
		}

		public int RemoveWhere<TMatch>(TMatch match) where TMatch : IStatModifierMatch<T>
		{
			float initialValue = finalValue;
			int removedCount = 0;

			for (int i = modifiers.Count - 1; i >= 0; i--)
			{
				StatModifier<T> modifier = modifiers[i];

				if (match.IsMatch(modifier))
				{
					modifiers.RemoveAt(i);
					finalValue = RemoveOperation(finalValue, modifier);
					removedCount++;
				}
			}

			if (finalValue != initialValue)
			{
				OnValueChanged?.Invoke(this, finalValue);
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
			float initialValue = finalValue;
			finalValue = baseValue;
			modifiers.Clear();

			if (finalValue != initialValue)
			{
				OnValueChanged?.Invoke(this, finalValue);
			}
		}

		protected abstract float AddOperation(float currentValue, StatModifier<T> modifier);
		protected abstract float RemoveOperation(float currentValue, StatModifier<T> modifier);

		public StatModifier<T> GetModifier(int index)
		{
			throw new NotImplementedException();
		}

		public float GetModifierValue(int index)
		{
			throw new NotImplementedException();
		}
	}
}