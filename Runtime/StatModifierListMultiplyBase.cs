namespace Kryz.RPG.Stats
{
	public sealed class StatModifierListMultiplyBase : StatModifierList<SimpleStatModifier>
	{
		public StatModifierListMultiplyBase() : base(defaultValue: 1) { }

		public override float Calculate(float value)
		{
			return value * CurrentValue;
		}

		protected override float AddOperation(float currentValue, SimpleStatModifier modifier)
		{
			return currentValue + modifier.Value;
		}

		protected override float RemoveOperation(float currentValue, SimpleStatModifier modifier)
		{
			return currentValue - modifier.Value;
		}
	}
}