namespace Kryz.RPG.Stats.Core
{
	public class SimpleStatMin<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		public SimpleStatMin(float baseValue = float.MaxValue) : base(baseValue) { }

		protected override bool AddOperation(StatModifier<T> modifier, float baseValue, float currentValue, out float newValue)
		{
			newValue = modifier.Value < currentValue ? modifier.Value : currentValue;
			return false;
		}

		protected override bool RemoveOperation(StatModifier<T> modifier, float baseValue, float currentValue, out float newValue)
		{
			newValue = currentValue;
			return modifier.Value == currentValue;
		}

		protected override bool SetBaseValue(float newBaseValue, float oldBaseValue, float currentValue, out float newValue)
		{
			newValue = newBaseValue < currentValue ? newBaseValue : currentValue;
			return oldBaseValue == currentValue;
		}
	}
}