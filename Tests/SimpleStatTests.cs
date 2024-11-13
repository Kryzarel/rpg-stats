using System;
using System.Linq;
using Kryz.RPG.Stats4;
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
				return false;
			}
		}

		private const int numIterations = 100;
		private static readonly int[] values = { 0, 1, 2, 3, 10 };

		[Test]
		public void StatAdd_NoModifiers_FinalEqualsBase()
		{
			SimpleStatAdd<TestModifierData> statAdd = new(0);
			Assert.AreEqual(0, statAdd.BaseValue, delta: 0);
			Assert.AreEqual(0, statAdd.FinalValue, delta: 0);

			statAdd.BaseValue = 1;
			Assert.AreEqual(1, statAdd.BaseValue, delta: 0);
			Assert.AreEqual(1, statAdd.FinalValue, delta: 0);
		}

		[Test]
		public void StatMult_NoModifiers_FinalEqualsBase()
		{
			SimpleStatMult<TestModifierData> statAdd = new(0);
			Assert.AreEqual(0, statAdd.BaseValue, delta: 0);
			Assert.AreEqual(0, statAdd.FinalValue, delta: 0);

			statAdd.BaseValue = 1;
			Assert.AreEqual(1, statAdd.BaseValue, delta: 0);
			Assert.AreEqual(1, statAdd.FinalValue, delta: 0);
		}

		[Test]
		public void StatOverride_NoModifiers_FinalEqualsBase()
		{
			SimpleStatOverride<TestModifierData> statAdd = new(0);
			Assert.AreEqual(0, statAdd.BaseValue, delta: 0);
			Assert.AreEqual(0, statAdd.FinalValue, delta: 0);

			statAdd.BaseValue = 1;
			Assert.AreEqual(1, statAdd.BaseValue, delta: 0);
			Assert.AreEqual(1, statAdd.FinalValue, delta: 0);
		}

		[Test]
		public void StatAdd_1Modifier_FinalEqualsSum([ValueSource(nameof(values))] float baseValue, [ValueSource(nameof(values))] float baseValue2, [ValueSource(nameof(values))] float modifierValue)
		{
			// Arrange
			SimpleStatAdd<TestModifierData> stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));

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
		public void StatMult_1Modifier_FinalEqualsMult([ValueSource(nameof(values))] float baseValue, [ValueSource(nameof(values))] float baseValue2, [ValueSource(nameof(values))] float modifierValue)
		{
			// Arrange
			SimpleStatMult<TestModifierData> stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));

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
		public void StatOverride_1Modifier_FinalEqualsModifier([ValueSource(nameof(values))] float baseValue, [ValueSource(nameof(values))] float baseValue2, [ValueSource(nameof(values))] float modifierValue)
		{
			// Arrange
			SimpleStatOverride<TestModifierData> stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));

			// Assert
			Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
			Assert.AreEqual(modifierValue, stat.FinalValue, delta: 0);

			// Act
			stat.BaseValue = baseValue2;

			// Assert
			Assert.AreEqual(baseValue2, stat.BaseValue, delta: 0);
			Assert.AreEqual(modifierValue, stat.FinalValue, delta: 0);
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
				float modifierValue = Random.Range(1f, 100f);
				stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));
				expected += modifierValue;

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
				Assert.AreEqual(expected, stat.FinalValue, delta: 0);
			}

			// Act
			for (int i = 0; i < stat.ModifiersCount; i++)
			{
				StatModifier<TestModifierData> modifier = stat.GetModifier(i);
				if (stat.RemoveModifier(modifier))
				{
					expected -= modifier.Value;
				}

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
				Assert.AreEqual(expected, stat.FinalValue, delta: 0);
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
				float modifierValue = Random.Range(1f, 100f);
				stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));
				expected *= 1 + modifierValue;

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
				Assert.AreEqual(expected, stat.FinalValue, delta: 0);
			}

			// Act
			for (int i = 0; i < stat.ModifiersCount; i++)
			{
				StatModifier<TestModifierData> modifier = stat.GetModifier(i);
				if (stat.RemoveModifier(modifier))
				{
					expected /= 1 + modifier.Value;
				}

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
				Assert.AreEqual(expected, stat.FinalValue, delta: 0);
			}
		}

		[Test]
		public void StatOverride_RandomModifiers_FinalEqualsMult([ValueSource(nameof(values))] float baseValue)
		{
			// Arrange
			SimpleStatOverride<TestModifierData> stat = new(baseValue);
			float expected = stat.BaseValue;

			// Act
			for (int i = 0; i < numIterations; i++)
			{
				float modifierValue = Random.Range(1f, 100f);
				stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));
				expected = Math.Max(expected, modifierValue);

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
				Assert.AreEqual(expected, stat.FinalValue, delta: 0);
			}

			// Act
			for (int i = 0; i < stat.ModifiersCount; i++)
			{
				StatModifier<TestModifierData> modifier = stat.GetModifier(i);
				if (stat.RemoveModifier(modifier))
				{
					expected = stat.Max().Value;
				}

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta: 0);
				Assert.AreEqual(expected, stat.FinalValue, delta: 0);
			}
		}
	}
}