namespace Kryz.RPG.Stats4
{
	public interface IStatModifierMatch<T> where T : struct, IStatModifierData
	{
		bool IsMatch(float modifierValue, T data);
	}
}