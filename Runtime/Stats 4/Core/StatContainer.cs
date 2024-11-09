namespace Kryz.RPG.Stats4
{
	public readonly struct StatContainer<T, TStat> where T : struct, IStatModifierData<T> where TStat : IStat<T>
	{
		public readonly TStat Stat;
		public readonly IStatOperation Operation;

		public StatContainer(TStat stat, IStatOperation operation)
		{
			Stat = stat;
			Operation = operation;
		}
	}
}