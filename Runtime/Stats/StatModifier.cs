using System;

namespace Kryz.RPG.Stats
{
	public readonly struct StatModifier : IComparable<StatModifier>, IEquatable<StatModifier>
	{
		public readonly float Value;
		public readonly StatModifierType Type;
		public readonly int Priority;
		public readonly object? Source;

		public StatModifier(float value, StatModifierType type, object? source = null) : this(value, type, GetDefaultPriority(type), source) { }

		public StatModifier(float value, StatModifierType type, int priority, object? source = null)
		{
			Value = value;
			Type = type;
			Priority = priority;
			Source = source;
		}

		private static int GetDefaultPriority(StatModifierType type) => type switch
		{
			StatModifierType.Add => 100,
			StatModifierType.MultiplyBase => 200,
			StatModifierType.MultiplyTotal => 300,
			StatModifierType.Override => 1000,
			_ => 0,
		};

		public int CompareTo(StatModifier other)
		{
			// Compare in reverse to get higher priority first
			return other.Priority.CompareTo(Priority);
		}

		public bool Equals(StatModifier other)
		{
			return other == this;
		}

		public override bool Equals(object obj)
		{
			return obj is StatModifier modifier && modifier == this;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Value, Type, Priority, Source);
		}

		public static bool operator ==(StatModifier a, StatModifier b)
		{
			return a.Value == b.Value && a.Type == b.Type && a.Priority == b.Priority && a.Source == b.Source;
		}

		public static bool operator !=(StatModifier a, StatModifier b)
		{
			return !(a == b);
		}
	}
}