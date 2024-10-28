using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats3
{
	public sealed class StatModifierListAdd<T> : StatModifierList<T> where T : struct, IStatModifier
	{
		public static readonly IStatModifierType<T> Type = new StatModifierType<T, StatModifierListAdd<T>>(100);

		public override int Priority => Type.Priority;

		public StatModifierListAdd() : base(defaultValue: 0) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float Calculate(float statBaseValue, float statCurrentValue, float modifierValue)
		{
			return statCurrentValue + modifierValue;
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