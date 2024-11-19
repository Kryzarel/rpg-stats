using System;
using System.Collections.Generic;

namespace Kryz.RPG.Stats.Core
{
	public class SimpleStatMin<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		public SimpleStatMin(float baseValue = float.MaxValue) : base(baseValue) { }

		protected override float AddOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			if (modifier.Value <= currentValue)
			{
				return modifier.Value;
			}

			// Improves performance by essentially inserting the modifiers sorted
			int index = BinarySearchLeftmost(modifiers, modifier.Value);
			modifiers.RemoveAt(modifiers.Count - 1);
			modifiers.Insert(index, modifier);
			return currentValue;
		}

		protected override float RemoveOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			return modifiers.Count > 0 ? Math.Min(modifiers[^1].Value, baseValue) : baseValue;
		}

		protected override float CalculateFinalValue(float baseValue, float currentValue)
		{
			return modifiers.Count > 0 ? Math.Min(modifiers[^1].Value, baseValue) : baseValue;
		}

		private static int BinarySearchLeftmost(IReadOnlyList<StatModifier<T>> list, float value)
		{
			int min = 0;
			int max = list.Count;

			while (min < max)
			{
				int mid = (min + max) / 2;

				if (list[mid].Value > value)
					min = mid + 1;
				else
					max = mid;
			}
			return min;
		}
	}
}