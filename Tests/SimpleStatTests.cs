using System;
using System.Collections.Generic;
using System.Linq;
using Kryz.RPG.Stats.Core;
using NUnit.Framework;
using Random = UnityEngine.Random;

namespace Kryz.RPG.Stats.Tests.Editor
{
	public class SimpleStatTests
	{
		private readonly struct TestModifierData : IStatModifierData<TestModifierData>
		{
			public int CompareTo(TestModifierData other)
			{
				return 0;
			}

			public bool Equals(TestModifierData other)
			{
				return true;
			}
		}

		private const int numIterations = 100;
		private const float delta = 0.0001f;

		private static readonly float[] values = { -1, 0, 0.3f, 1, 2.5f, 5 };
		private static readonly float rangeMin = 0.1f;
		private static readonly float rangeMax = 10f;

		private readonly List<StatModifier<TestModifierData>> modifiers = new();

		[Test]
		public void StatAdd_NoModifiers_FinalEqualsBase([ValueSource(nameof(values))] float baseValue, [ValueSource(nameof(values))] float baseValue2)
		{
			SimpleStatAdd<TestModifierData> stat = new(baseValue);
			Assert.AreEqual(baseValue, stat.BaseValue, delta);
			Assert.AreEqual(baseValue, stat.FinalValue, delta);

			stat.BaseValue = baseValue2;
			Assert.AreEqual(baseValue2, stat.BaseValue, delta);
			Assert.AreEqual(baseValue2, stat.FinalValue, delta);
		}

		[Test]
		public void StatMult_NoModifiers_FinalEqualsBase([ValueSource(nameof(values))] float baseValue, [ValueSource(nameof(values))] float baseValue2)
		{
			SimpleStatMult<TestModifierData> stat = new(baseValue);
			Assert.AreEqual(baseValue, stat.BaseValue, delta);
			Assert.AreEqual(baseValue, stat.FinalValue, delta);

			stat.BaseValue = baseValue2;
			Assert.AreEqual(baseValue2, stat.BaseValue, delta);
			Assert.AreEqual(baseValue2, stat.FinalValue, delta);
		}

		[Test]
		public void StatMax_NoModifiers_FinalEqualsBase([ValueSource(nameof(values))] float baseValue, [ValueSource(nameof(values))] float baseValue2)
		{
			SimpleStatMax<TestModifierData> stat = new(baseValue);
			Assert.AreEqual(baseValue, stat.BaseValue, delta);
			Assert.AreEqual(baseValue, stat.FinalValue, delta);

			stat.BaseValue = baseValue2;
			Assert.AreEqual(baseValue2, stat.BaseValue, delta);
			Assert.AreEqual(baseValue2, stat.FinalValue, delta);
		}

		[Test]
		public void StatAdd_1Modifier_FinalEqualsSum([ValueSource(nameof(values))] float baseValue, [ValueSource(nameof(values))] float baseValue2, [ValueSource(nameof(values))] float modifierValue)
		{
			// Arrange
			SimpleStatAdd<TestModifierData> stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));

			// Assert
			Assert.AreEqual(baseValue, stat.BaseValue, delta);
			Assert.AreEqual(baseValue + modifierValue, stat.FinalValue, delta);

			// Act
			stat.BaseValue = baseValue2;

