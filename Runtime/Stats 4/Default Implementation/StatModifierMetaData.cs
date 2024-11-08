using System;

namespace Kryz.RPG.Stats4
{
	public readonly struct StatModifierMetaData : IStatModifierMetaData, IComparable<StatModifierMetaData>, IEquatable<StatModifierMetaData>
	{
		public readonly StatModifierType Type;
		public readonly object? Source;

		public StatModifierMetaData(StatModifierType type, object? source = default)
		{
			Type = type;
			Source = source;
		}

		public int CompareTo(StatModifierMetaData other)
		{
			return Type.CompareTo(other.Type);
		}

		public bool Equals(StatModifierMetaData other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			return obj is StatModifierMetaData other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Type, Source);
		}

		public static bool operator ==(StatModifierMetaData a, StatModifierMetaData b)
		{
			return a.Type == b.Type && a.Source == b.Source;
		}

		public static bool operator !=(StatModifierMetaData a, StatModifierMetaData b)
		{
			return !(a == b);
		}
	}
}