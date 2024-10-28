using System;

namespace Kryz.RPG.Stats3
{
	public readonly struct StatModifier : IStatModifier, IComparable<StatModifier>, IEquatable<StatModifier>
	{
		public readonly float Value;
		public readonly int Priority;
		public readonly object? Source;
		public readonly IStatModifierType<StatModifier> Type;

		float IStatModifier.Value => Value;
		int IStatModifier.Priority => Priority;
		object? IStatModifier.Source => Source;
		IStatModifierType IStatModifier.Type => Type;

		public StatModifier(float value, IStatModifierType<StatModifier> type, int priority = 0, object? source = null)
		{
			Value = value;
			Priority = priority;
			Source = source;
			Type = type;
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
			return HashCode.Combine(Value, Source);
		}

		public static bool operator ==(StatModifier a, StatModifier b)
		{
			return a.Value == b.Value && a.Source == b.Source && a.Priority == b.Priority && a.Type == b.Type;
		}

		public static bool operator !=(StatModifier a, StatModifier b)
		{
			return !(a == b);
		}
	}
}