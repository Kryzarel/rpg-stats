using System;

namespace Kryz.RPG.Stats
{
	public class AddOperation<T> : IStatOperation<T> where T : struct, IStatModifierData<T>
	{
		public static readonly AddOperation<T> Default = new();
		public float Calculate(float outerStatValue, IStat<T> innerStat) => outerStatValue + innerStat.FinalValue;

		/// <summary>
		/// Default constructor is not allowed to avoid unnecessary allocations.
		/// Use '<see cref="Default"/>' instead.
		/// </summary>
		private AddOperation() { }
	}

	public class MultOperation<T> : IStatOperation<T> where T : struct, IStatModifierData<T>
	{
		public static readonly MultOperation<T> Default = new();
		public float Calculate(float outerStatValue, IStat<T> innerStat) => outerStatValue * innerStat.FinalValue;

		/// <summary>
		/// Default constructor is not allowed to avoid unnecessary allocations.
		/// Use '<see cref="Default"/>' instead.
		/// </summary>
		private MultOperation() { }
	}

	public class MaxOperation<T> : IStatOperation<T> where T : struct, IStatModifierData<T>
	{
		public static readonly MaxOperation<T> Default = new();
		public float Calculate(float outerStatValue, IStat<T> innerStat) => Math.Max(outerStatValue, innerStat.FinalValue);

		/// <summary>
		/// Default constructor is not allowed to avoid unnecessary allocations.
		/// Use '<see cref="Default"/>' instead.
		/// </summary>
		private MaxOperation() { }
	}

	public class MinOperation<T> : IStatOperation<T> where T : struct, IStatModifierData<T>
	{
		public static readonly MinOperation<T> Default = new();
		public float Calculate(float outerStatValue, IStat<T> innerStat) => Math.Min(outerStatValue, innerStat.FinalValue);

		/// <summary>
		/// Default constructor is not allowed to avoid unnecessary allocations.
		/// Use '<see cref="Default"/>' instead.
		/// </summary>
		private MinOperation() { }
	}
}