			// Assert
			Assert.AreEqual(baseValue2, stat.BaseValue, delta);
			Assert.AreEqual(baseValue2 + modifierValue, stat.FinalValue, delta);
		}

		[Test]
		public void StatMult_1Modifier_FinalEqualsMult([ValueSource(nameof(values))] float baseValue, [ValueSource(nameof(values))] float baseValue2, [ValueSource(nameof(values))] float modifierValue)
		{
			// Arrange
			SimpleStatMult<TestModifierData> stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));

			// Assert
			Assert.AreEqual(baseValue, stat.BaseValue, delta);
			Assert.AreEqual(baseValue * (1 + modifierValue), stat.FinalValue, delta);

			// Act
			stat.BaseValue = baseValue2;

			// Assert
			Assert.AreEqual(baseValue2, stat.BaseValue, delta);
			Assert.AreEqual(baseValue2 * (1 + modifierValue), stat.FinalValue, delta);
		}

		[Test]
		public void StatMult_BRUH([ValueSource(nameof(values))] float baseValue)
		{
			// Arrange
			SimpleStatMult<TestModifierData> stat = new(baseValue);

			foreach (float modifierValue in values)
			{
				// Arrange
				StatModifier<TestModifierData> modifier = new(modifierValue, default);

				// Act
				stat.AddModifier(modifier);

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta);
				Assert.AreEqual(baseValue * (1 + modifierValue), stat.FinalValue, delta);

				// Act
				stat.RemoveModifier(modifier);
				Assert.AreEqual(baseValue, stat.BaseValue, delta);

				modifiers.Clear();
				stat.GetModifiers(modifiers);

				float expected = stat.BaseValue;
				for (int i = 0; i < modifiers.Count; i++)
				{
					expected *= 1 + modifiers[i].Value;
				}
				Assert.AreEqual(expected, stat.FinalValue, delta);
			}
		}

		[Test]
		public void StatMax_1Modifier_FinalEqualsMax([ValueSource(nameof(values))] float baseValue, [ValueSource(nameof(values))] float baseValue2, [ValueSource(nameof(values))] float modifierValue)
		{
			// Arrange
			SimpleStatMax<TestModifierData> stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));

			// Assert
			Assert.AreEqual(baseValue, stat.BaseValue, delta);
			Assert.AreEqual(Math.Max(baseValue, modifierValue), stat.FinalValue, delta);

			// Act
			stat.BaseValue = baseValue2;

			// Assert
			Assert.AreEqual(baseValue2, stat.BaseValue, delta);
			Assert.AreEqual(Math.Max(baseValue2, modifierValue), stat.FinalValue, delta);
		}

		[Test]
		public void StatMin_1Modifier_FinalEqualsMin([ValueSource(nameof(values))] float baseValue, [ValueSource(nameof(values))] float baseValue2, [ValueSource(nameof(values))] float modifierValue)
		{
			// Arrange
			SimpleStatMin<TestModifierData> stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));

			// Assert
			Assert.AreEqual(baseValue, stat.BaseValue, delta);
			Assert.AreEqual(Math.Min(baseValue, modifierValue), stat.FinalValue, delta);

			// Act
			stat.BaseValue = baseValue2;

			// Assert
			Assert.AreEqual(baseValue2, stat.BaseValue, delta);
			Assert.AreEqual(Math.Min(baseValue2, modifierValue), stat.FinalValue, delta);
		}

		[Test]
		public void StatAdd_RandomModifiers_FinalEqualsSum([ValueSource(nameof(values))] float baseValue)
		{
			// Arrange
			SimpleStatAdd<TestModifierData> stat = new(baseValue);
			float expected = stat.BaseValue;

			// Act
			for (int i = 0; i < numIterations; i++)
			{
				float modifierValue = Random.Range(rangeMin, rangeMax);
				stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));
				expected += modifierValue;

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta);
				Assert.AreEqual(expected, stat.FinalValue, delta);
			}

			// Act
			modifiers.Clear();
			stat.GetModifiers(modifiers);

			for (int i = 0; i < modifiers.Count; i++)
			{
				int index = Random.Range(0, modifiers.Count);
				StatModifier<TestModifierData> modifier = modifiers[index];
				modifiers.RemoveAt(index);

				if (!stat.RemoveModifier(modifier))
				{
					Assert.Fail("Failed to remove modifier");
				}

				expected -= modifier.Value;

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta);
				Assert.AreEqual(expected, stat.FinalValue, delta);
			}
		}

		[Test]
		public void StatMult_RandomModifiers_FinalEqualsMult([ValueSource(nameof(values))] float baseValue)
		{
			// Arrange
			SimpleStatMult<TestModifierData> stat = new(baseValue);
			float expected = stat.BaseValue;

			// Act
			for (int i = 0; i < numIterations; i++)
			{
				float modifierValue = Random.Range(rangeMin, rangeMax);
				stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));
				expected *= 1 + modifierValue;

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta);
				Assert.AreEqual(expected, stat.FinalValue, delta);
			}

			// Act
			modifiers.Clear();
			stat.GetModifiers(modifiers);

			for (int i = 0; i < modifiers.Count; i++)
			{
				int index = Random.Range(0, modifiers.Count);
				StatModifier<TestModifierData> modifier = modifiers[index];
				modifiers.RemoveAt(index);

				if (!stat.RemoveModifier(modifier))
				{
					Assert.Fail("Failed to remove modifier");
				}

				expected /= 1 + modifier.Value;

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta);
				Assert.AreEqual(expected, stat.FinalValue, delta);
			}
		}

		[Test]
		public void StatMax_RandomModifiers_FinalEqualsMax([ValueSource(nameof(values))] float baseValue)
		{
			// Arrange
			SimpleStatMax<TestModifierData> stat = new(baseValue);
			float expected = stat.BaseValue;

			// Act
			for (int i = 0; i < numIterations; i++)
			{
				float modifierValue = Random.Range(rangeMin, rangeMax);
				stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));
				expected = Math.Max(expected, modifierValue);

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta);
				Assert.AreEqual(expected, stat.FinalValue, delta);
			}

			// Act
			modifiers.Clear();
			stat.GetModifiers(modifiers);

			for (int i = 0; i < modifiers.Count; i++)
			{
				int index = Random.Range(0, modifiers.Count);
				StatModifier<TestModifierData> modifier = modifiers[index];
				modifiers.RemoveAt(index);

				if (!stat.RemoveModifier(modifier))
				{
					Assert.Fail("Failed to remove modifier");
				}

				StatModifier<TestModifierData> modifierMax = modifiers.Max();
				expected = Math.Max(modifierMax.Value, baseValue);

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta);
				Assert.AreEqual(expected, stat.FinalValue, delta);
			}
		}

		[Test]
		public void StatMin_RandomModifiers_FinalEqualsMin([ValueSource(nameof(values))] float baseValue)
		{
			// Arrange
			SimpleStatMin<TestModifierData> stat = new(baseValue);
			float expected = stat.BaseValue;

			// Act
			for (int i = 0; i < numIterations; i++)
			{
				float modifierValue = Random.Range(rangeMin, rangeMax);
				stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));
				expected = Math.Min(expected, modifierValue);

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta);
				Assert.AreEqual(expected, stat.FinalValue, delta);
			}

			// Act
			modifiers.Clear();
			stat.GetModifiers(modifiers);

			for (int i = 0; i < modifiers.Count; i++)
			{
				int index = Random.Range(0, modifiers.Count);
				StatModifier<TestModifierData> modifier = modifiers[index];
				modifiers.RemoveAt(index);

				if (!stat.RemoveModifier(modifier))
				{
					Assert.Fail("Failed to remove modifier");
				}

				StatModifier<TestModifierData> modifierMin = modifiers.Min();
				expected = Math.Min(modifierMin.Value, baseValue);

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta);
				Assert.AreEqual(expected, stat.FinalValue, delta);
			}
		}
	}
}