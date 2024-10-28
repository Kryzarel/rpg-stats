namespace Kryz.RPG.Stats3
{
	public interface IStatModifierList
	{
		int Priority { get; }

		float ModifierValue { get; }
		float Calculate(float statBaseValue, float statCurrentValue);

		int RemoveFromSource(object source);
		void Clear();
	}

	public interface IStatModifierList<T> : IStatModifierList where T : struct, IStatModifier
	{
		void Add(T modifier);
		bool Remove(T modifier);
	}
}