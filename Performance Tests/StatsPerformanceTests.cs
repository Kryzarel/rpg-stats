using Kryz.CharacterStats;
using Kryz.RPG.Stats.Core;
using Kryz.RPG.Stats.Default;
using UnityEngine;

namespace Kryz.RPG.StatsPerfTests
{
	public class StatsPerformanceTests : MonoBehaviour
	{
		private readonly StatModifier[] modifiersLegacy = new StatModifier[100];
		private readonly StatModifier<StatModifierData>[] modifiers = new StatModifier<StatModifierData>[100];

		private readonly CharacterStat statLegacy = new(10);
		private readonly Stat stat = new(10);

		private readonly object source1 = new();
		private readonly object source2 = new();

		private static readonly int modTypeLegacyCount = System.Enum.GetValues(typeof(StatModType)).Length;
		private static readonly int modTypeCount = System.Enum.GetValues(typeof(StatModifierType)).Length;

		private void Awake()
		{
			for (int i = 0; i < 1000; i++)
			{
				statLegacy.AddModifier(new StatModifier(2, StatModType.Flat));
				stat.AddModifier(new StatModifier<StatModifierData>(2, new StatModifierData(StatModifierType.Add)));
			}
		}

		private void Update()
		{
			GenerateLegacy();
			GenerateNew();

			for (int i = 0; i < 10; i++)
			{
				TestLegacy();
				TestNew();
			}
		}

		private void GenerateLegacy()
		{
			for (int i = 0; i < modifiersLegacy.Length; i++)
			{
				float value = Random.Range(0.1f, 3f);
				object source = i % 2 == 0 ? source1 : source2;
				StatModType modTypeLegacy = (StatModType)(i % modTypeLegacyCount);
				modifiersLegacy[i] = new StatModifier(value, modTypeLegacy, source);
			}
		}

		private void GenerateNew()
		{
			for (int i = 0; i < modifiers.Length; i++)
			{
				float value = Random.Range(0.1f, 3f);
				object source = i % 2 == 0 ? source1 : source2;
				StatModifierType modType = (StatModifierType)(i % modTypeCount);
				modifiers[i] = new StatModifier<StatModifierData>(value, new(modType, source));
			}
		}

		private void TestLegacy()
		{
			AddLegacy10();
			RemoveLegacy10();

			AddLegacy100();
			RemoveLegacy100();

			AddLegacy100();
			RemoveFromSourceLegacy();
		}

		private void TestNew()
		{
			Add10();
			Remove10();

			Add100();
			Remove100();

			Add100();
			RemoveFromSource();
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
