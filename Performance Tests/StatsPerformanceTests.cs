using Kryz.CharacterStats;
using Kryz.RPG.Stats.Core;
using Kryz.RPG.Stats.Default;
using UnityEngine;

namespace Kryz.RPG.StatsPerfTests
{
	public class StatsPerformanceTests : MonoBehaviour
	{
		private const int Length = 1000;

		private readonly StatModifier[] modifiersLegacy = new StatModifier[Length];
		private readonly StatModifier<StatModifierData>[] modifiers = new StatModifier<StatModifierData>[Length];

		private readonly CharacterStat statLegacy = new(10);
		private readonly Stat stat = new(10);

		private readonly object source1 = new();
		private readonly object source2 = new();

		private void Awake()
		{
			int modTypeLegacyCount = System.Enum.GetValues(typeof(StatModType)).Length;
			int modTypeCount = System.Enum.GetValues(typeof(StatModifierType)).Length;
			// int modTypeCount = 3;

			for (int i = 0; i < Length; i++)
			{
				float value = i;
				object source = i % 2 == 0 ? source1 : source2;

				StatModType modTypeLegacy = (StatModType)(i % modTypeLegacyCount);
				StatModifier modifierlegacy = new(value, modTypeLegacy, source);

				StatModifierType modType = (StatModifierType)(i % modTypeCount);
				StatModifier<StatModifierData> modifier = new(value, new(modType, source));

				modifiersLegacy[i] = modifierlegacy;
				modifiers[i] = modifier;
			}

			for (int i = 0; i < 100; i++)
			{
				statLegacy.AddModifier(new StatModifier(2, StatModType.Flat));
				stat.AddModifier(new StatModifier<StatModifierData>(2, new StatModifierData(StatModifierType.Add)));
			}
		}

		private void Update()
		{
			for (int i = 0; i < 10; i++)
			{
				// Legacy
				AddLegacy10();
				RemoveLegacy10();

				AddLegacy100();
				RemoveLegacy100();

				AddLegacy100();
				RemoveFromSourceLegacy();

				// New
				Add10();
				Remove10();

				Add100();
				Remove100();

				Add100();
				RemoveFromSource();
			}
		}

		private float AddLegacy(int count)
		{
			for (int i = 0; i < count; i++)
			{
				statLegacy.AddModifier(modifiersLegacy[i]);
			}
			return statLegacy.Value;
		}

		private float RemoveLegacy(int count)
		{
			for (int i = 0; i < count; i++)
			{
				statLegacy.RemoveModifier(modifiersLegacy[i]);
			}
			return statLegacy.Value;
		}

		private float AddLegacy10() => AddLegacy(10);
		private float AddLegacy100() => AddLegacy(100);

		private float RemoveLegacy10() => RemoveLegacy(10);
		private float RemoveLegacy100() => RemoveLegacy(100);

		private float RemoveFromSourceLegacy()
		{
			statLegacy.RemoveAllModifiersFromSource(source1);
			statLegacy.RemoveAllModifiersFromSource(source2);
			return statLegacy.Value;
		}

		private float Add(int count)
		{
			for (int i = 0; i < count; i++)
			{
				stat.AddModifier(modifiers[i]);
			}
			return stat.FinalValue;
		}

		private float Remove(int count)
		{
			for (int i = 0; i < count; i++)
			{
				stat.RemoveModifier(modifiers[i]);
			}
			return stat.FinalValue;
		}

		private float Add10() => Add(10);
		private float Add100() => Add(100);

		private float Remove10() => Remove(10);
		private float Remove100() => Remove(100);

		private float RemoveFromSource()
		{
			stat.RemoveModifiersFromSource(source1);
			stat.RemoveModifiersFromSource(source2);
			return stat.FinalValue;
		}
	}
}
