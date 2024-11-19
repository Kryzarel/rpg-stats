using Kryz.RPG.Stats.Core;

namespace Kryz.RPG.Stats.Default
{
	public readonly struct StatModifierMatch : IStatModifierMatch<StatModifierData>
	{
		public readonly ValueContainer<float> ModifierValue;
		public readonly ValueContainer<StatModifierType> Type;
		public readonly ValueContainer<object> Source;

		public StatModifierMatch(ValueContainer<float> modifierValue = default, ValueContainer<StatModifierType> type = default, ValueContainer<object> source = default)
		{
			ModifierValue = modifierValue;
			Type = type;
			Source = source;
		}

		public bool IsMatch(StatModifier<StatModifierData> modifier)
		{
			return (!ModifierValue.HasValue || ModifierValue.Value == modifier.Value) && (!Type.HasValue || Type.Value == modifier.Data.Type) && (!Source.HasValue || Source.Value == modifier.Data.Source);
		}
	}

	public readonly struct ValueContainer<T>
	{
		public readonly T Value;
		public readonly bool HasValue;

		public ValueContainer(T value)
		{
			Value = value;
			HasValue = true;
		}

		public static implicit operator ValueContainer<T>(T value) => new(value);
	}
}