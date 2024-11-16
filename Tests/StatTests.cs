using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Kryz.RPG.Stats.Tests.Editor
{
	public class StatTests
	{
		private const string vals = nameof(values);
		private static readonly int[] values = { 0, 1, 2, 3, 10 };

		[Test]
		public void Stat_NoModifiers_FinalEqualsBase([ValueSource(vals)] float baseValue, [ValueSource(vals)] float baseValue2)
		{
			Stat stat = new(baseValue);
			Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
			Assert.AreEqual(baseValue, stat.FinalValue, delta: 0);

			stat.BaseValue = baseValue2;
			Assert.AreEqual(baseValue2, stat.BaseValue, delta: 0);
			Assert.AreEqual(baseValue2, stat.FinalValue, delta: 0);
		}

		[Test]
		public void Stat_ModifierAdd_FinalEqualsSum([ValueSource(vals)] float baseValue, [ValueSource(vals)] float baseValue2, [ValueSource(vals)] float modifierValue)
		{
			// Arrange
			Stat stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<StatModifierData>(modifierValue, new StatModifierData(StatModifierType.Add)));

			// Assert
			Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
			Assert.AreEqual(baseValue + modifierValue, stat.FinalValue, delta: 0);

			// Act
			stat.BaseValue = baseValue2;

			// Assert
			Assert.AreEqual(baseValue2, stat.BaseValue, delta: 0);
			Assert.AreEqual(baseValue2 + modifierValue, stat.FinalValue, delta: 0);
		}

		[Test]
		public void Stat_ModifierMult_FinalEqualsMult([ValueSource(vals)] float baseValue, [ValueSource(vals)] float baseValue2, [ValueSource(vals)] float modifierValue)
		{
			// Arrange
			Stat stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<StatModifierData>(modifierValue, new StatModifierData(StatModifierType.Mult)));

			// Assert
			Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
			Assert.AreEqual(baseValue * (1 + modifierValue), stat.FinalValue, delta: 0);

			// Act
			stat.BaseValue = baseValue2;

			// Assert
			Assert.AreEqual(baseValue2, stat.BaseValue, delta: 0);
			Assert.AreEqual(baseValue2 * (1 + modifierValue), stat.FinalValue, delta: 0);
		}

		[Test]
		public void Stat_ModifierMultTotal_FinalEqualsMult([ValueSource(vals)] float baseValue, [ValueSource(vals)] float baseValue2, [ValueSource(vals)] float modifierValue)
		{
			// Arrange
			Stat stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<StatModifierData>(modifierValue, new StatModifierData(StatModifierType.MultTotal)));

			// Assert
			Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
			Assert.AreEqual(baseValue * (1 + modifierValue), stat.FinalValue, delta: 0);

			// Act
			stat.BaseValue = baseValue2;

			// Assert
			Assert.AreEqual(baseValue2, stat.BaseValue, delta: 0);
			Assert.AreEqual(baseValue2 * (1 + modifierValue), stat.FinalValue, delta: 0);
		}

		[Test]
		public void Stat_ModifierMax_FinalEqualsMax([ValueSource(vals)] float baseValue, [ValueSource(vals)] float baseValue2, [ValueSource(vals)] float modifierValue)
		{
			// Arrange
			Stat stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<StatModifierData>(modifierValue, new StatModifierData(StatModifierType.Max)));

			// Assert
			Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
			Assert.AreEqual(Math.Max(baseValue, modifierValue), stat.FinalValue, delta: 0);

			// Act
			stat.BaseValue = baseValue2;

			// Assert
			Assert.AreEqual(baseValue2, stat.BaseValue, delta: 0);
			Assert.AreEqual(Math.Max(baseValue2, modifierValue), stat.FinalValue, delta: 0);
		}

		[Test]
		public void Stat_ModifierMin_FinalEqualsMin([ValueSource(vals)] float baseValue, [ValueSource(vals)] float baseValue2, [ValueSource(vals)] float modifierValue)
		{
			// Arrange
			Stat stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<StatModifierData>(modifierValue, new StatModifierData(StatModifierType.Min)));

			// Assert
			Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
			Assert.AreEqual(Math.Min(baseValue, modifierValue), stat.FinalValue, delta: 0);

			// Act
			stat.BaseValue = baseValue2;

			// Assert
			Assert.AreEqual(baseValue2, stat.BaseValue, delta: 0);
			Assert.AreEqual(Math.Min(baseValue2, modifierValue), stat.FinalValue, delta: 0);
		}

		[Test]
		public void Stat_AllModifiers_CorrectCalc([ValueSource(vals)] float baseValue, [ValueSource(vals)] float modifierAdd, [ValueSource(vals)] float modifierMult, [ValueSource(vals)] float modifierMultTotal, [ValueSource(vals)] float modifierMax, [ValueSource(vals)] float modifierMin)
		{
			// Arrange
			Stat stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<StatModifierData>(modifierAdd, new StatModifierData(StatModifierType.Add)));
			stat.AddModifier(new StatModifier<StatModifierData>(modifierMult, new StatModifierData(StatModifierType.Mult)));
			stat.AddModifier(new StatModifier<StatModifierData>(modifierMultTotal, new StatModifierData(StatModifierType.MultTotal)));
			float expected = (baseValue + modifierAdd) * (1 + modifierMult) * (1 + modifierMultTotal);

			// Assert
			Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
			Assert.AreEqual(expected, stat.FinalValue, delta: 0);

			// Act
			stat.AddModifier(new StatModifier<StatModifierData>(modifierMax, new StatModifierData(StatModifierType.Max)));
			expected = Math.Max(expected, modifierMax);

			// Assert
			Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
			Assert.AreEqual(expected, stat.FinalValue, delta: 0);

			// Act
			stat.AddModifier(new StatModifier<StatModifierData>(modifierMin, new StatModifierData(StatModifierType.Min)));
			expected = Math.Min(expected, modifierMin);

			// Assert
			Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
			Assert.AreEqual(expected, stat.FinalValue, delta: 0);
		}

		// [Test]
		// public void StatAdd_RandomModifiers_FinalEqualsSum([ValueSource(vals)] float baseValue)
		// {
		// 	// Arrange
		// 	SimpleStatAdd<TestModifierData> stat = new(baseValue);
		// 	float expected = stat.BaseValue;

		// 	// Act
		// 	for (int i = 0; i < numIterations; i++)
		// 	{
		// 		float modifierValue = Random.Range(1f, 100f);
		// 		stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));
		// 		expected += modifierValue;

		// 		// Assert
		// 		Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
		// 		Assert.AreEqual(expected, stat.FinalValue, delta: 0);
		// 	}

		// 	// Act
		// 	for (int i = 0; i < stat.ModifiersCount; i++)
		// 	{
		// 		StatModifier<TestModifierData> modifier = stat.GetModifier(i);
		// 		if (stat.RemoveModifier(modifier))
		// 		{
		// 			expected -= modifier.Value;
		// 		}

		// 		// Assert
		// 		Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
		// 		Assert.AreEqual(expected, stat.FinalValue, delta: 0);
		// 	}
		// }
	}
}