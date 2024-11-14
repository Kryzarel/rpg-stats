using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.StatsLegacy
{
	public abstract class Stat<T> : IStat<T> where T : struct, IStatModifier
	{
		private readonly List<T> modifiers = new();

		private float baseValue;
		private float finalValue;
		private bool shouldSort;
		private bool shouldCalculate;

		public float BaseValue { get => baseValue; set => baseValue = value; }
		public float FinalValue => GetFinalValue();
		public IReadOnlyList<T> Modifiers => modifiers;

		public Stat(float baseValue)
		{
			this.baseValue = baseValue;
		}

		public void AddModifier(T modifier)
		{
			modifiers.Add(modifier);
			shouldSort = shouldCalculate = true;
		}

		public bool RemoveModifier(T modifier)
		{
			int index = modifiers.BinarySearch(modifier);
			if (index >= 0)
			{
				modifiers.RemoveAt(index);
				shouldCalculate = true;
				return true;
			}
			return false;
		}

		public int RemoveModifiersFromSource(object source)
		{
			int numRemoved = 0;
			for (int i = modifiers.Count - 1; i >= 0; i--)
			{
				T modifier = modifiers[i];
				if (modifier.Source == source)
				{
					modifiers.RemoveAt(i);
					numRemoved++;
				}
			}
			shouldCalculate |= numRemoved > 0;
			return numRemoved;
		}

		public void Clear()
		{
			modifiers.Clear();
			finalValue = baseValue = 0;
		}

		public void ClearModifiers()
		{
			modifiers.Clear();
			finalValue = baseValue;
		}

		private float GetFinalValue()
		{
			if (shouldCalculate)
			{
				if (shouldSort)
				{
					Sort(modifiers);
					shouldSort = false;
				}
				finalValue = CalculateFinalValue();
				shouldCalculate = false;
			}
			return finalValue;
		}

		private float CalculateFinalValue()
		{
			float result = baseValue;
			for (int i = 0; i < modifiers.Count; i++)
			{
				T modifier = modifiers[i];
				result = CalculateModifier(result, baseValue, modifier);
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract float CalculateModifier(float currentValue, float baseValue, T modifier);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract void Sort(List<T> modifiers);
	}
}