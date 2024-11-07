namespace Kryz.RPG.Stats2
{
	public abstract class StatModifierListDefault : StatModifierList<StatModifier>
	{
		protected StatModifierListDefault(float defaultValue) : base(defaultValue) { }

		public int RemoveFromSource(object source)
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