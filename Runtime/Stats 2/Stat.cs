namespace Kryz.RPG.Stats2
{
	public class Stat : Stat<StatModifier, StatModifierList<StatModifier>>
	{
		public Stat(float baseValue = 0) : base(baseValue, new StatModifierListAdd(), new StatModifierListMultiplyBase(), new StatModifierListMultiplyTotal()) { }
	}
}