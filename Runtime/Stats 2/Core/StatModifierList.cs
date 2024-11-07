using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats2
{
	public abstract class StatModifierList<T> : IStatModifierList<T> where T : struct, IStatModifier
	{
		protected readonly List<T> modifiers = new();

		private readonly float defaultValue;
		private float modifierValue;

		public float ModifierValue => modifierValue;
		public int Count => modifiers.Count;
		public T this[int index] => modifiers[index];

		protected StatModifierList(float defaultValue)
		{
			this.defaultValue = defaultValue;
			modifierValue = defaultValue;
		}

		public void Add(T modifier)
		{
			modifiers.Add(modifier);
			modifierValue = AddOperation(modifierValue, modifier);
		}

		public bool Remove(T modifier)
		{
			if (modifiers.Remove(modifier))
			{
				modifierValue = RemoveOperation(modifierValue, modifier);
				return true;
			}
			return false;
		}

		public void Clear()
		{
			modifiers.Clear();
			modifierValue = defaultValue;
		}

		public IEnumerator<T> GetEnumerator() => modifiers.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => modifiers.GetEnumerator();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float Calculate(float statBaseValue, float statCurrentValue) => Calculate(statBaseValue, statCurrentValue, modifierValue);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract float Calculate(float statBaseValue, float statCurrentValue, float modifierValue);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract float AddOperation(float modifierValue, T modifier);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract float RemoveOperation(float modifierValue, T modifier);
	}
}