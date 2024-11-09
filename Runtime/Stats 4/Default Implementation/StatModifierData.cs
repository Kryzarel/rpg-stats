using System;

namespace Kryz.RPG.Stats4
{
	public readonly struct StatModifierData : IStatModifierData<StatModifierData>
	{
		public readonly StatModifierType Type;
		public readonly object? Source;

		public StatModifierData(StatModifierType type, object? source = default)
		{
			Type = type;
			Source = source;
		}

		public int CompareTo(StatModifierData other)
		{
			return Type.CompareTo(other.Type);
		}

		public bool Equals(StatModifierData other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			return obj is StatModifierData other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Type, Source);
		}

		public static bool operator ==(StatModifierData a, StatModifierData b)
		{
			return a.Type == b.Type && a.Source == b.Source;
		}

		public static bool operator !=(StatModifierData a, StatModifierData b)
		{
			return !(a == b);
		}
	}
}