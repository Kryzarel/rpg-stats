using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats2.Default
{
	public sealed class StatModifierListOverride : StatModifierList<StatModifier>
	{
		public StatModifierListOverride() : base(defaultValue: 1) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Calculate(float statBaseValue, float statCurrentValue, float modifierValue)
		{
			return modifiers.Count > 0 ? modifierValue : statCurrentValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float AddOperation(float modifierValue, StatModifier modifier)
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
		protected override float RemoveOperation(float modifierValue, StatModifier modifier)
		{
			return modifiers.Count > 0 ? modifiers[^1].Value : modifierValue;
		}

		private static int BinarySearchLeftmost(IReadOnlyList<StatModifier> list, StatModifier value)
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