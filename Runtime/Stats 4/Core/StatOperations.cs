namespace Kryz.RPG.Stats4
{
	public class AddOperation<T> : IStatOperation<T> where T : struct, IStatModifierData<T>
	{
		public static readonly AddOperation<T> Instance = new();
		public float Calculate(float outerStatValue, IStat<T> innerStat) => outerStatValue + innerStat.FinalValue;

		/// <summary>
		/// Default constructor is not allowed to avoid unnecessary allocations.
		/// Use '<see cref="Instance"/>' instead.
		/// </summary>
		private AddOperation() { }
	}

	public class MultiplyOperation<T> : IStatOperation<T> where T : struct, IStatModifierData<T>
	{
		public static readonly MultiplyOperation<T> Instance = new();
		public float Calculate(float outerStatValue, IStat<T> innerStat) => outerStatValue * innerStat.FinalValue;

		/// <summary>
		/// Default constructor is not allowed to avoid unnecessary allocations.
		/// Use '<see cref="Instance"/>' instead.
		/// </summary>
		private MultiplyOperation() { }
	}

	public class OverrideOperation<T> : IStatOperation<T> where T : struct, IStatModifierData<T>
	{
		public static readonly OverrideOperation<T> Instance = new();
		public float Calculate(float outerStatValue, IStat<T> innerStat) => innerStat.ModifiersCount > 0 ? innerStat.FinalValue : outerStatValue;

		/// <summary>
		/// Default constructor is not allowed to avoid unnecessary allocations.
		/// Use '<see cref="Instance"/>' instead.
		/// </summary>
		private OverrideOperation() { }
	}
}