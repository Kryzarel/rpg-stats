namespace Kryz.RPG.Stats
{
	public class Stat2
	{
		private readonly StatModifierListAdd modifiersAdd = new();
		private readonly StatModifierListMultiplyBase modifiersMultiplyBase = new();
		private readonly StatModifierListMultiplyTotal modifiersMultiplyTotal = new();

		private float baseValue;
		private float finalValue;

		public float BaseValue { get => baseValue; set => baseValue = value; }
		public float FinalValue => finalValue;

		public Stat2(float baseValue = 0)
		{
			this.baseValue = baseValue;
			finalValue = baseValue;
		}

		public void AddModifierAdd(SimpleStatModifier modifier)
		{
			modifiersAdd.Add(modifier);
			CalculateFinalValue();
		}

		public void AddModifierMultiplyBase(SimpleStatModifier modifier)
		{
			modifiersMultiplyBase.Add(modifier);
			CalculateFinalValue();
		}

		public void AddModifierMultiplyTotal(SimpleStatModifier modifier)
		{
			modifiersMultiplyTotal.Add(modifier);
			CalculateFinalValue();
		}

		private void CalculateFinalValue()
		{
			finalValue = 0;
			finalValue = modifiersAdd.Calculate(finalValue);
			finalValue = modifiersMultiplyBase.Calculate(finalValue);
			finalValue = modifiersMultiplyTotal.Calculate(finalValue);
		}
	}
}