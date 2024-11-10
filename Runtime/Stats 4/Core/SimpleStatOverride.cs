using System.Collections.Generic;

namespace Kryz.RPG.Stats4
{
	public class SimpleStatOverride<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		public SimpleStatOverride() : base() { }

		protected override float AddOperation(float currentValue, StatModifier<T> modifier)
		{
			return modifier.Value >= currentValue ? modifier.Value : currentValue;
		}

		public override bool RemoveModifier(StatModifier<T> modifier)
		{
			bool removed = false;
			float maxValue = float.MinValue;
			for (int i = modifiers.Count - 1; i >= 0; i--)
			{
				StatModifier<T> mod = modifiers[i];
				if (mod == modifier)
				{
					modifiers.RemoveAt(i);
					removed = true;
				}
				else if (mod.Value > maxValue)
				{
					maxValue = mod.Value;
				}
			}
			finalValue = maxValue;
			return removed;
		}

		public override int RemoveWhere<TMatch>(TMatch match)
		{
			int removedCount = 0;
			float maxValue = float.MinValue;
			for (int i = modifiers.Count - 1; i >= 0; i--)
			{
				StatModifier<T> modifier = modifiers[i];

				if (match.IsMatch(modifier))
				{
					modifiers.RemoveAt(i);
					removedCount++;
				}
				else if (modifier.Value > maxValue)
				{
					maxValue = modifier.Value;
				}
			}
			finalValue = maxValue;
			return removedCount;
		}

		protected override float RemoveOperation(float currentValue, StatModifier<T> modifier)
		{
			throw new System.NotImplementedException();
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