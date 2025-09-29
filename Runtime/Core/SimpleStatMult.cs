namespace Kryz.RPG.Stats.Core
{
	public class SimpleStatMult<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		private int zeroesCount;
		private float nonZeroValue;

		public SimpleStatMult(float baseValue = 1) : base(baseValue)
		{
			OnClear(baseValue);
		}

		protected override bool AddOperation(StatModifier<T> modifier, float baseValue, float currentValue, out float newValue)
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

			newValue = zeroesCount > 0 ? 0 : nonZeroValue;
			return false;
		}

		protected override bool RemoveOperation(StatModifier<T> modifier, float baseValue, float currentValue, out float newValue)
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

			newValue = zeroesCount > 0 ? 0 : nonZeroValue;
			return false;
		}

		protected override bool SetBaseValue(float newBaseValue, float oldBaseValue, float currentValue, out float newValue)
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

			newValue = zeroesCount > 0 ? 0 : nonZeroValue;
			return false;
		}

		protected override void OnClear(float baseValue)
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
	}
}