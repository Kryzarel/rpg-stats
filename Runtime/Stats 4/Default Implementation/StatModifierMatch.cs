namespace Kryz.RPG.Stats4
{
	public readonly struct StatModifierMatch : IStatModifierMatch<StatModifierdata>
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

		public bool IsMatch(float modifierValue, StatModifierdata data)
		{
			return (!ModifierValue.HasValue || ModifierValue.Value == modifierValue) && (!Type.HasValue || Type.Value == data.Type) && (!Source.HasValue || Source.Value == data.Source);
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