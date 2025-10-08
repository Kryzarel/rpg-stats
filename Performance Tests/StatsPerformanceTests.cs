using Kryz.CharacterStats;
using Kryz.RPG.Stats.Core;
using Kryz.RPG.Stats.Default;
using UnityEngine;

namespace Kryz.RPG.StatsPerfTests
{
	public class StatsPerformanceTests : MonoBehaviour
	{
		private const int Length = 100;
		private const int StartingStats = 0;

		private readonly StatModifier[] modifiersCharacter = new StatModifier[Length];
		private readonly StatModifier<StatModifierData>[] modifiersRPG = new StatModifier<StatModifierData>[Length];

		private readonly CharacterStat statCharacter = new(10);
		private readonly Stat statRPG = new(10);

		private readonly object source1 = new();
		private readonly object source2 = new();

		private static readonly int modTypeCountCharacter = System.Enum.GetValues(typeof(StatModType)).Length;
		private static readonly int modTypeCountRPG = System.Enum.GetValues(typeof(StatModifierType)).Length;

		private void Awake()
		{
			for (int i = 0; i < StartingStats; i++)
			{
				statCharacter.AddModifier(new StatModifier(2, StatModType.Flat));
				statRPG.AddModifier(new StatModifier<StatModifierData>(2, new StatModifierData(StatModifierType.Add)));
			}
		}

		private void Update()
		{
			GenerateModifiersCharacter();
			GenerateModifiersRPG();

			for (int i = 0; i < 10; i++)
			{
				TestStatsCharacter();
				TestStatsRPG();
			}
		}

		private void GenerateModifiersCharacter()
		{
			for (int i = 0; i < modifiersCharacter.Length; i++)
			{
				float value = Random.Range(0.1f, 3f);
				object source = i % 2 == 0 ? source1 : source2;
				StatModType modTypeCharacter = (StatModType)(i % modTypeCountCharacter);
				modifiersCharacter[i] = new StatModifier(value, modTypeCharacter, source);
			}
		}

		private void GenerateModifiersRPG()
		{
			for (int i = 0; i < modifiersRPG.Length; i++)
			{
				float value = Random.Range(0.1f, 3f);
				object source = i % 2 == 0 ? source1 : source2;
				StatModifierType modType = (StatModifierType)(i % modTypeCountRPG);
				modifiersRPG[i] = new StatModifier<StatModifierData>(value, new(modType, source));
			}
		}

		private void TestStatsCharacter()
		{
			Add10Character();
			Remove10Character();

			Add100Character();
			Remove100Character();

			Add100Character();
			RemoveFromSourceCharacter();

			AddGetValue10Character();
			RemoveGetValue10Character();
		}

		private void TestStatsRPG()
		{
			Add10RPG();
			Remove10RPG();

			Add100RPG();
			Remove100RPG();

			Add100RPG();
			RemoveFromSourceRPG();

			AddGetValue10RPG();
			RemoveGetValue10RPG();
		}

		private float AddCharacter(int count)
		{
			for (int i = 0; i < count; i++)
			{
				statCharacter.AddModifier(modifiersCharacter[i]);
			}
			return statCharacter.Value;
		}

		private float RemoveCharacter(int count)
		{
			for (int i = 0; i < count; i++)
			{
				statCharacter.RemoveModifier(modifiersCharacter[i]);
			}
			return statCharacter.Value;
		}

		private void AddGetValueCharacter(int count)
		{
			for (int i = 0; i < count; i++)
			{
				statCharacter.AddModifier(modifiersCharacter[i]);
				float value = statCharacter.Value;
			}
		}

		private void RemoveGetValueCharacter(int count)
		{
			for (int i = 0; i < count; i++)
			{
				statCharacter.RemoveModifier(modifiersCharacter[i]);
				float value = statCharacter.Value;
			}
		}

		private float Add10Character() => AddCharacter(10);
		private float Add100Character() => AddCharacter(100);

		private float Remove10Character() => RemoveCharacter(10);
		private float Remove100Character() => RemoveCharacter(100);

		private void AddGetValue10Character() => AddGetValueCharacter(10);
		private void RemoveGetValue10Character() => RemoveGetValueCharacter(10);

		private float RemoveFromSourceCharacter()
		{
			statCharacter.RemoveAllModifiersFromSource(source1);
			statCharacter.RemoveAllModifiersFromSource(source2);
			return statCharacter.Value;
		}

		private float AddRPG(int count)
		{
			for (int i = 0; i < count; i++)
			{
				statRPG.AddModifier(modifiersRPG[i]);
			}
			return statRPG.FinalValue;
		}

		private float RemoveRPG(int count)
		{
			for (int i = 0; i < count; i++)
			{
				statRPG.RemoveModifier(modifiersRPG[i]);
			}
			return statRPG.FinalValue;
		}

		private void AddGetValueRPG(int count)
		{
			for (int i = 0; i < count; i++)
			{
				statRPG.AddModifier(modifiersRPG[i]);
				float value = statRPG.FinalValue;
			}
		}

		private void RemoveGetValueRPG(int count)
		{
			for (int i = 0; i < count; i++)
			{
				statRPG.RemoveModifier(modifiersRPG[i]);
				float value = statRPG.FinalValue;
			}
		}

		private float Add10RPG() => AddRPG(10);
		private float Add100RPG() => AddRPG(100);

		private float Remove10RPG() => RemoveRPG(10);
		private float Remove100RPG() => RemoveRPG(100);

		private void AddGetValue10RPG() => AddGetValueRPG(10);
		private void RemoveGetValue10RPG() => RemoveGetValueRPG(10);

		private float RemoveFromSourceRPG()
		{
			statRPG.RemoveModifiersFromSource(source1);
			statRPG.RemoveModifiersFromSource(source2);
			return statRPG.FinalValue;
		}
	}
}
