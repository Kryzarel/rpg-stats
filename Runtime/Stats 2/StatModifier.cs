using System;

namespace Kryz.RPG.Stats2
{
	public readonly struct StatModifier : IStatModifier, IEquatable<StatModifier>
	{
		public readonly float Value;
		public readonly object? Source;

		object? IStatModifier.Source => Source;

		public StatModifier(float value, object? source = null)
		{
			Value = value;
			Source = source;
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
			return a.Value == b.Value && a.Source == b.Source;
		}

		public static bool operator !=(StatModifier a, StatModifier b)
		{
			return !(a == b);
		}
	}
}