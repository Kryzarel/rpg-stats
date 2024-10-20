namespace Kryz.RPG.Stats
{
	public readonly struct StatModifier
	{
		public readonly float Value;
		public readonly StatModifierType Type;
		public readonly object? Source;

		public StatModifier(float value, StatModifierType type, object? source = null)
		{
			Value = value;
			Type = type;
			Source = source;
		}
	}
}