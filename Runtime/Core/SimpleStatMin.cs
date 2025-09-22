namespace Kryz.RPG.Stats.Core
{
	public class SimpleStatMin<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		public SimpleStatMin(float baseValue = float.MaxValue) : base(baseValue) { }

		protected override bool AddOperation(StatModifier<T> modifier, float baseValue, float currentValue, out float finalValue)
		{
			finalValue = currentValue <= modifier.Value ? currentValue : modifier.Value;
			return false;
		}

		protected override bool RemoveOperation(StatModifier<T> modifier, float baseValue, float currentValue, out float finalValue)
		{
			finalValue = currentValue;
			return modifier.Value <= currentValue;
		}

		protected override bool SetBaseValue(float newBaseValue, float oldBaseValue, float currentValue, out float finalValue)
		{
			finalValue = currentValue <= newBaseValue ? currentValue : newBaseValue;
			return false;
		}
	}
}