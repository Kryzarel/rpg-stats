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
					StatModifierType.Add => StatContainer<StatModifierData>.Create(new SimpleStatAdd<StatModifierData>(0), new AddOperation<StatModifierData>()),
					StatModifierType.Mult => StatContainer<StatModifierData>.Create(new SimpleStatAdd<StatModifierData>(1), new MultOperation<StatModifierData>()),
					StatModifierType.MultTotal => StatContainer<StatModifierData>.Create(new SimpleStatMult<StatModifierData>(1), new MultOperation<StatModifierData>()),
					StatModifierType.Max => StatContainer<StatModifierData>.Create(new SimpleStatMax<StatModifierData>(0), new MaxOperation<StatModifierData>()),
					StatModifierType.Min => StatContainer<StatModifierData>.Create(new SimpleStatMin<StatModifierData>(float.MaxValue), new MinOperation<StatModifierData>()),
					_ => throw new NotImplementedException(),
				};
			}
			return lists;
		}
	}
}