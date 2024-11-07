using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats2.Default
{
	public sealed class StatModifierListAdd : StatModifierList<StatModifier>
	{
		public StatModifierListAdd() : base(defaultValue: 0) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Calculate(float statBaseValue, float statCurrentValue, float modifierValue)
		{
			return statCurrentValue + modifierValue;
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