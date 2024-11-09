using System;

namespace Kryz.RPG.Stats4
{
	public sealed class Stat : Stat<StatModifierData>
	{
		public Stat(float baseValue = 0) : base(baseValue, GetModifierLists()) { }

		public override void AddModifier(StatModifier<StatModifierData> modifier)
		{
			statContainers[(int)modifier.Data.Type].Stat.AddModifier(modifier);
		}

		public override bool RemoveModifier(StatModifier<StatModifierData> modifier)
		{
			return statContainers[(int)modifier.Data.Type].Stat.RemoveModifier(modifier);
		}

		public int RemoveModifiersFromSource(object source)
		{
			return RemoveWhere(new StatModifierMatch(source: source));
		}

		private static readonly StatModifierType[] modifierTypes = (StatModifierType[])Enum.GetValues(typeof(StatModifierType));

		private static StatContainer<StatModifierData>[] GetModifierLists()
		{
			var lists = new StatContainer<StatModifierData>[modifierTypes.Length];

			for (int i = 0; i < modifierTypes.Length; i++)
			{
				StatModifierType type = modifierTypes[i];
				lists[i] = type switch
				{
					StatModifierType.Add => new(new SimpleStatAdd<StatModifierData>(0), AddOperation<StatModifierData>.Instance),
					StatModifierType.Multiply => new(new SimpleStatAdd<StatModifierData>(1), MultiplyOperation<StatModifierData>.Instance),
					StatModifierType.MultiplyTotal => new(new SimpleStatMult<StatModifierData>(1), MultiplyOperation<StatModifierData>.Instance),
					StatModifierType.Override => new(new SimpleStatOverride<StatModifierData>(), OverrideOperation<StatModifierData>.Instance),
					_ => throw new NotImplementedException(),
				};
			}
			return lists;
		}
	}
}