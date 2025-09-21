using System;
using System.Runtime.CompilerServices;
using Kryz.RPG.Stats.Core;

namespace Kryz.RPG.Stats.Default
{
	public readonly struct StatModifierData : IStatModifierData<StatModifierData>
	{
		public readonly StatModifierType Type;
		public readonly object? Source;

		private readonly int hashCode;

		public StatModifierData(StatModifierType type, object? source = default)
		{
			Type = type;
			Source = source;
			hashCode = HashCode.Combine(Type, Source);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(StatModifierData other)
		{
			// Cast to int to avoid using CompareTo(object) and causing boxing
			return ((int)Type).CompareTo((int)other.Type);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(StatModifierData other)
		{
			return Type == other.Type && Source == other.Source;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			return obj is StatModifierData other && Equals(other);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return hashCode;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(StatModifierData a, StatModifierData b)
		{
			return a.Equals(b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(StatModifierData a, StatModifierData b)
		{
			return !a.Equals(b);
		}
	}
}