using Kryz.RPG.Stats2.Core;

namespace Kryz.RPG.Stats2
{
	public abstract class StatModifierList : StatModifierList<StatModifier>, IStatModifierList
	{
		protected StatModifierList(float defaultValue) : base(defaultValue) { }

		public int RemoveModifiersFromSource(object source)
		{
			int numRemoved = 0;
			for (int i = modifiers.Count - 1; i >= 0; i--)
			{
				if (modifiers[i].Source == source)
				{
					modifiers.RemoveAt(i);
					numRemoved++;
				}
			}
			return numRemoved;
		}
	}
}