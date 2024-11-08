namespace Kryz.RPG.Stats4
{
	public interface IStatModifierMatch<T> where T : struct, IStatModifierMetaData
	{
		bool IsMatch(float modifierValue, T metaData);
	}
}