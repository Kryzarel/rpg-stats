using System.Collections.Generic;

namespace Kryz.RPG.Stats2
{
	public class Stat : IStat<StatModifier>
	{
		private readonly StatModifierList<StatModifier>[] modifierLists;
		private float baseValue;
		private float finalValue;

		public IReadOnlyList<IReadOnlyStatModifierList<StatModifier>> Modifiers => modifierLists;
		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
		public float FinalValue => finalValue;

		public Stat(float baseValue = 0) : this(baseValue, new StatModifierListAdd(), new StatModifierListMultiplyBase(), new StatModifierListMultiplyTotal()) { }

		public Stat(float baseValue = 0, params StatModifierList<StatModifier>[] modifierLists)
		{
			this.modifierLists = modifierLists;
			this.baseValue = baseValue;
			finalValue = baseValue;
		}

		public bool TryAddModifier(int listIndex, StatModifier modifier)
		{
			if (listIndex >= 0 && listIndex < modifierLists.Length)
			{
				modifierLists[listIndex].Add(modifier);
				CalculateFinalValue();
				return true;
			}
			return false;
		}

		public bool TryRemoveModifier(int listIndex, StatModifier modifier)
		{
			if (listIndex >= 0 && listIndex < modifierLists.Length)
			{
				if (modifierLists[listIndex].Remove(modifier))
				{
					CalculateFinalValue();
					return true;
				}
			}
			return false;
		}

		public int RemoveModifiersFromSource(object source)
		{
			int numRemoved = 0;
			for (int i = 0; i < modifierLists.Length; i++)
			{
				numRemoved += modifierLists[i].RemoveFromSource(source);
			}
			if (numRemoved > 0)
			{
				CalculateFinalValue();
			}
			return numRemoved;
		}

		private void CalculateFinalValue()
		{
			finalValue = baseValue;
			for (int i = 0; i < modifierLists.Length; i++)
			{
				finalValue = modifierLists[i].Calculate(finalValue);
			}
		}
	}
}