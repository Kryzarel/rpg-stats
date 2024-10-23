using UnityEngine;
using Stat2 = Kryz.RPG.Stats2.Stat;
using StatModifier2 = Kryz.RPG.Stats2.StatModifier;

namespace Kryz.RPG.Stats.PerfTests
{
	public class StatsPerformanceTests : MonoBehaviour
	{
		private const int Length = 1000;
		private readonly StatModifier[] modifiers = new StatModifier[Length];
		private readonly StatModifier2[] modifiers2 = new StatModifier2[Length];
		private readonly int[] listIndexes = new int[Length];

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
				int listIndex = Random.Range(0, 3);

				StatModifier modifider = new(value, (StatModifierType)listIndex, this);
				StatModifier2 modifier2 = new(value, this);

				modifiers[i] = modifider;
				modifiers2[i] = modifier2;
				listIndexes[i] = listIndex;

				stat.Add(modifider);
				stat2.TryAddModifier(listIndex, modifier2);
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
		}

		private void AddRemove1()
		{
			stat.Remove(modifiers[step1]);
			stat.Add(modifiers[step1]);
			float value = stat.FinalValue;
		}

		private void AddRemove10()
		{
			for (int i = step10, count = 0; i < Length && count < 10; i += step10, count++)
			{
				stat.Remove(modifiers[i]);
				stat.Add(modifiers[i]);
			}
			float value = stat.FinalValue;
		}

		private void AddRemove100()
		{
			for (int i = step100, count = 0; i < Length && count < 100; i += step100, count++)
			{
				stat.Remove(modifiers[i]);
				stat.Add(modifiers[i]);
			}
			float value = stat.FinalValue;
		}

		private void AddRemoveAllFromSource()
		{
			stat.RemoveAllFromSource(this);
			for (int i = 0; i < Length; i++)
			{
				stat.Add(modifiers[i]);
			}
			float value = stat.FinalValue;
		}

		private void AddRemove1_2()
		{
			stat2.TryRemoveModifier(listIndexes[step1], modifiers2[step1]);
			stat2.TryAddModifier(listIndexes[step1], modifiers2[step1]);
			float value = stat2.FinalValue;
		}

		private void AddRemove10_2()
		{
			for (int i = step10, count = 0; i < Length && count < 10; i += step10, count++)
			{
				stat2.TryRemoveModifier(listIndexes[i], modifiers2[i]);
				stat2.TryAddModifier(listIndexes[i], modifiers2[i]);
			}
			float value = stat2.FinalValue;
		}

		private void AddRemove100_2()
		{
			for (int i = step100, count = 0; i < Length && count < 100; i += step100, count++)
			{
				stat2.TryRemoveModifier(listIndexes[i], modifiers2[i]);
				stat2.TryAddModifier(listIndexes[i], modifiers2[i]);
			}
			float value = stat2.FinalValue;
		}

		private void AddRemoveAllFromSource_2()
		{
			stat2.RemoveModifiersFromSource(this);
			for (int i = 0; i < Length; i++)
			{
				stat2.TryAddModifier(listIndexes[i], modifiers2[i]);
			}
			float value = stat2.FinalValue;
		}
	}
}