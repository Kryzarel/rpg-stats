using System.Collections.Generic;

namespace Kryz.RPG.Stats2
{
	public interface IReadOnlyStatModifierList<T> where T : struct, IStatModifier
	{
		IReadOnlyList<T> Modifiers { get; }
		float Calculate(float value);
	}

	public interface IStatModifierList<T> : IReadOnlyStatModifierList<T> where T : struct, IStatModifier
	{
		void Add(T modifier);
		bool Remove(T modifier);
		int RemoveFromSource(object source);
		void Clear();
	}
}