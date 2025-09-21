namespace Kryz.RPG.Stats.Core
{
	public class SimpleStatMult<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		private int zeroesCount;
		private float nonZeroValue;

		public SimpleStatMult(float baseValue = 1) : base(baseValue)
		{
			if (baseValue == 0)
			{
				zeroesCount = 1;
				nonZeroValue = 1;
			}
			else
			{
				zeroesCount = 0;
				nonZeroValue = baseValue;
			}
		}

		protected override float AddOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			float value = 1 + modifier.Value;

			if (value == 0)
			{
				zeroesCount++;
			}
			else
			{
				nonZeroValue *= value;
			}

			return zeroesCount > 0 ? 0 : nonZeroValue;
		}

		protected override float RemoveOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			float value = 1 + modifier.Value;

			if (value == 0)
			{
				zeroesCount--;
			}
			else
			{
				nonZeroValue /= value;
			}

			return zeroesCount > 0 ? 0 : nonZeroValue;
		}

		protected override float SetBaseValue(float oldBaseValue, float newBaseValue, float currentValue)
		{
			if (oldBaseValue == 0)
			{
				zeroesCount--;
			}
			else
			{
				nonZeroValue /= oldBaseValue;
			}

			if (newBaseValue == 0)
			{
				zeroesCount++;
			}
			else
			{
				nonZeroValue *= newBaseValue;
			}

			return zeroesCount > 0 ? 0 : nonZeroValue;
		}
	}
}