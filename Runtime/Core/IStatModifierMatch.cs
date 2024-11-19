namespace Kryz.RPG.Stats.Core
{
	public interface IStatModifierMatch<T> where T : struct, IStatModifierData<T>
	{
		bool IsMatch(StatModifier<T> modifier);
	}
}