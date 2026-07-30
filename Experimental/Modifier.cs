namespace Experimental
{
	public readonly struct Modifier
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
	}
}