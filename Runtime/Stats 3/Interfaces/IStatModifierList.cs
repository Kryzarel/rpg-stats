using System.Collections.Generic;

namespace Kryz.RPG.Stats3
{
	public interface IReadOnlyStatModifierList<out T> : IReadOnlyList<T> where T : struct, IStatModifier
	{
		float ModifierValue { get; }
		float Calculate(float statBaseValue, float statCurrentValue);
	}

	public interface IStatModifierList<T> : IReadOnlyStatModifierList<T> where T : struct, IStatModifier
	{
		void Add(T modifier);
		bool Remove(T modifier);
		int RemoveFromSource(object source);
		void Clear();
	}
}