using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Kryz.Utils;

namespace Kryz.RPG.Stats.Core
{
	public class SimpleStatMin<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		private struct Comparer : IComparer<StatModifier<T>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public readonly int Compare(StatModifier<T> x, StatModifier<T> y)
			{
				return y.Value < x.Value ? -1 : y.Value > x.Value ? 1 : 0;
			}
		}

		private static readonly Comparer comparer = new();

		public SimpleStatMin(float baseValue = float.MaxValue) : base(baseValue) { }

		protected override void Add(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			if (modifier.Value <= currentValue)
			{
				modifiers.Add(modifier);
				return;
			}
			// Improves performance by inserting the modifiers sorted
			int index = modifiers.BinarySearchLeftmost(modifier, comparer);
			modifiers.Insert(index, modifier);
		}

		protected override bool Remove(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			int index = modifiers.BinarySearchLeftmost(modifier, comparer);
			if (index >= 0)
			{
				index = modifiers.IndexOf(modifier, index);
				modifiers.RemoveAt(index);
				return true;
			}
			return false;
		}

		protected override float AddOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			return modifier.Value <= currentValue ? modifier.Value : currentValue;
		}

		protected override float RemoveOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			return modifiers.Count > 0 ? Math.Min(modifiers[^1].Value, baseValue) : baseValue;
		}

		protected override float CalculateFinalValue(float baseValue, float currentValue)
		{
			return modifiers.Count > 0 ? Math.Min(modifiers[^1].Value, baseValue) : baseValue;
		}
	}
}