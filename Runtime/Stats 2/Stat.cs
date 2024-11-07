using Kryz.RPG.Stats2.Core;

namespace Kryz.RPG.Stats2
{
	public abstract class Stat<T> : Stat<T, StatModifierList<T>>, IStat<T> where T : struct, IStatModifier
	{
		protected Stat(float baseValue = 0, params StatModifierList<T>[] modifierLists) : base(baseValue, modifierLists) { }

		public int RemoveModifiersFromSource(object source)
		{
			int numRemoved = 0;
			for (int i = 0; i < modifierLists.Length; i++)
			{
				numRemoved += modifierLists[i].RemoveModifiersFromSource(source);
			}
			if (numRemoved > 0)
			{
				CalculateFinalValue();
			}
			return numRemoved;
		}
	}
}