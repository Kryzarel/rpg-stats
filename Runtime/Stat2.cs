namespace Kryz.RPG.Stats
{
	public class Stat2
	{
		private readonly StatModifierList<SimpleStatModifier>[] modifierLists;

		private float baseValue;
		private float finalValue;

		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
		public float FinalValue => finalValue;

		public Stat2(float baseValue = 0) : this(baseValue, new StatModifierListAdd(), new StatModifierListMultiplyBase(), new StatModifierListMultiplyTotal()) { }

		public Stat2(float baseValue = 0, params StatModifierList<SimpleStatModifier>[] modifierLists)
		{
			this.modifierLists = modifierLists;
			this.baseValue = baseValue;
			finalValue = baseValue;
		}

		public bool TryAddModifier(int listIndex, SimpleStatModifier modifier)
		{
			if (listIndex >= 0 && listIndex < modifierLists.Length)
			{
				modifierLists[listIndex].Add(modifier);
				CalculateFinalValue();
				return true;
			}
			return false;
		}

		public bool TryRemoveModifier(int listIndex, SimpleStatModifier modifier)
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

		public int RemoveAllFromSource(object source)
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