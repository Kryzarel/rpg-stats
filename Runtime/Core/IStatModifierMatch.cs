namespace Kryz.RPG.Stats
{
	public interface IStatModifierMatch<T> where T : struct, IStatModifierData<T>
	{
		bool IsMatch(StatModifier<T> modifier);
	}
}