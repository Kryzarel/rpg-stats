using System;
using Kryz.RPG.Stats.Core;

namespace Kryz.RPG.Stats.Default
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
			return RemoveAll(new StatModifierMatch(source: source));
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
					StatModifierType.Add => new StatContainerAdd<StatModifierData, SimpleStatAdd<StatModifierData>>(new SimpleStatAdd<StatModifierData>(0)),
					StatModifierType.Mult => new StatContainerMult<StatModifierData, SimpleStatAdd<StatModifierData>>(new SimpleStatAdd<StatModifierData>(1)),
					StatModifierType.MultTotal => new StatContainerMult<StatModifierData, SimpleStatMult<StatModifierData>>(new SimpleStatMult<StatModifierData>(1)),
					StatModifierType.Max => new StatContainerMax<StatModifierData, SimpleStatMax<StatModifierData>>(new SimpleStatMax<StatModifierData>(0)),
					StatModifierType.Min => new StatContainerMin<StatModifierData, SimpleStatMin<StatModifierData>>(new SimpleStatMin<StatModifierData>(float.MaxValue)),
					_ => throw new NotImplementedException(),
				};
			}
			return lists;
		}
	}
}