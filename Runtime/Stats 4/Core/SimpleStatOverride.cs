using System.Collections.Generic;

namespace Kryz.RPG.Stats4
{
	public class SimpleStatOverride<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		public SimpleStatOverride(float baseValue = 0) : base(baseValue) { }

		protected override float AddOperation(float currentValue, StatModifier<T> modifier)
		{
			if (modifier.Value >= currentValue)
			{
				return modifier.Value;
			}

			// Improves performance by essentially inserting the modifiers sorted
			int index = BinarySearchLeftmost(modifiers, modifier.Value);
			modifiers.RemoveAt(modifiers.Count - 1);
			modifiers.Insert(index, modifier);
			return modifier.Value;
		}

		protected override float RemoveOperation(float currentValue, StatModifier<T> modifier)
		{
			return modifiers.Count > 0 ? modifiers[^1].Value : currentValue;
		}

		protected override float CalculateFinalValue(float currentValue)
		{
			return modifiers.Count > 0 ? modifiers[^1].Value : currentValue;
		}

		private static int BinarySearchLeftmost(IReadOnlyList<StatModifier<T>> list, float value)
		{
			int min = 0;
			int max = list.Count;

			while (min < max)
			{
				int mid = (min + max) / 2;

				if (list[mid].Value < value)
					min = mid + 1;
				else
					max = mid;
			}
			return min;
		}
	}
}