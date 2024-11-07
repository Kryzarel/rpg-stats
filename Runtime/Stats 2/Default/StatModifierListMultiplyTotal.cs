using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats2
{
	public sealed class StatModifierListMultiplyTotal : StatModifierList
	{
		public StatModifierListMultiplyTotal() : base(defaultValue: 1) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Calculate(float statBaseValue, float statCurrentValue, float modifierValue)
		{
			return statCurrentValue * modifierValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float AddOperation(float modifierValue, StatModifier modifier)
		{
			return modifierValue * (1 + modifier.Value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float RemoveOperation(float modifierValue, StatModifier modifier)
		{
			return modifierValue / (1 + modifier.Value);
		}
	}
}