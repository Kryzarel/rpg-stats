using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats2.Default
{
	public sealed class StatModifierListMultiplyBase : StatModifierList<StatModifier>
	{
		public StatModifierListMultiplyBase() : base(defaultValue: 1) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Calculate(float statBaseValue, float statCurrentValue, float modifierValue)
		{
			return statCurrentValue + statBaseValue * modifierValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float AddOperation(float modifierValue, StatModifier modifier)
		{
			return modifierValue + modifier.Value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float RemoveOperation(float modifierValue, StatModifier modifier)
		{
			return modifierValue - modifier.Value;
		}
	}
}