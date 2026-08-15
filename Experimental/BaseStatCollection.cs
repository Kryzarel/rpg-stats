using System;

namespace Experimental
{
	public abstract class BaseStatCollection
	{
		public readonly int StatCount;
		public readonly int ModifierTypeCount;

		protected readonly float[] baseStats;
		protected readonly float[] finalStats;
		protected readonly float[] accumulators;
		protected readonly bool[] dirtyStats;

		public BaseStatCollection(int statCount, int modifierTypeCount)
		{
			StatCount = statCount;
			ModifierTypeCount = modifierTypeCount;

			baseStats = new float[statCount];
			finalStats = new float[statCount];
			accumulators = new float[statCount * modifierTypeCount];
			dirtyStats = new bool[statCount];

			Array.Fill(dirtyStats, true);
		}

		public float GetValue(int stat)
		{
			if (dirtyStats[stat])
			{
				Recalculate(stat);
			}
			return finalStats[stat];
		}

		public float GetBaseValue(int stat) => baseStats[stat];

		public void SetBaseValue(int stat, float value)
		{
			if (baseStats[stat] != value)
			{
				baseStats[stat] = value;
				dirtyStats[stat] = true;
			}
		}

		protected abstract void Recalculate(int stat);
	}
}