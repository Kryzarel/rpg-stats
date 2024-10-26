using System;

namespace Kryz.RPG.Stats
{
	public readonly struct StatModifier : IStatModifier, IComparable<StatModifier>, IEquatable<StatModifier>
	{
		public readonly float Value;
		public readonly StatModifierType Type;
		public readonly int Priority;
		public readonly object? Source;

		object? IStatModifier.Source => Source;

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
			StatModifierType.Override => 900,
			_ => 0,
		};

		public int CompareTo(StatModifier other)
		{
			int result = Priority.CompareTo(other.Priority);
			if (result == 0)
			{
				// Cast to int to avoid boxing
				result = ((int)Type).CompareTo((int)other.Type);
			}
			if (result == 0)
			{
				result = Value.CompareTo(other.Value);
			}
			return result;
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