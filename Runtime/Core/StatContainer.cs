namespace Kryz.RPG.Stats.Core
{
	public abstract class StatContainer<T> where T : struct, IStatModifierData<T>
	{
		public readonly IStat<T> Stat;
		public abstract float Calculate(float outerStatValue);

		public StatContainer(IStat<T> stat) => Stat = stat;

		public static StatContainer<T> Create<TStat, TOperation>(TStat stat, TOperation operation) where TStat : IStat<T> where TOperation : IStatOperation<T>
		{
			return new StatContainer<T, TStat, TOperation>(stat, operation);
		}
	}

	public class StatContainer<T, TStat, TOperation> : StatContainer<T> where T : struct, IStatModifierData<T> where TStat : IStat<T> where TOperation : IStatOperation<T>
	{
		public new readonly TStat Stat;
		public readonly TOperation Operation;

		public StatContainer(TStat stat, TOperation operation) : base(stat)
		{
			Stat = stat;
			Operation = operation;
		}

		public override float Calculate(float outerStatValue)
		{
			return Operation.Calculate(outerStatValue, Stat);
		}
	}
}