using System;

namespace Kryz.RPG.Stats
{
	public readonly struct SimpleStatModifier : IStatModifier, IEquatable<SimpleStatModifier>
	{
		public readonly float Value;
		public readonly object? Source;

		object? IStatModifier.Source => Source;

		public SimpleStatModifier(float value, object? source = null)
		{
			Value = value;
			Source = source;
		}

		public bool Equals(SimpleStatModifier other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			return obj is SimpleStatModifier other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Value, Source);
		}

		public static bool operator ==(SimpleStatModifier a, SimpleStatModifier b)
		{
			return a.Value == b.Value && a.Source == b.Source;
		}

		public static bool operator !=(SimpleStatModifier a, SimpleStatModifier b)
		{
			return !(a == b);
		}
	}
}