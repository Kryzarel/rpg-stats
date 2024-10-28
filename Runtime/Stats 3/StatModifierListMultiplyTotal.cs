using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats3
{
	public sealed class StatModifierListMultiplyTotal<T> : StatModifierList<T> where T : struct, IStatModifier
	{
		public static readonly IStatModifierType<T> Type = new StatModifierType<T, StatModifierListMultiplyTotal<T>>(300);

		public override int Priority => Type.Priority;

		public StatModifierListMultiplyTotal() : base(defaultValue: 1) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Calculate(float statBaseValue, float statCurrentValue, float modifierValue)
		{
			return statCurrentValue * modifierValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float AddOperation(float modifierValue, T modifier)
		{
			return modifierValue * (1 + modifier.Value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float RemoveOperation(float modifierValue, T modifier)
		{
			return modifierValue / (1 + modifier.Value);
		}
	}
}