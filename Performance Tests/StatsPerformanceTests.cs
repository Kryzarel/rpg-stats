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
			// int modTypeCount = System.Enum.GetValues(typeof(StatModifierType)).Length;
			int modTypeCount = 3;

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

				// AddLegacy1000();
				// RemoveLegacy1000();

				// AddLegacy1000();
				AddLegacy100();
				RemoveFromSourceLegacy();

				// New
				Add10();
				Remove10();

				Add100();
				Remove100();

				// Add1000();
				// Remove1000();

				// Add1000();
				Add100();
				RemoveFromSource();
			}
		}

		private float AddLegacy10()
		{
			for (int i = 0; i < 10; i++)
			{
				statLegacy.AddModifier(modifiersLegacy[i]);
			}
			return statLegacy.Value;
		}

		private float AddLegacy100()
		{
			for (int i = 0; i < 100; i++)
			{
				statLegacy.AddModifier(modifiersLegacy[i]);
			}
			return statLegacy.Value;
		}

		private float AddLegacy1000()
		{
			for (int i = 0; i < 1000; i++)
			{
				statLegacy.AddModifier(modifiersLegacy[i]);
			}
			return statLegacy.Value;
		}

		private float RemoveLegacy10()
		{
			for (int i = 0; i < 10; i++)
			{
				statLegacy.RemoveModifier(modifiersLegacy[i]);
			}
			return statLegacy.Value;
		}

		private float RemoveLegacy100()
		{
			for (int i = 0; i < 100; i++)
			{
				statLegacy.RemoveModifier(modifiersLegacy[i]);
			}
			return statLegacy.Value;
		}

		private float RemoveLegacy1000()
		{
			for (int i = 0; i < 1000; i++)
			{
				statLegacy.RemoveModifier(modifiersLegacy[i]);
			}
			return statLegacy.Value;
		}

		private float RemoveFromSourceLegacy()
		{
			statLegacy.RemoveAllModifiersFromSource(source1);
			statLegacy.RemoveAllModifiersFromSource(source2);
			return statLegacy.Value;
		}

		private float Add10()
		{
			for (int i = 0; i < 10; i++)
			{
				stat.AddModifier(modifiers[i]);
			}
			return stat.FinalValue;
		}

		private float Add100()
		{
			for (int i = 0; i < 100; i++)
			{
				stat.AddModifier(modifiers[i]);
			}
			return stat.FinalValue;
		}

		private float Add1000()
		{
			for (int i = 0; i < 1000; i++)
			{
				stat.AddModifier(modifiers[i]);
			}
			return stat.FinalValue;
		}

		private float Remove10()
		{
			for (int i = 0; i < 10; i++)
			{
				stat.RemoveModifier(modifiers[i]);
			}
			return stat.FinalValue;
		}

		private float Remove100()
		{
			for (int i = 0; i < 100; i++)
			{
				stat.RemoveModifier(modifiers[i]);
			}
			return stat.FinalValue;
		}

		private float Remove1000()
		{
			for (int i = 0; i < 1000; i++)
			{
				stat.RemoveModifier(modifiers[i]);
			}
			return stat.FinalValue;
		}

		private float RemoveFromSource()
		{
			stat.RemoveModifiersFromSource(source1);
			stat.RemoveModifiersFromSource(source2);
			return stat.FinalValue;
		}
	}
}
