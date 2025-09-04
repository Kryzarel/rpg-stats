using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kryz.RPG.Stats.Core
{
	public abstract class Stat<T> : IStat<T> where T : struct, IStatModifierData<T>
	{
		protected readonly IStat<T>[] stats;

		private bool isDirty;
		private float baseValue;
		private float finalValue;

		public event Action? OnValueChanged;

		public float BaseValue { get => baseValue; set => SetBaseValue(value); }
		public float FinalValue => GetFinalValue();

		public IReadOnlyList<IStat<T>> Stats => stats;

		IReadOnlyList<IStat> IStat.Stats => stats;
		IReadOnlyList<IReadOnlyStat<T>> IReadOnlyStat<T>.Stats => stats;
		IReadOnlyList<IReadOnlyStat> IReadOnlyStat.Stats => stats;

		IReadOnlyList<StatModifier<T>> IReadOnlyStat<T>.Modifiers => Array.Empty<StatModifier<T>>();

		protected Stat(float baseValue = 0, params IStat<T>[] stats)
		{
			isDirty = false;
			this.stats = stats;
			this.baseValue = baseValue;
			finalValue = CalculateFinalValue(baseValue);

			for (int i = 0; i < stats.Length; i++)
			{
				stats[i].OnValueChanged += OnChanged;
			}
		}

		public abstract void AddModifier(StatModifier<T> modifier);
		public abstract bool RemoveModifier(StatModifier<T> modifier);
		protected abstract float CalculateFinalValue(float baseValue);

		public int RemoveAll<TMatch>(TMatch match) where TMatch : IEquatable<StatModifier<T>>
		{
			int removedCount = 0;
			for (int i = 0; i < stats.Length; i++)
			{
				removedCount += stats[i].RemoveAll(match);
			}
			return removedCount;
		}

		public void Clear()
		{
			for (int i = 0; i < stats.Length; i++)
			{
				stats[i].Clear();
			}
		}

		private void OnChanged()
		{
			isDirty = true;
			OnValueChanged?.Invoke();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetBaseValue(float value)
		{
			if (baseValue != value)
			{
				isDirty = true;
				baseValue = value;
				OnValueChanged?.Invoke();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private float GetFinalValue()
		{
			if (isDirty)
			{
				isDirty = false;
				finalValue = CalculateFinalValue(baseValue);
			}
			return finalValue;
		}
	}
}