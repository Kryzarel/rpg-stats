using System;

namespace Kryz.RPG.Stats.Core
{
	public class SimpleStatMin<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		public SimpleStatMin(float baseValue = float.MaxValue) : base(baseValue) { }

		protected override void Add(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			if (modifier.Value <= currentValue)
			{
				Add(modifier);
			}
			else
			{
				// Improves performance by inserting the modifiers sorted
				int index = BinarySearchLeftmost(modifiers, modifier.Value, count);
				Insert(index, modifier);
			}
		}

		protected override float AddOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			return modifier.Value <= currentValue ? modifier.Value : currentValue;
		}

		protected override float RemoveOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			return count > 0 ? Math.Min(modifiers[count - 1].Value, baseValue) : baseValue;
		}

		protected override float CalculateFinalValue(float baseValue, float currentValue)
		{
			return count > 0 ? Math.Min(modifiers[count - 1].Value, baseValue) : baseValue;
		}

		private static int BinarySearchLeftmost(StatModifier<T>[] modifiers, float value, int count)
		{
			int min = 0;
			int max = count;

			while (min < max)
			{
				int mid = (min + max) / 2;

				if (modifiers[mid].Value > value)
					min = mid + 1;
				else
					max = mid;
			}
			return min;
		}
	}
}