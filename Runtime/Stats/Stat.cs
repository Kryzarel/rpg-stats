using System.Collections.Generic;

namespace Kryz.RPG.Stats
{
	public class Stat
	{
		private readonly List<StatModifier> modifiers = new();

		private float baseValue;
		private float finalValue;
		private bool shouldSort;
		private bool shouldCalculate;

		public float BaseValue { get => baseValue; set => baseValue = value; }
		public float FinalValue => GetFinalValue();
		public IReadOnlyList<StatModifier> Modifiers => modifiers;

		public Stat(float baseValue)
		{
			this.baseValue = baseValue;
		}

		public void Add(StatModifier modifier)
		{
			modifiers.Add(modifier);
			shouldSort = shouldCalculate = true;
		}

		public bool Remove(StatModifier modifier)
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

		public int RemoveAllFromSource(object source)
		{
			int numRemoved = 0;
			for (int i = modifiers.Count - 1; i >= 0; i--)
			{
				StatModifier modifier = modifiers[i];
				if (modifier.Source == source)
				{
					modifiers.RemoveAt(i);
					numRemoved++;
				}
			}
			shouldCalculate |= numRemoved > 0;
			return numRemoved;
		}

		private float GetFinalValue()
		{
			if (shouldCalculate)
			{
				if (shouldSort)
				{
					modifiers.Sort();
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
				StatModifier modifier = modifiers[i];
				result = CalculateModifier(result, baseValue, modifier);

				if (modifier.Type == StatModifierType.Override)
				{
					break;
				}
			}
			return result;
		}

		private static float CalculateModifier(float currentValue, float baseValue, StatModifier modifier) => modifier.Type switch
		{
			StatModifierType.Add => currentValue + modifier.Value,
			StatModifierType.MultiplyBase => currentValue + baseValue * modifier.Value,
			StatModifierType.MultiplyTotal => currentValue + currentValue * modifier.Value,
			StatModifierType.Override => modifier.Value,
			_ => currentValue,
		};
	}
}