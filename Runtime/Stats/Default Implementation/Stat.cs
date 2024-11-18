using System;

namespace Kryz.RPG.Stats
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

		protected override float CalculateFinalValue(float baseValue)
		{
			float result = (baseValue + statContainers[0].Stat.FinalValue) * statContainers[1].Stat.FinalValue * statContainers[2].Stat.FinalValue;
			result = Math.Max(result, statContainers[3].Stat.FinalValue);
			result = Math.Min(result, statContainers[4].Stat.FinalValue);
			return result;
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
					StatModifierType.Add => new StatContainer<StatModifierData>(new SimpleStatAdd<StatModifierData>(0), AddOperation<StatModifierData>.Default),
					StatModifierType.Mult => new StatContainer<StatModifierData>(new SimpleStatAdd<StatModifierData>(1), MultOperation<StatModifierData>.Default),
					StatModifierType.MultTotal => new StatContainer<StatModifierData>(new SimpleStatMult<StatModifierData>(1), MultOperation<StatModifierData>.Default),
					StatModifierType.Max => new StatContainer<StatModifierData>(new SimpleStatMax<StatModifierData>(0), MaxOperation<StatModifierData>.Default),
					StatModifierType.Min => new StatContainer<StatModifierData>(new SimpleStatMin<StatModifierData>(float.MaxValue), MinOperation<StatModifierData>.Default),
					_ => throw new NotImplementedException(),
				};
			}
			return lists;
		}
	}
}