using System;

namespace Kryz.RPG.Stats4
{
	public readonly struct StatModifier : IComparable<StatModifier>, IEquatable<StatModifier>
	{
		public readonly float Value;
		public readonly StatModifierMetaData MetaData;

		public StatModifier(float value, StatModifierType type, object? source = default)
		{
			Value = value;
			MetaData = new StatModifierMetaData(type, source);
		}

		public StatModifier(float value, StatModifierMetaData metaData)
		{
			Value = value;
			MetaData = metaData;
		}

		public int CompareTo(StatModifier other)
		{
			int result = MetaData.CompareTo(other.MetaData);
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
			return HashCode.Combine(Value, MetaData);
		}

		public static bool operator ==(StatModifier a, StatModifier b)
		{
			return a.Value == b.Value && a.MetaData == b.MetaData;
		}

		public static bool operator !=(StatModifier a, StatModifier b)
		{
			return !(a == b);
		}
	}
}