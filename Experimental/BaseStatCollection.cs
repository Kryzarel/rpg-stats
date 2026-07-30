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
		}

		public float GetValue(int stat) => finalStats[stat];
		public float GetBaseValue(int stat) => baseStats[stat];
		public bool IsDirty(int stat) => dirtyStats[stat];
		public void SetDirty(int stat) => dirtyStats[stat] = true;

		public abstract void Recalculate();
	}
}