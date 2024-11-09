namespace Kryz.RPG.Stats4
{
	public readonly struct StatContainer<T> where T : struct, IStatModifierData<T>
	{
		public readonly IStat<T> Stat;
		public readonly IStatOperation<T> Operation;

		public StatContainer(IStat<T> stat, IStatOperation<T> operation)
		{
			Stat = stat;
			Operation = operation;
		}
	}
}