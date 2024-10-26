namespace Kryz.RPG.Stats2
{
	public class Stat : Stat<StatModifier, StatModifierList<StatModifier>>
	{
		public enum Type
		{
			Add,
			MultiplyBase,
			MultiplyTotal,
		}

		public Stat(float baseValue = 0) : base(baseValue, new StatModifierListAdd(), new StatModifierListMultiplyBase(), new StatModifierListMultiplyTotal()) { }

		public bool TryAddModifier(Type type, StatModifier modifier)
		{
			return TryAddModifier((int)type, modifier);
		}

		public bool TryRemoveModifier(Type type, StatModifier modifier)
		{
			return TryRemoveModifier((int)type, modifier);
		}
	}
}