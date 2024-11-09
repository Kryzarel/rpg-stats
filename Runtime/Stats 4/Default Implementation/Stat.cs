using System;

namespace Kryz.RPG.Stats4
{
	public sealed class Stat : Stat<StatModifierData>
	{
		public Stat(float baseValue = 0) : base(baseValue, GetModifierLists()) { }

		public override void AddModifier(StatModifier<StatModifierData> modifier)
		{
			AddModifier((int)modifier.Data.Type, modifier);
		}

		public override bool RemoveModifier(StatModifier<StatModifierData> modifier)
		{
			return RemoveModifier((int)modifier.Data.Type, modifier);
		}

		public int RemoveModifiersFromSource(object source)
		{
			return RemoveWhere(new StatModifierMatch(source: source));
		}

		private static readonly StatModifierType[] modifierTypes = (StatModifierType[])Enum.GetValues(typeof(StatModifierType));

		private static StatContainer<StatModifierData, IStat<StatModifierData>>[] GetModifierLists()
		{
			var lists = new StatContainer<StatModifierData, IStat<StatModifierData>>[modifierTypes.Length];

			for (int i = 0; i < modifierTypes.Length; i++)
			{
				StatModifierType type = modifierTypes[i];
				lists[i] = type switch
				{
					StatModifierType.Add => new(new SimpleStatAdd<StatModifierData>(0), AddOperation.Instance),
					StatModifierType.Multiply => new(new SimpleStatAdd<StatModifierData>(1), MultiplyOperation.Instance),
					StatModifierType.MultiplyTotal => new(new SimpleStatMult<StatModifierData>(1), MultiplyOperation.Instance),
					StatModifierType.Override => new(new SimpleStatOverride<StatModifierData>(0), OverrideOperation.Instance),
					_ => throw new NotImplementedException(),
				};
			}
			return lists;
		}

		private class AddOperation : IStatOperation
		{
			public static readonly AddOperation Instance = new();
			public float Calculate(float statValue, IStat innerStat) => statValue + innerStat.FinalValue;
		}

		private class MultiplyOperation : IStatOperation
		{
			public static readonly MultiplyOperation Instance = new();
			public float Calculate(float statValue, IStat innerStat) => statValue * innerStat.FinalValue;
		}

		private class OverrideOperation : IStatOperation
		{
			public static readonly OverrideOperation Instance = new();
			public float Calculate(float statValue, IStat innerStat) => innerStat.ModifiersCount > 0 ? innerStat.FinalValue : statValue;
		}
	}
}