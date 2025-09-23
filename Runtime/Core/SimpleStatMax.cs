namespace Kryz.RPG.Stats.Core
{
	public class SimpleStatMax<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		public SimpleStatMax(float baseValue = 0) : base(baseValue) { }

		protected override bool AddOperation(StatModifier<T> modifier, float baseValue, float currentValue, out float finalValue)
		{
			finalValue = modifier.Value > currentValue ? modifier.Value : currentValue;
			return false;
		}

		protected override bool RemoveOperation(StatModifier<T> modifier, float baseValue, float currentValue, out float finalValue)
		{
			finalValue = currentValue;
			return modifier.Value == currentValue;
		}

		protected override bool SetBaseValue(float newBaseValue, float oldBaseValue, float currentValue, out float finalValue)
		{
			finalValue = newBaseValue > currentValue ? newBaseValue : currentValue;
			return oldBaseValue == currentValue;
		}
	}
}