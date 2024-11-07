using System;

namespace Kryz.RPG.Stats2.Default
{
	public sealed class Stat : Stat<StatModifier>, IStat<StatModifier>
	{
		public Stat(float baseValue = 0) : base(baseValue, GetModifierLists()) { }

		public override void AddModifier(StatModifier modifier)
		{
			AddModifier((int)modifier.Type, modifier);
		}

		public override bool RemoveModifier(StatModifier modifier)
		{
			return RemoveModifier((int)modifier.Type, modifier);
		}

		private static readonly StatModifierType[] modifierTypes = (StatModifierType[])Enum.GetValues(typeof(StatModifierType));

		private static StatModifierList<StatModifier>[] GetModifierLists()
		{
			StatModifierList<StatModifier>[] lists = new StatModifierList<StatModifier>[modifierTypes.Length];

			for (int i = 0; i < modifierTypes.Length; i++)
			{
				StatModifierType type = modifierTypes[i];
				lists[i] = type switch
				{
					StatModifierType.Add => new StatModifierListAdd<StatModifier>(),
					StatModifierType.MultiplyBase => new StatModifierListMultiplyBase<StatModifier>(),
					StatModifierType.MultiplyTotal => new StatModifierListMultiplyTotal<StatModifier>(),
					StatModifierType.Override => new StatModifierListOverride<StatModifier>(),
					_ => throw new NotImplementedException(),
				};
			}
			return lists;
		}
	}
}