using System.Collections.Generic;

namespace Kryz.RPG.Stats2
{
	public class Stat<T, TList> : IStat<T> where T : struct, IStatModifier where TList : IStatModifierList<T>
	{
		private readonly TList[] modifierLists;
		private float baseValue;
		private float finalValue;

		public IReadOnlyList<IReadOnlyStatModifierList<T>> Modifiers { get; private set; }
		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
		public float FinalValue => finalValue;

		public Stat(float baseValue = 0, params TList[] modifierLists)
		{
			// I legit don't understand why all these gymnastics are needed.
			// If TList is already constrained to be IStatModifierList<T>, direct implicit cast should work?
			Modifiers = (IReadOnlyList<IReadOnlyStatModifierList<T>>)(IReadOnlyList<TList>)modifierLists;

			this.modifierLists = modifierLists;
			this.baseValue = baseValue;
			finalValue = baseValue;
		}

		public bool TryAddModifier(int listIndex, T modifier)
		{
			if (listIndex >= 0 && listIndex < modifierLists.Length)
			{
				modifierLists[listIndex].Add(modifier);
				CalculateFinalValue();
				return true;
			}
			return false;
		}

		public bool TryRemoveModifier(int listIndex, T modifier)
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

		public void Clear()
		{
			baseValue = 0;
			finalValue = baseValue;
			ClearModifiers();
		}

		public void ClearModifiers()
		{
			for (int i = 0; i < modifierLists.Length; i++)
			{
				modifierLists[i].Clear();
			}
			finalValue = baseValue;
		}
	}
}