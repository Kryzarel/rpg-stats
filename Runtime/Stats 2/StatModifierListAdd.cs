using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats2
{
	public sealed class StatModifierListAdd : StatModifierList<StatModifier>
	{
		public StatModifierListAdd() : base(defaultValue: 0) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Calculate(float value, float currentValue)
		{
			return value + currentValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float AddOperation(float currentValue, StatModifier modifier)
		{
			return currentValue + modifier.Value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float RemoveOperation(float currentValue, StatModifier modifier)
		{
			return currentValue - modifier.Value;
		}
	}
}