using System;

namespace Kryz.RPG.Stats2
{
	public sealed class Stat : Stat<StatModifier, StatModifierList<StatModifier>>
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
				numRemoved += modifierLists[i].RemoveFromSource(source);
			}
			if (numRemoved > 0)
			{
				CalculateFinalValue();

			}
			return numRemoved;
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
					StatModifierType.Add => new StatModifierListAdd(),
					StatModifierType.MultiplyBase => new StatModifierListMultiplyBase(),
					StatModifierType.MultiplyTotal => new StatModifierListMultiplyTotal(),
					StatModifierType.Override => throw new NotImplementedException(),
					_ => throw new NotImplementedException(),
				};
			}
			return lists;
		}
	}
}