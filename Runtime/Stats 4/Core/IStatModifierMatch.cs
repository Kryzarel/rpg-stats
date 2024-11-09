namespace Kryz.RPG.Stats4
{
	public interface IStatModifierMatch<T> where T : struct, IStatModifierData<T>
	{
		bool IsMatch(StatModifier<T> modifier);
	}
}