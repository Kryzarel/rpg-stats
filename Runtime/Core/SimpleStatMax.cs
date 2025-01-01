using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Kryz.Utils;

namespace Kryz.RPG.Stats.Core
{
	public class SimpleStatMax<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		private struct Comparer : IComparer<StatModifier<T>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public readonly int Compare(StatModifier<T> x, StatModifier<T> y)
			{
				return x.Value < y.Value ? -1 : x.Value > y.Value ? 1 : 0;
			}
		}

		private static readonly Comparer comparer = new();

		public SimpleStatMax(float baseValue = 0) : base(baseValue) { }

		protected override void Add(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			if (modifier.Value >= currentValue)
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
				if (index >= 0)
				{
					modifiers.RemoveAt(index);
				}
				return true;
			}
			return false;
		}

		protected override float AddOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			return modifier.Value >= currentValue ? modifier.Value : currentValue;
		}

		protected override float RemoveOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			return modifiers.Count > 0 ? Math.Max(modifiers[^1].Value, baseValue) : baseValue;
		}

		protected override float CalculateFinalValue(float baseValue)
		{
			return modifiers.Count > 0 ? Math.Max(modifiers[^1].Value, baseValue) : baseValue;
		}
	}
}