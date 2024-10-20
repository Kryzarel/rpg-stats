using System.Collections.Generic;

namespace Kryz.RPG.Stats
{
	public interface IStatModifierList<T> where T : struct, IStatModifier
	{
		IReadOnlyList<T> Modifiers { get; }

		void Add(T modifier);
		bool Remove(T modifier);
		int RemoveFromSource(object source);
		void Clear();

		float Calculate(float value);
	}
}