using System.Collections.Generic;

namespace Kryz.RPG.Stats4
{
	public interface IReadOnlyStatModifierList
	{
		float Value { get; }
		IReadOnlyList<float> Modifiers { get; }
		float Calculate(float statCurrentValue);
	}

	public interface IStatModifierList : IReadOnlyStatModifierList
	{
		void Clear();
	}

	public interface IReadOnlyStatModifierList<T> : IReadOnlyStatModifierList where T : struct, IStatModifierMetaData
	{
		IReadOnlyList<T> ModifiersMetaData { get; }
	}

	public interface IStatModifierList<T> : IStatModifierList, IReadOnlyStatModifierList<T> where T : struct, IStatModifierMetaData
	{
		void Add(float modifierValue, T metaData);
		bool Remove(float modifierValue, T metaData);
		int RemoveWhere<TMatch>(TMatch match) where TMatch : IStatModifierMatch<T>;
	}
}