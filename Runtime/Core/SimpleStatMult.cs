namespace Kryz.RPG.Stats.Core
{
	public class SimpleStatMult<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		public SimpleStatMult(float baseValue = 1) : base(baseValue) { }

		protected override float AddOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			return currentValue * (1 + modifier.Value);
		}

		protected override float RemoveOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			return currentValue / (1 + modifier.Value);
		}

		protected override float ChangeBaseValue(float oldBaseValue, float newBaseValue, float currentValue)
		{
			if (newBaseValue == 0)
			{
				return 0;
			}

			if (oldBaseValue == 0 || currentValue == 0)
			{
				return base.ChangeBaseValue(oldBaseValue, newBaseValue, currentValue);
			}

			return currentValue / oldBaseValue * newBaseValue;
		}
	}
}