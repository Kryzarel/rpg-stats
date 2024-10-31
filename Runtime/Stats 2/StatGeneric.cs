using System.Collections.Generic;

namespace Kryz.RPG.Stats2
{
	public abstract class Stat<T, TList> : IStat<T> where T : struct, IStatModifier where TList : IStatModifierList<T>
	{
		private readonly TList[] modifierLists;
		private float baseValue;
		private float finalValue;

		public IReadOnlyList<IReadOnlyStatModifierList<T>> Modifiers { get; private set; }
		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
		public float FinalValue => finalValue;

		protected Stat(float baseValue = 0, params TList[] modifierLists)
		{
			// I legit don't understand why all these gymnastics are needed.
			// If TList is already constrained to be IStatModifierList<T>, direct implicit cast should work?
			Modifiers = (IReadOnlyList<IReadOnlyStatModifierList<T>>)(IReadOnlyList<TList>)modifierLists;

			this.modifierLists = modifierLists;
			this.baseValue = baseValue;
			CalculateFinalValue();
		}

		public abstract void AddModifier(T modifier);
		public abstract bool RemoveModifier(T modifier);

		protected void AddModifier(int listIndex, T modifier)
		{
			modifierLists[listIndex].Add(modifier);
			CalculateFinalValue();
		}

		protected bool RemoveModifier(int listIndex, T modifier)
		{
			if (modifierLists[listIndex].Remove(modifier))
			{
				CalculateFinalValue();
				return true;
			}
			return false;
		}

		private void CalculateFinalValue()
		{
			finalValue = baseValue;
			for (int i = 0; i < modifierLists.Length; i++)
			{
				finalValue = modifierLists[i].Calculate(baseValue, finalValue);
			}
		}

		public void Clear()
		{
			baseValue = 0;
			ClearModifiers();
		}

		public void ClearModifiers()
		{
			finalValue = baseValue;
			for (int i = 0; i < modifierLists.Length; i++)
			{
				modifierLists[i].Clear();
			}
		}
	}
}