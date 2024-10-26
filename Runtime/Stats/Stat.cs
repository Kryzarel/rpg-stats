using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats
{
	public sealed class Stat : Stat<StatModifier>
	{
		public Stat(float baseValue) : base(baseValue) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override float CalculateModifier(float currentValue, float baseValue, StatModifier modifier) => modifier.Type switch
		{
			StatModifierType.Add => currentValue + modifier.Value,
			StatModifierType.MultiplyBase => currentValue + baseValue * modifier.Value,
			StatModifierType.MultiplyTotal => currentValue + currentValue * modifier.Value,
			StatModifierType.Override => modifier.Value,
			_ => currentValue,
		};

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override void Sort(List<StatModifier> modifiers)
		{
			modifiers.Sort((a, b) => a.CompareTo(b));
		}
	}
}