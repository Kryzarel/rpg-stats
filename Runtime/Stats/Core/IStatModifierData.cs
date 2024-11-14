using System;

namespace Kryz.RPG.Stats
{
	public interface IStatModifierData
	{
	}

	public interface IStatModifierData<T> : IStatModifierData, IComparable<T>, IEquatable<T> where T : struct, IStatModifierData<T>
	{
	}
}