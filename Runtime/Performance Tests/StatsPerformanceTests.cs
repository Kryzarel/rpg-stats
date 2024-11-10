using UnityEngine;
using Stat2 = Kryz.RPG.Stats2.Default.Stat;
using StatModifier2 = Kryz.RPG.Stats2.Default.StatModifier;
using StatModifierType2 = Kryz.RPG.Stats2.Default.StatModifierType;

using Kryz.RPG.Stats3;
using Stat3 = Kryz.RPG.Stats3.Stat;
using StatModifier3 = Kryz.RPG.Stats3.StatModifier;

using Stat4 = Kryz.RPG.Stats4.Stat;
using StatModifier4 = Kryz.RPG.Stats4.StatModifier<Kryz.RPG.Stats4.StatModifierData>;
using StatModifierType4 = Kryz.RPG.Stats4.StatModifierType;

namespace Kryz.RPG.Stats.PerfTests
{
	public class StatsPerformanceTests : MonoBehaviour
	{
		private const int Length = 1000;
		private readonly StatModifier[] modifiers = new StatModifier[Length];
		private readonly StatModifier2[] modifiers2 = new StatModifier2[Length];
		private readonly StatModifier3[] modifiers3 = new StatModifier3[Length];
		private readonly StatModifier4[] modifiers4 = new StatModifier4[Length];

		private readonly Stat stat = new(10);
		private readonly Stat2 stat2 = new(10);
		private readonly Stat3 stat3 = new(10);
		private readonly Stat4 stat4 = new(10);

		private int step1;
		private int step10;
		private int step100;

		private void Awake()
		{
			for (int i = 0; i < Length; i++)
			{
				float value = Random.Range(-100, 100 + 1);
				int listIndex = Random.Range(0, 4);

				StatModifier modifier = new(value, (StatModifierType)listIndex, this);
				StatModifier2 modifier2 = new(value, (StatModifierType2)listIndex, this);
				StatModifier4 modifier4 = new(value, new((StatModifierType4)listIndex, this));

				IStatModifierType<StatModifier3> type = listIndex switch
				{
					0 => StatModifierListAdd<StatModifier3>.Type,
					1 => StatModifierListMultiplyBase<StatModifier3>.Type,
					2 => StatModifierListMultiplyTotal<StatModifier3>.Type,
					3 => StatModifierListMultiplyTotal<StatModifier3>.Type,
					_ => throw new System.NotImplementedException(),
				};
				StatModifier3 modifier3 = new(value, type, priority: 0, this);

				modifiers[i] = modifier;
				modifiers2[i] = modifier2;
				modifiers3[i] = modifier3;
				modifiers4[i] = modifier4;

				stat.AddModifier(modifier);
				stat2.AddModifier(modifier2);
				stat3.AddModifier(modifier3);
				stat4.AddModifier(modifier4);
			}
		}

		private void Update()
		{
			step1 = Random.Range(0, Length / 1);
			step10 = Random.Range(0, Length / 10);
			step100 = Random.Range(0, Length / 100);

			AddRemove1();
			AddRemove10();
			AddRemove100();
			AddRemoveAllFromSource();

			AddRemove1_2();
			AddRemove10_2();
			AddRemove100_2();
			AddRemoveAllFromSource_2();

			AddRemove1_3();
			AddRemove10_3();
			AddRemove100_3();
			AddRemoveAllFromSource_3();

			AddRemove1_4();
			AddRemove10_4();
			AddRemove100_4();
			AddRemoveAllFromSource_4();
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

		private void AddRemove1_3()
		{
			stat3.RemoveModifier(modifiers3[step1]);
			stat3.AddModifier(modifiers3[step1]);
			float value = stat3.FinalValue;
		}

		private void AddRemove10_3()
		{
			for (int i = step10, count = 0; i < Length && count < 10; i += step10, count++)
			{
				stat3.RemoveModifier(modifiers3[i]);
				stat3.AddModifier(modifiers3[i]);
			}
			float value = stat3.FinalValue;
		}

		private void AddRemove100_3()
		{
			for (int i = step100, count = 0; i < Length && count < 100; i += step100, count++)
			{
				stat3.RemoveModifier(modifiers3[i]);
				stat3.AddModifier(modifiers3[i]);
			}
			float value = stat3.FinalValue;
		}

		private void AddRemoveAllFromSource_3()
		{
			stat3.RemoveModifiersFromSource(this);
			for (int i = 0; i < Length; i++)
			{
				stat3.AddModifier(modifiers3[i]);
			}
			float value = stat3.FinalValue;
		}

		private void AddRemove1_4()
		{
			stat4.RemoveModifier(modifiers4[step1]);
			stat4.AddModifier(modifiers4[step1]);
			float value = stat4.FinalValue;
		}

		private void AddRemove10_4()
		{
			for (int i = step10, count = 0; i < Length && count < 10; i += step10, count++)
			{
				stat4.RemoveModifier(modifiers4[i]);
				stat4.AddModifier(modifiers4[i]);
			}
			float value = stat4.FinalValue;
		}

		private void AddRemove100_4()
		{
			for (int i = step100, count = 0; i < Length && count < 100; i += step100, count++)
			{
				stat4.RemoveModifier(modifiers4[i]);
				stat4.AddModifier(modifiers4[i]);
			}
			float value = stat4.FinalValue;
		}

		private void AddRemoveAllFromSource_4()
		{
			stat4.RemoveModifiersFromSource(this);
			for (int i = 0; i < Length; i++)
			{
				stat4.AddModifier(modifiers4[i]);
			}
			float value = stat4.FinalValue;
		}
	}
}