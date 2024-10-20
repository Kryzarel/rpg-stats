namespace Kryz.RPG.Stats
{
	public sealed class StatModifierListMultiplyTotal : StatModifierList<SimpleStatModifier>
	{
		public StatModifierListMultiplyTotal() : base(defaultValue: 1) { }

		public override float Calculate(float value)
		{
			return value * CurrentValue;
		}

		protected override float AddOperation(float currentValue, SimpleStatModifier modifier)
		{
			return currentValue * (1 + modifier.Value);
		}

		protected override float RemoveOperation(float currentValue, SimpleStatModifier modifier)
		{
			return currentValue / (1 + modifier.Value);
		}
	}
}