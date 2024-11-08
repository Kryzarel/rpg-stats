using System;

namespace Kryz.RPG.Stats4
{
	public sealed class Stat : Stat<StatModifierdata>
	{
		public Stat(float baseValue = 0) : base(baseValue, GetModifierLists()) { }

		public void AddModifier(StatModifier modifier)
		{
			AddModifier((int)modifier.Data.Type, modifier.Value, modifier.Data);
		}

		public bool RemoveModifier(StatModifier modifier)
		{
			return RemoveModifier((int)modifier.Data.Type, modifier.Value, modifier.Data);
		}

		public int RemoveModifiersFromSource(object source)
		{
			return RemoveWhere(new StatModifierMatch(source: source));
		}

		public override void AddModifier(float value, StatModifierdata data)
		{
			AddModifier((int)data.Type, value, data);
		}

		public override bool RemoveModifier(float value, StatModifierdata data)
		{
			return RemoveModifier((int)data.Type, value, data);
		}

		private static readonly StatModifierType[] modifierTypes = (StatModifierType[])Enum.GetValues(typeof(StatModifierType));

		private static SimpleStat<StatModifierdata>[] GetModifierLists()
		{
			SimpleStat<StatModifierdata>[] lists = new SimpleStat<StatModifierdata>[modifierTypes.Length];

			for (int i = 0; i < modifierTypes.Length; i++)
			{
				StatModifierType type = modifierTypes[i];
				lists[i] = type switch
				{
					StatModifierType.Add => new SimpleStatAdd<StatModifierdata>(0),
					StatModifierType.MultiplyBase => new SimpleStatAdd<StatModifierdata>(1),
					StatModifierType.MultiplyTotal => new SimpleStatMult<StatModifierdata>(1),
					StatModifierType.Override => new SimpleStatOverride<StatModifierdata>(0),
					_ => throw new NotImplementedException(),
				};
			}
			return lists;
		}
	}
}