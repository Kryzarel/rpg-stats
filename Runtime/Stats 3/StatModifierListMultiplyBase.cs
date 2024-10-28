using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats3
{
	public sealed class StatModifierListMultiplyBase<T> : StatModifierList<T> where T : struct, IStatModifier
	{
		public static readonly IStatModifierType<T> Type = new StatModifierType<T, StatModifierListMultiplyBase<T>>();

		public override int Priority => 200;

		public StatModifierListMultiplyBase() : base(defaultValue: 1) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Calculate(float statBaseValue, float statCurrentValue, float modifierValue)
		{
			return statCurrentValue + statBaseValue * modifierValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float AddOperation(float modifierValue, T modifier)
		{
			return modifierValue + modifier.Value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float RemoveOperation(float modifierValue, T modifier)
		{
			return modifierValue - modifier.Value;
		}
	}
}