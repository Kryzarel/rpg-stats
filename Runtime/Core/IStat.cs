namespace Kryz.RPG.Stats.Core
{
	public interface IStat : IReadOnlyStat
	{
		new float BaseValue { get; set; }

		void Clear();
		void ClearModifiers();
	}

	public interface IStat<T> : IStat, IReadOnlyStat<T> where T : struct, IStatModifierData<T>
	{
		void AddModifier(StatModifier<T> modifier);
		bool RemoveModifier(StatModifier<T> modifier);
		int RemoveWhere<TMatch>(TMatch match) where TMatch : IStatModifierMatch<T>;
	}
}