namespace Kryz.RPG.Stats2
{
	public interface IStatModifier : Core.IStatModifier
	{
		float Value { get; }
		object? Source { get; }
	}
}