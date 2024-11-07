using System;
using Kryz.RPG.Stats2.Core;

namespace Kryz.RPG.Stats2
{
	public sealed class Stat : Stat<StatModifier, StatModifierList>, IStat
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

		public int RemoveModifiersFromSource(object source)
		{
			int numRemoved = 0;
			for (int i = 0; i < modifierLists.Length; i++)
			{
				numRemoved += modifierLists[i].RemoveModifiersFromSource(source);
			}
			if (numRemoved > 0)
			{
				CalculateFinalValue();
			}
			return numRemoved;
		}

		private static readonly StatModifierType[] modifierTypes = (StatModifierType[])Enum.GetValues(typeof(StatModifierType));

		private static StatModifierList[] GetModifierLists()
		{
			StatModifierList[] lists = new StatModifierList[modifierTypes.Length];

			for (int i = 0; i < modifierTypes.Length; i++)
			{
				StatModifierType type = modifierTypes[i];
				lists[i] = type switch
				{
					StatModifierType.Add => new StatModifierListAdd(),
					StatModifierType.MultiplyBase => new StatModifierListMultiplyBase(),
					StatModifierType.MultiplyTotal => new StatModifierListMultiplyTotal(),
					StatModifierType.Override => new StatModifierListOverride(),
					_ => throw new NotImplementedException(),
				};
			}
			return lists;
		}
	}
}