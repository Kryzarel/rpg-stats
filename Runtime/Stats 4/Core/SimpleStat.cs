using System;
using System.Collections.Generic;

namespace Kryz.RPG.Stats4
{
    public abstract class SimpleStat<T> : IStat<T> where T : struct, IStatModifierData
	{
		private static readonly EqualityComparer<T> dataComparer = EqualityComparer<T>.Default;

		protected readonly List<float> modifierValues = new();
		protected readonly List<T> modifierDatas = new();

		private float baseValue;
		private float finalValue;

		public event Action<IReadOnlyStat, float>? OnValueChanged;

		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
		public float FinalValue => finalValue;

		public IReadOnlyList<float> ModifierValues => modifierValues;
		public IReadOnlyList<T> ModifierDatas => modifierDatas;

		protected SimpleStat(float baseValue = 0)
		{
			this.baseValue = baseValue;
			finalValue = baseValue;
		}

		protected void CalculateFinalValue()
		{
			float initialValue = finalValue;
			finalValue = baseValue;

			for (int i = 0; i < modifierValues.Count; i++)
			{
				AddOperation(finalValue, modifierValues[i], modifierDatas[i]);
			}

			if (finalValue != initialValue)
			{
				OnValueChanged?.Invoke(this, finalValue);
			}
		}

		public void AddModifier(float modifierValue, T data)
		{
			modifierValues.Add(modifierValue);
			modifierDatas.Add(data);

			float initialValue = finalValue;
			finalValue = AddOperation(finalValue, modifierValue, data);

			if (finalValue != initialValue)
			{
				OnValueChanged?.Invoke(this, finalValue);
			}
		}

		public bool RemoveModifier(float modifierValue, T data)
		{
			for (int i = modifierValues.Count - 1; i >= 0; i--)
			{
				if (modifierValues[i] == modifierValue && dataComparer.Equals(modifierDatas[i], data))
				{
					modifierValues.RemoveAt(i);
					modifierDatas.RemoveAt(i);

					float initialValue = finalValue;
					finalValue = RemoveOperation(finalValue, modifierValue, data);

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

			for (int i = modifierValues.Count - 1; i >= 0; i--)
			{
				float value = modifierValues[i];
				T data = modifierDatas[i];

				if (match.IsMatch(value, data))
				{
					modifierValues.RemoveAt(i);
					modifierDatas.RemoveAt(i);
					finalValue = RemoveOperation(finalValue, value, data);
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
			modifierValues.Clear();
			modifierDatas.Clear();

			if (finalValue != initialValue)
			{
				OnValueChanged?.Invoke(this, finalValue);
			}
		}

		protected abstract float AddOperation(float currentValue, float modifierValue, T data);
		protected abstract float RemoveOperation(float currentValue, float modifierValue, T data);
	}
}