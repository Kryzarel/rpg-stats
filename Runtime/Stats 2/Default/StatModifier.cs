using System;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats2.Default
{
	public readonly struct StatModifier : IStatModifier, IComparable<StatModifier>, IEquatable<StatModifier>
	{
		public readonly float Value;
		public readonly StatModifierType Type;
		public readonly object? Source;

		float IStatModifier.Value
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Value;
		}

		object? IStatModifier.Source
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Source;
		}

		public StatModifier(float value, StatModifierType type, object? source = default)
		{
			Value = value;
			Type = type;
			Source = source;
		}

		public int CompareTo(StatModifier other)
		{
			return Value.CompareTo(other.Value);
		}

		public bool Equals(StatModifier other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			return obj is StatModifier other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Value, Type, Source);
		}

		public static bool operator ==(StatModifier a, StatModifier b)
		{
			return a.Value == b.Value && a.Type == b.Type && a.Source == b.Source;
		}

		public static bool operator !=(StatModifier a, StatModifier b)
		{
			return !(a == b);
		}
	}
}