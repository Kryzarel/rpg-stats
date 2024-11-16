using System;

namespace Kryz.RPG.Stats
{
	public sealed class Stat : Stat<StatModifierData>
	{
		public Stat(float baseValue = 0) : base(baseValue, GetModifierLists()) { }

		public override void AddModifier(StatModifier<StatModifierData> modifier)
		{
			statContainers[(int)modifier.Data.Type].Stat.AddModifier(modifier);
			// CalculateFinalValue();
		}

		public override bool RemoveModifier(StatModifier<StatModifierData> modifier)
		{
			if (statContainers[(int)modifier.Data.Type].Stat.RemoveModifier(modifier))
			{
				// CalculateFinalValue();
				return true;
			}
			return false;
		}

		// protected override float CalculateFinalValue(float baseValue)
		// {
		// 	if (statContainers[3].Stat.ModifiersCount > 0)
		// 	{
		// 		return statContainers[3].Stat.FinalValue;
		// 	}
		// 	return (baseValue + statContainers[0].Stat.FinalValue) * statContainers[1].Stat.FinalValue * statContainers[2].Stat.FinalValue;
		// }

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
					StatModifierType.Add => new StatContainer<StatModifierData>(new SimpleStatAdd<StatModifierData>(0), AddOperation<StatModifierData>.Instance),
					StatModifierType.Multiply => new StatContainer<StatModifierData>(new SimpleStatAdd<StatModifierData>(1), MultiplyOperation<StatModifierData>.Instance),
					StatModifierType.MultiplyTotal => new StatContainer<StatModifierData>(new SimpleStatMult<StatModifierData>(1), MultiplyOperation<StatModifierData>.Instance),
					StatModifierType.Override => new StatContainer<StatModifierData>(new SimpleStatOverride<StatModifierData>(0), OverrideOperation<StatModifierData>.Instance),
					_ => throw new NotImplementedException(),
				};
			}
			return lists;
		}
	}
}