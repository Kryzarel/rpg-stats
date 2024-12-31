using System;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats.Core
{
	public readonly struct StatModifier<T> : IComparable<StatModifier<T>>, IEquatable<StatModifier<T>> where T : struct, IStatModifierData<T>
	{
		public readonly float Value;
		public readonly T Data;

		public StatModifier(float value, T data)
		{
			Value = value;
			Data = data;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(StatModifier<T> other)
		{
			int result = Value.CompareTo(other.Value);
			if (result == 0)
			{
				result = Data.CompareTo(other.Data);
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(StatModifier<T> other)
		{
			return Value == other.Value && Data.Equals(other.Data);
		}

		public override bool Equals(object obj)
		{
			return obj is StatModifier<T> other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Value, Data);
		}

		public static bool operator ==(StatModifier<T> a, StatModifier<T> b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(StatModifier<T> a, StatModifier<T> b)
		{
			return !a.Equals(b);
		}
	}
}