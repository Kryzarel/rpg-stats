namespace Kryz.RPG.Stats4
{
	public class StatModifierListMultiply<T> : StatModifierList<T> where T : struct, IStatModifierMetaData
	{
		public StatModifierListMultiply() : base(defaultValue: 1) { }

		protected override float AddOperation(float currentValue, float modifierValue, T metaData)
		{
			return currentValue + modifierValue;
		}

		protected override float RemoveOperation(float currentValue, float modifierValue, T metaData)
		{
			return currentValue - modifierValue;
		}

		protected override float Calculate(float statCurrentValue, float modifierValue)
		{
			return statCurrentValue * modifierValue;
		}
	}
}