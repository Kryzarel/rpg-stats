using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats2
{
	public sealed class StatModifierListOverride<T> : StatModifierList<T> where T : struct, IStatModifier
	{
		public StatModifierListOverride() : base(defaultValue: 1) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Calculate(float statBaseValue, float statCurrentValue, float modifierValue)
		{
			return modifierValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float AddOperation(float modifierValue, T modifier)
		{
			if (modifier.Value >= modifierValue)
			{
				return modifier.Value;
			}

			// Improves performance by essentially inserting the modifiers sorted
			int index = BinarySearchLeftmost(modifiers, modifier);
			modifiers.RemoveAt(modifiers.Count - 1);
			modifiers.Insert(index, modifier);

			return modifierValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float RemoveOperation(float modifierValue, T modifier)
		{
			return modifiers[^1].Value;
		}

		private static int BinarySearchLeftmost(IReadOnlyList<T> list, T value)
		{
			int min = 0;
			int max = list.Count;

			while (min < max)
			{
				int mid = (min + max) / 2;

				if (list[mid].Value < value.Value)
					min = mid + 1;
				else
					max = mid;
			}
			return min;
		}
	}
}