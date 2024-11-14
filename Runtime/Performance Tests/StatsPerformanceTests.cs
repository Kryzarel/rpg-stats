using UnityEngine;

using StatLegacy = Kryz.RPG.StatsLegacy.Stat;
using StatModifierLegacy = Kryz.RPG.StatsLegacy.StatModifier;
using StatModifierTypeLegacy = Kryz.RPG.StatsLegacy.StatModifierType;

using Stat = Kryz.RPG.Stats.Stat;
using StatModifier = Kryz.RPG.Stats.StatModifier<Kryz.RPG.Stats.StatModifierData>;
using StatModifierType = Kryz.RPG.Stats.StatModifierType;

using Stat2 = Kryz.RPG.Stats2.Default.Stat;
using StatModifier2 = Kryz.RPG.Stats2.Default.StatModifier;
using StatModifierType2 = Kryz.RPG.Stats2.Default.StatModifierType;

namespace Kryz.RPG.StatsPerfTests
{
	public class StatsPerformanceTests : MonoBehaviour
	{
		private const int Length = 1000;
		private readonly StatModifierLegacy[] modifiers_legacy = new StatModifierLegacy[Length];
		private readonly StatModifier[] modifiers = new StatModifier[Length];
		private readonly StatModifier2[] modifiers2 = new StatModifier2[Length];

		private readonly StatLegacy stat_legacy = new(10);
		private readonly Stat stat = new(10);
		private readonly Stat2 stat2 = new(10);

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
				StatModifier modifier = new(value, new((StatModifierType)listIndex, this));
				StatModifier2 modifier2 = new(value, (StatModifierType2)listIndex, this);

				modifiers_legacy[i] = modifier_legacy;
				modifiers[i] = modifier;
				modifiers2[i] = modifier2;

				stat_legacy.AddModifier(modifier_legacy);
				stat.AddModifier(modifier);
				stat2.AddModifier(modifier2);
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

			AddRemove1_2();
			AddRemove10_2();
			AddRemove100_2();
			AddRemoveAllFromSource_2();

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

		private void AddRemove1_2()
		{
			stat2.RemoveModifier(modifiers2[step1]);
			stat2.AddModifier(modifiers2[step1]);
			float value = stat2.FinalValue;
		}

		private void AddRemove10_2()
		{
			for (int i = step10, count = 0; i < Length && count < 10; i += step10, count++)
			{
				stat2.RemoveModifier(modifiers2[i]);
				stat2.AddModifier(modifiers2[i]);
			}
			float value = stat2.FinalValue;
		}

		private void AddRemove100_2()
		{
			for (int i = step100, count = 0; i < Length && count < 100; i += step100, count++)
			{
				stat2.RemoveModifier(modifiers2[i]);
				stat2.AddModifier(modifiers2[i]);
			}
			float value = stat2.FinalValue;
		}

		private void AddRemoveAllFromSource_2()
		{
			stat2.RemoveModifiersFromSource(this);
			for (int i = 0; i < Length; i++)
			{
				stat2.AddModifier(modifiers2[i]);
			}
			float value = stat2.FinalValue;
		}
	}
}