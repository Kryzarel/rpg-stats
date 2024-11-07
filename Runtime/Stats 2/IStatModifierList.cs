namespace Kryz.RPG.Stats2
{
	public interface IStatModifierList<T> : Core.IStatModifierList<T> where T : struct, IStatModifier
	{
		int RemoveModifiersFromSource(object source);
	}
}