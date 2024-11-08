using System.Collections.Generic;

namespace Kryz.RPG.Stats4
{
	public abstract class Stat<T, TList> : IStat<T> where T : struct, IStatModifierMetaData where TList : IStatModifierList<T>
	{
		protected readonly TList[] modifierLists;

		private float baseValue;
		private float finalValue;

		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
		public float FinalValue => finalValue;

		public IReadOnlyList<IReadOnlyStatModifierList<T>> Modifiers { get; }
		IReadOnlyList<IStatModifierList> IStat.Modifiers => (IReadOnlyList<IStatModifierList>)Modifiers;

		protected Stat(float baseValue = 0, params TList[] modifierLists)
		{
			// I legit don't understand why all these gymnastics are needed.
			// If TList is already constrained to be IStatModifierList<T>, direct implicit cast should work?
			Modifiers = (IReadOnlyList<IReadOnlyStatModifierList<T>>)(IReadOnlyList<TList>)modifierLists;

			this.modifierLists = modifierLists;
			this.baseValue = baseValue;
			CalculateFinalValue();
		}

		protected void CalculateFinalValue()
		{
			finalValue = baseValue;
			for (int i = 0; i < modifierLists.Length; i++)
			{
				finalValue = modifierLists[i].Calculate(finalValue);
			}
		}

		protected void AddModifier(int listIndex, float modifierValue, T metaData)
		{
			modifierLists[listIndex].Add(modifierValue, metaData);
			CalculateFinalValue();
		}

		protected bool RemoveModifier(int listIndex, float modifierValue, T metaData)
		{
			if (modifierLists[listIndex].Remove(modifierValue, metaData))
			{
				CalculateFinalValue();
				return true;
			}
			return false;
		}

		public int RemoveWhere<TMatch>(TMatch match) where TMatch : IStatModifierMatch<T>
		{
			int removedCount = 0;
			for (int i = 0; i < modifierLists.Length; i++)
			{
				removedCount += modifierLists[i].RemoveWhere(match);
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
			for (int i = 0; i < modifierLists.Length; i++)
			{
				modifierLists[i].Clear();
			}
		}

		public abstract void AddModifier(float modifierValue, T metaData);
		public abstract bool RemoveModifier(float modifierValue, T metaData);
	}
}