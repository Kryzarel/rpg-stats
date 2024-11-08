namespace Kryz.RPG.Stats4
{
	public class StatModifierListAdd<T> : StatModifierList<T> where T : struct, IStatModifierMetaData
	{
		public StatModifierListAdd() : base(defaultValue: 0) { }

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
			return statCurrentValue + modifierValue;
		}
	}
}