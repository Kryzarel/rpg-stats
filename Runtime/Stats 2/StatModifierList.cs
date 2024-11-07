namespace Kryz.RPG.Stats2
{
	public abstract class StatModifierList<T> : Core.StatModifierList<T>, IStatModifierList<T> where T : struct, IStatModifier
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