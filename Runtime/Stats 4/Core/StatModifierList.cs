using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats4
{
	public abstract class StatModifierList<T> : IStatModifierList<T> where T : struct, IStatModifierMetaData
	{
		private static readonly EqualityComparer<T> metaDataComparer = EqualityComparer<T>.Default;

		protected readonly List<float> modifiers = new();
		protected readonly List<T> metaDatas = new();
		private readonly float defaultValue;
		private float currentValue;

		public float Value => currentValue;
		public IReadOnlyList<float> Modifiers => modifiers;
		public IReadOnlyList<T> ModifiersMetaData => metaDatas;

		public StatModifierList(float defaultValue)
		{
			this.defaultValue = defaultValue;
			currentValue = defaultValue;
		}

		public void Add(float modifierValue, T metaData)
		{
			modifiers.Add(modifierValue);
			metaDatas.Add(metaData);
			currentValue = AddOperation(currentValue, modifierValue, metaData);
		}

		public bool Remove(float modifierValue, T metaData)
		{
			for (int i = modifiers.Count - 1; i >= 0; i--)
			{
				if (modifiers[i] == modifierValue && metaDataComparer.Equals(metaDatas[i], metaData))
				{
					modifiers.RemoveAt(i);
					metaDatas.RemoveAt(i);
					currentValue = RemoveOperation(currentValue, modifierValue, metaData);
					return true;
				}
			}
			return false;
		}

		public int RemoveWhere(Func<float, T, bool> predicate)
		{
			int removedCount = 0;
			for (int i = modifiers.Count - 1; i >= 0; i--)
			{
				if (predicate(modifiers[i], metaDatas[i]))
				{
					modifiers.RemoveAt(i);
					metaDatas.RemoveAt(i);
					removedCount++;
				}
			}
			return removedCount;
		}

		public void Clear()
		{
			modifiers.Clear();
			currentValue = defaultValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float Calculate(float statCurrentValue) => Calculate(statCurrentValue, currentValue);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract float Calculate(float statCurrentValue, float modifierValue);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract float AddOperation(float currentValue, float modifierValue, T metaData);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract float RemoveOperation(float currentValue, float modifierValue, T metaData);
	}
}