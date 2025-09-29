namespace Kryz.RPG.Stats.Core
{
	public class SimpleStatAdd<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		public SimpleStatAdd(float baseValue = 0) : base(baseValue) { }

		protected override bool AddOperation(StatModifier<T> modifier, float baseValue, float currentValue, out float newValue)
		{
			newValue = currentValue + modifier.Value;
			return false;
		}

		protected override bool RemoveOperation(StatModifier<T> modifier, float baseValue, float currentValue, out float newValue)
		{
			newValue = currentValue - modifier.Value;
			return false;
		}

		protected override bool SetBaseValue(float newBaseValue, float oldBaseValue, float currentValue, out float newValue)
		{
			newValue = currentValue - oldBaseValue + newBaseValue;
			return false;
		}
	}
}