using System;

namespace Experimental
{
	public readonly struct Modifier : IEquatable<Modifier>, IComparable<Modifier>
	{
		public readonly int Stat;
		public readonly float Value;
		public readonly ModifierType Type;

		public Modifier(int stat, float value, ModifierType type)
		{
			Stat = stat;
			Value = value;
			Type = type;
		}

		public int CompareTo(Modifier other)
		{
			int result = Stat.CompareTo(other.Stat);
			if (result == 0) result = ((int)Type).CompareTo((int)other.Type);
			if (result == 0) result = Value.CompareTo(other.Value);
			return result;
		}

		public bool Equals(Modifier other)
		{
			return Stat == other.Stat && Value == other.Value && Type == other.Type;
		}

		public override bool Equals(object obj)
		{
			return obj is Modifier other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Stat, Value, Type);
		}

		public static bool operator ==(Modifier a, Modifier b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Modifier a, Modifier b)
		{
			return !a.Equals(b);
		}
	}
}