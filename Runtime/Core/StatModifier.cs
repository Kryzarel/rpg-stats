using System;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats.Core
{
	public readonly struct StatModifier<T> : IComparable<StatModifier<T>>, IEquatable<StatModifier<T>> where T : struct, IStatModifierData<T>
	{
		public readonly float Value;
		public readonly T Data;

		private readonly int hashCode;

		public StatModifier(float value, T data)
		{
			Value = value;
			Data = data;
			hashCode = HashCode.Combine(Value, Data);
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			return obj is StatModifier<T> other && Equals(other);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return hashCode;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(StatModifier<T> a, StatModifier<T> b)
		{
			return a.Equals(b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(StatModifier<T> a, StatModifier<T> b)
		{
			return !a.Equals(b);
		}
	}
}