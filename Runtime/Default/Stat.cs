using System;
using Kryz.RPG.Stats.Core;

namespace Kryz.RPG.Stats.Default
{
	public sealed class Stat : Stat<StatModifierData>
	{
		public Stat(float baseValue = 0) : base(baseValue, GetInnerStats()) { }

		public override void AddModifier(StatModifier<StatModifierData> modifier)
		{
			stats[(int)modifier.Data.Type].AddModifier(modifier);
		}

		public override bool RemoveModifier(StatModifier<StatModifierData> modifier)
		{
			return stats[(int)modifier.Data.Type].RemoveModifier(modifier);
		}

		public int RemoveModifiersFromSource(object source)
		{
			return RemoveAllModifiers(new StatModifierMatch(source: source));
		}

		protected override float CalculateFinalValue(float baseValue)
		{
			float finalValue = baseValue;
			finalValue += stats[(int)StatModifierType.Add].FinalValue;
			finalValue *= stats[(int)StatModifierType.Mult].FinalValue;
			finalValue *= stats[(int)StatModifierType.MultTotal].FinalValue;
			finalValue = Math.Max(finalValue, stats[(int)StatModifierType.Max].FinalValue);
			finalValue = Math.Min(finalValue, stats[(int)StatModifierType.Min].FinalValue);
			return finalValue;
		}

		private static SimpleStat<StatModifierData>[] GetInnerStats()
		{
			StatModifierType[] modifierTypes = (StatModifierType[])Enum.GetValues(typeof(StatModifierType));
			SimpleStat<StatModifierData>[] stats = new SimpleStat<StatModifierData>[modifierTypes.Length];

			for (int i = 0; i < modifierTypes.Length; i++)
			{
				stats[i] = modifierTypes[i] switch
				{
					StatModifierType.Add => new SimpleStatAdd<StatModifierData>(0),
					StatModifierType.Mult => new SimpleStatAdd<StatModifierData>(1),
					StatModifierType.MultTotal => new SimpleStatMult<StatModifierData>(1),
					StatModifierType.Max => new SimpleStatMax<StatModifierData>(0),
					StatModifierType.Min => new SimpleStatMin<StatModifierData>(float.MaxValue),
					_ => throw new NotImplementedException(),
				};
			}
			return stats;
		}
	}
}