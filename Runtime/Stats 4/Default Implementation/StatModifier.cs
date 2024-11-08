using System;

namespace Kryz.RPG.Stats4
{
	public readonly struct StatModifier : IComparable<StatModifier>, IEquatable<StatModifier>
	{
		public readonly float Value;
		public readonly StatModifierdata Data;

		public StatModifier(float value, StatModifierType type, object? source = default)
		{
			Value = value;
			Data = new StatModifierdata(type, source);
		}

		public StatModifier(float value, StatModifierdata data)
		{
			Value = value;
			Data = data;
		}

		public int CompareTo(StatModifier other)
		{
			int result = Data.CompareTo(other.Data);
			if (result == 0)
			{
				result = Value.CompareTo(other.Value);
			}
			return result;
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
			return HashCode.Combine(Value, Data);
		}

		public static bool operator ==(StatModifier a, StatModifier b)
		{
			return a.Value == b.Value && a.Data == b.Data;
		}

		public static bool operator !=(StatModifier a, StatModifier b)
		{
			return !(a == b);
		}
	}
}