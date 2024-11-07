namespace Kryz.RPG.Stats2
{
	public interface IStat<T> : Core.IStat<T> where T : struct, IStatModifier
	{
		int RemoveModifiersFromSource(object source);
	}
}