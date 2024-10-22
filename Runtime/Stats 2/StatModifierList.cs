using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats2
{
	public abstract class StatModifierList<T> : IStatModifierList<T> where T : struct, IStatModifier
	{
		public IReadOnlyList<T> Modifiers => modifiers;
		public float CurrentValue => currentValue;

		private readonly List<T> modifiers = new();
		private readonly float defaultValue;
		private float currentValue;

		protected StatModifierList(float defaultValue)
		{
			this.defaultValue = defaultValue;
			currentValue = defaultValue;
		}

		public void Add(T modifier)
		{
			modifiers.Add(modifier);
			currentValue = AddOperation(currentValue, modifier);
		}

		public bool Remove(T modifier)
		{
			if (modifiers.Remove(modifier))
			{
				currentValue = RemoveOperation(currentValue, modifier);
				return true;
			}
			return false;
		}

		public int RemoveFromSource(object source)
		{
			int numRemoved = 0;
			for (int i = modifiers.Count - 1; i >= 0; i--)
			{
				T modifier = modifiers[i];
				if (modifier.Source == source)
				{
					modifiers.RemoveAt(i);
					currentValue = RemoveOperation(currentValue, modifier);
					numRemoved++;
				}
			}
			return numRemoved;
		}

		public void Clear()
		{
			modifiers.Clear();
			currentValue = defaultValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public abstract float Calculate(float value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract float AddOperation(float currentValue, T modifier);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract float RemoveOperation(float currentValue, T modifier);
	}
}