using System;
using Kryz.RPG.Stats.Core;

namespace Kryz.RPG.Stats.Default
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
			// Cast to the underlying type to avoid using CompareTo(object) and causing boxing
			return ((byte)Type).CompareTo((byte)other.Type);
		}

		public bool Equals(StatModifierData other)
		{
			return Type == other.Type && Source == other.Source;
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
			return a.Equals(b);
		}

		public static bool operator !=(StatModifierData a, StatModifierData b)
		{
			return !a.Equals(b);
		}
	}
}