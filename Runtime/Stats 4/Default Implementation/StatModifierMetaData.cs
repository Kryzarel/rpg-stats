using System;

namespace Kryz.RPG.Stats4
{
	public readonly struct StatModifierdata : IStatModifierData, IComparable<StatModifierdata>, IEquatable<StatModifierdata>
	{
		public readonly StatModifierType Type;
		public readonly object? Source;

		public StatModifierdata(StatModifierType type, object? source = default)
		{
			Type = type;
			Source = source;
		}

		public int CompareTo(StatModifierdata other)
		{
			return Type.CompareTo(other.Type);
		}

		public bool Equals(StatModifierdata other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			return obj is StatModifierdata other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Type, Source);
		}

		public static bool operator ==(StatModifierdata a, StatModifierdata b)
		{
			return a.Type == b.Type && a.Source == b.Source;
		}

		public static bool operator !=(StatModifierdata a, StatModifierdata b)
		{
			return !(a == b);
		}
	}
}