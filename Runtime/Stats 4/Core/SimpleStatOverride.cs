using System.Collections.Generic;

namespace Kryz.RPG.Stats4
{
	public class SimpleStatOverride<T> : SimpleStat<T> where T : struct, IStatModifierData
	{
		public SimpleStatOverride(float baseValue = 0) : base(baseValue) { }

		protected override float AddOperation(float currentValue, float modifierValue, T data)
		{
			if (modifierValue >= currentValue)
			{
				return modifierValue;
			}

			// Improves performance by essentially inserting the modifiers sorted
			int index = BinarySearchLeftmost(modifierValues, modifierValue);

			modifierValues.RemoveAt(modifierValues.Count - 1);
			modifierDatas.RemoveAt(modifierDatas.Count - 1);

			modifierValues.Insert(index, modifierValue);
			modifierDatas.Insert(index, data);

			return modifierValue;
		}

		protected override float RemoveOperation(float currentValue, float modifierValue, T data)
		{
			return modifierValues.Count > 0 ? modifierValues[^1] : currentValue;
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