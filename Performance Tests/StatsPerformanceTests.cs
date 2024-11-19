using Kryz.RPG.Stats.Core;
using Kryz.RPG.Stats.Default;
using UnityEngine;

using StatLegacy = Kryz.RPG.StatsLegacy.Stat;
using StatModifierLegacy = Kryz.RPG.StatsLegacy.StatModifier;
using StatModifierTypeLegacy = Kryz.RPG.StatsLegacy.StatModifierType;

namespace Kryz.RPG.StatsPerfTests
{
	public class StatsPerformanceTests : MonoBehaviour
	{
		private const int Length = 1000;
		private readonly StatModifierLegacy[] modifiers_legacy = new StatModifierLegacy[Length];
		private readonly StatModifier<StatModifierData>[] modifiers = new StatModifier<StatModifierData>[Length];

		private readonly StatLegacy stat_legacy = new(10);
		private readonly Stat stat = new(10);

		private int step1;
		private int step10;
		private int step100;

		private void Awake()
		{
			for (int i = 0; i < Length; i++)
			{
				float value = Random.Range(-100, 100 + 1);
				int listIndex = Random.Range(0, 4);

				StatModifierLegacy modifier_legacy = new(value, (StatModifierTypeLegacy)listIndex, this);
				StatModifier<StatModifierData> modifier = new(value, new((StatModifierType)listIndex, this));

				modifiers_legacy[i] = modifier_legacy;
				modifiers[i] = modifier;

				stat_legacy.AddModifier(modifier_legacy);
				stat.AddModifier(modifier);
			}
		}

		private void Update()
		{
			step1 = Random.Range(0, Length / 1);
			step10 = Random.Range(0, Length / 10);
			step100 = Random.Range(0, Length / 100);

			AddRemove1_Legacy();
			AddRemove10_Legacy();
			AddRemove100_Legacy();
			AddRemoveAllFromSource_Legacy();

			AddRemove1();
			AddRemove10();
			AddRemove100();
			AddRemoveAllFromSource();
		}

		private void AddRemove1_Legacy()
		{
			stat_legacy.RemoveModifier(modifiers_legacy[step1]);
			stat_legacy.AddModifier(modifiers_legacy[step1]);
			float value = stat_legacy.FinalValue;
		}

		private void AddRemove10_Legacy()
		{
			for (int i = step10, count = 0; i < Length && count < 10; i += step10, count++)
			{
				stat_legacy.RemoveModifier(modifiers_legacy[i]);
				stat_legacy.AddModifier(modifiers_legacy[i]);
			}
			float value = stat_legacy.FinalValue;
		}

		private void AddRemove100_Legacy()
		{
			for (int i = step100, count = 0; i < Length && count < 100; i += step100, count++)
			{
				stat_legacy.RemoveModifier(modifiers_legacy[i]);
				stat_legacy.AddModifier(modifiers_legacy[i]);
			}
			float value = stat_legacy.FinalValue;
		}

		private void AddRemoveAllFromSource_Legacy()
		{
			stat_legacy.RemoveModifiersFromSource(this);
			for (int i = 0; i < Length; i++)
			{
				stat_legacy.AddModifier(modifiers_legacy[i]);
			}
			float value = stat_legacy.FinalValue;
		}

		private void AddRemove1()
		{
			stat.RemoveModifier(modifiers[step1]);
			stat.AddModifier(modifiers[step1]);
			float value = stat.FinalValue;
		}

		private void AddRemove10()
		{
			for (int i = step10, count = 0; i < Length && count < 10; i += step10, count++)
			{
				stat.RemoveModifier(modifiers[i]);
				stat.AddModifier(modifiers[i]);
			}
			float value = stat.FinalValue;
		}

		private void AddRemove100()
		{
			for (int i = step100, count = 0; i < Length && count < 100; i += step100, count++)
			{
				stat.RemoveModifier(modifiers[i]);
				stat.AddModifier(modifiers[i]);
			}
			float value = stat.FinalValue;
		}

		private void AddRemoveAllFromSource()
		{
			stat.RemoveModifiersFromSource(this);
			for (int i = 0; i < Length; i++)
			{
				stat.AddModifier(modifiers[i]);
			}
			float value = stat.FinalValue;
		}
	}
}