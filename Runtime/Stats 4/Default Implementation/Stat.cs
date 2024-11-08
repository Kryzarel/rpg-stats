using System;

namespace Kryz.RPG.Stats4
{
	public sealed class Stat : Stat<StatModifierMetaData, StatModifierList<StatModifierMetaData>>
	{
		public Stat(float baseValue = 0) : base(baseValue, GetModifierLists()) { }

		public void AddModifier(StatModifier modifier)
		{
			AddModifier((int)modifier.MetaData.Type, modifier.Value, modifier.MetaData);
		}

		public bool RemoveModifier(StatModifier modifier)
		{
			return RemoveModifier((int)modifier.MetaData.Type, modifier.Value, modifier.MetaData);
		}

		public int RemoveModifiersFromSource(object source)
		{
			return RemoveWhere((value, metaData) => metaData.Source == source);
		}

		public override void AddModifier(float value, StatModifierMetaData metaData)
		{
			AddModifier((int)metaData.Type, value, metaData);
		}

		public override bool RemoveModifier(float value, StatModifierMetaData metaData)
		{
			return RemoveModifier((int)metaData.Type, value, metaData);
		}

		private static readonly StatModifierType[] modifierTypes = (StatModifierType[])Enum.GetValues(typeof(StatModifierType));

		private static StatModifierList<StatModifierMetaData>[] GetModifierLists()
		{
			StatModifierList<StatModifierMetaData>[] lists = new StatModifierList<StatModifierMetaData>[modifierTypes.Length];

			for (int i = 0; i < modifierTypes.Length; i++)
			{
				StatModifierType type = modifierTypes[i];
				lists[i] = type switch
				{
					StatModifierType.Add => new StatModifierListAdd<StatModifierMetaData>(),
					StatModifierType.MultiplyBase => new StatModifierListMultiply<StatModifierMetaData>(),
					StatModifierType.MultiplyTotal => new StatModifierListMultiplyTotal<StatModifierMetaData>(),
					StatModifierType.Override => new StatModifierListOverride<StatModifierMetaData>(),
					_ => throw new NotImplementedException(),
				};
			}
			return lists;
		}
	}
}