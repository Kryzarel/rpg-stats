using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
			finalValue = baseValue;
			for (int i = 0; i < modifierValues.Count; i++)
			{
				AddOperation(finalValue, modifierValues[i], modifierDatas[i]);
			}
		}

		public void AddModifier(float modifierValue, T data)
		{
			modifierValues.Add(modifierValue);
			modifierDatas.Add(data);
			finalValue = AddOperation(finalValue, modifierValue, data);
		}

		public bool RemoveModifier(float modifierValue, T data)
		{
			for (int i = modifierValues.Count - 1; i >= 0; i--)
			{
				if (modifierValues[i] == modifierValue && dataComparer.Equals(modifierDatas[i], data))
				{
					modifierValues.RemoveAt(i);
					modifierDatas.RemoveAt(i);
					finalValue = RemoveOperation(finalValue, modifierValue, data);
					return true;
				}
			}
			return false;
		}

		public int RemoveWhere<TMatch>(TMatch match) where TMatch : IStatModifierMatch<T>
		{
			int removedCount = 0;
			for (int i = modifierValues.Count - 1; i >= 0; i--)
			{
				if (match.IsMatch(modifierValues[i], modifierDatas[i]))
				{
					modifierValues.RemoveAt(i);
					modifierDatas.RemoveAt(i);
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
			modifierValues.Clear();
			modifierDatas.Clear();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract float AddOperation(float currentValue, float modifierValue, T data);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract float RemoveOperation(float currentValue, float modifierValue, T data);
	}
}