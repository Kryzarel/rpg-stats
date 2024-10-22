using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats
{
	public sealed class StatModifierListMultiplyTotal : StatModifierList<SimpleStatModifier>
	{
		public StatModifierListMultiplyTotal() : base(defaultValue: 1) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override float Calculate(float value)
		{
			return value * CurrentValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float AddOperation(float currentValue, SimpleStatModifier modifier)
		{
			return currentValue * (1 + modifier.Value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float RemoveOperation(float currentValue, SimpleStatModifier modifier)
		{
			return currentValue / (1 + modifier.Value);
		}
	}
}