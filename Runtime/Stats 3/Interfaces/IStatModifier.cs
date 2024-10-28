namespace Kryz.RPG.Stats3
{
	public interface IStatModifier
	{
		float Value { get; }
		int Priority { get; }
		object? Source { get; }
		IStatModifierType Type { get; }
	}
}