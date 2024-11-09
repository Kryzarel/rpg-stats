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

		private static StatContainer<StatModifierdata, IStat<StatModifierdata>>[] GetModifierLists()
		{
			var lists = new StatContainer<StatModifierdata, IStat<StatModifierdata>>[modifierTypes.Length];

			for (int i = 0; i < modifierTypes.Length; i++)
			{
				StatModifierType type = modifierTypes[i];
				lists[i] = type switch
				{
					StatModifierType.Add => new(new SimpleStatAdd<StatModifierdata>(0), Add),
					StatModifierType.Multiply => new(new SimpleStatAdd<StatModifierdata>(1), Multiply),
					StatModifierType.MultiplyTotal => new(new SimpleStatMult<StatModifierdata>(1), Multiply),
					StatModifierType.Override => new(new SimpleStatOverride<StatModifierdata>(0), Override),
					_ => throw new NotImplementedException(),
				};
			}
			return lists;
		}

		private static float Add(float a, IStat<StatModifierdata> b) => a + b.FinalValue;
		private static float Multiply(float a, IStat<StatModifierdata> b) => a * b.FinalValue;
		private static float Override(float a, IStat<StatModifierdata> b) => b.ModifierValues.Count > 0 ? b.FinalValue : a;
	}
}