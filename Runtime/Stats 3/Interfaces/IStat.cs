namespace Kryz.RPG.Stats3
{
	public interface IStat
	{
		float BaseValue { get; set; }
		float FinalValue { get; }

		void Clear();
		void ClearModifiers();

		void AddModifier<T>(T modifier) where T : struct, IStatModifier;
		bool RemoveModifier<T>(T modifier) where T : struct, IStatModifier;
		int RemoveModifiersFromSource(object source);
	}
}