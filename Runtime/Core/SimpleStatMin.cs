using System;
using Kryz.Utils;

namespace Kryz.RPG.Stats.Core
{
	public class SimpleStatMin<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		private readonly PooledList<StatModifier<T>> modifiers = new();

		public SimpleStatMin(float baseValue = float.MaxValue) : base(baseValue) { }

		protected override void Add(StatModifier<T> modifier)
		{
			// Improves performance by inserting the modifiers sorted
			int index = modifiers.BinarySearch(modifier, StatModifierComparer<T>.Default);
			if (index < 0) index = ~index;

			modifiers.Insert(index, modifier);
		}

		protected override bool Remove(StatModifier<T> modifier)
		{
			int index = modifiers.BinarySearch(modifier, StatModifierComparer<T>.Default);
			if (index < 0) return false;

			modifiers.RemoveAt(index);
			return true;
		}

		protected override int RemoveAll<TEquatable>(float baseValue, float currentValue, TEquatable match, out float finalValue)
		{
			finalValue = currentValue;
			int removedCount = modifiers.RemoveAll(match);
			if (removedCount > 0)
			{
				finalValue = modifiers.Count > 0 ? Math.Max(modifiers[^1].Value, baseValue) : baseValue;
			}
			return removedCount;
		}

		protected override float AddOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			return modifier.Value <= currentValue ? modifier.Value : currentValue;
		}

		protected override float RemoveOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			return modifiers.Count > 0 ? Math.Min(modifiers[0].Value, baseValue) : baseValue;
		}

		protected override float SetBaseValue(float oldBaseValue, float newBaseValue, float currentValue)
		{
			return modifiers.Count > 0 ? Math.Min(modifiers[0].Value, newBaseValue) : newBaseValue;
		}
	}
}