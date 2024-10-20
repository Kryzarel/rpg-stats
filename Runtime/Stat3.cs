using System.Collections.Generic;

namespace Kryz.RPG.Stats
{
	public class Stat3
	{
		private readonly List<SimpleStatModifier> modifiersAdd = new();
		private readonly List<SimpleStatModifier> modifiersMultiplyBase = new();
		private readonly List<SimpleStatModifier> modifiersMultiplyTotal = new();

		private float addValue;
		private float multiplyBaseValue;
		private float multiplyTotalValue = 1;

		private float baseValue;
		private float finalValue;

		public float BaseValue { get => baseValue; set => baseValue = value; }
		public float FinalValue => finalValue;

		public Stat3(float baseValue = 0)
		{
			this.baseValue = baseValue;
			finalValue = baseValue;
		}

		public void AddModifierAdd(SimpleStatModifier modifier)
		{
			modifiersAdd.Add(modifier);
			addValue += modifier.Value;
			CalculateFinalValue();
		}

		public void AddModifierMultiplyBase(SimpleStatModifier modifier)
		{
			modifiersMultiplyBase.Add(modifier);
			multiplyBaseValue += modifier.Value;
			CalculateFinalValue();
		}

		public void AddModifierMultiplyTotal(SimpleStatModifier modifier)
		{
			modifiersMultiplyTotal.Add(modifier);
			multiplyTotalValue *= 1 + modifier.Value;
			CalculateFinalValue();
		}

		private void CalculateFinalValue()
		{
			finalValue = (baseValue + addValue + (baseValue * multiplyBaseValue)) * multiplyTotalValue;
		}
	}
}