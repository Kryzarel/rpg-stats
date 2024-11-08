using System.Collections.Generic;

namespace Kryz.RPG.Stats4
{
	public class StatModifierListOverride<T> : StatModifierList<T> where T : struct, IStatModifierMetaData
	{
		public StatModifierListOverride() : base(defaultValue: 0) { }

		protected override float AddOperation(float currentValue, float modifierValue, T metaData)
		{
			if (modifierValue >= currentValue)
			{
				return modifierValue;
			}

			// Improves performance by essentially inserting the modifiers sorted
			int index = BinarySearchLeftmost(modifiers, modifierValue);

			modifiers.RemoveAt(modifiers.Count - 1);
			metaDatas.RemoveAt(metaDatas.Count - 1);

			modifiers.Insert(index, modifierValue);
			metaDatas.Insert(index, metaData);

			return modifierValue;
		}

		protected override float RemoveOperation(float currentValue, float modifierValue, T metaData)
		{
			return modifiers[^1];
		}

		protected override float Calculate(float statCurrentValue, float modifierValue)
		{
			return modifiers.Count > 0 ? modifierValue : statCurrentValue;
		}

		private static int BinarySearchLeftmost(IReadOnlyList<float> list, float value)
		{
			int min = 0;
			int max = list.Count;

			while (min < max)
			{
				int mid = (min + max) / 2;

				if (list[mid] < value)
					min = mid + 1;
				else
					max = mid;
			}
			return min;
		}
	}
}