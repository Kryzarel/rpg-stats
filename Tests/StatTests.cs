using System;
using System.Reflection;
using Kryz.RPG.Stats.Core;
using Kryz.RPG.Stats.Default;
using NUnit.Framework;
using Random = UnityEngine.Random;

namespace Kryz.RPG.Stats.Tests.Editor
{
	public class StatTests
	{
		private const string vals = nameof(baseValues);
		private const float delta = 0.01f;
		private static readonly float[] baseValues = { 0, 0.3f, 2.5f, 5 };
		private static readonly StatModifierType[] modifierTypes = (StatModifierType[])Enum.GetValues(typeof(StatModifierType));

		[Test]
		public void Stat_NoModifiers_FinalEqualsBase([ValueSource(vals)] float baseValue, [ValueSource(vals)] float baseValue2)
		{
			Stat stat = new(baseValue);
			Assert.AreEqual(baseValue, stat.BaseValue, delta);
			Assert.AreEqual(baseValue, stat.FinalValue, delta);

			stat.BaseValue = baseValue2;
			Assert.AreEqual(baseValue2, stat.BaseValue, delta);
			Assert.AreEqual(baseValue2, stat.FinalValue, delta);
		}

		[Test]
		public void Stat_ModifierAdd_FinalEqualsSum([ValueSource(vals)] float baseValue, [ValueSource(vals)] float baseValue2, [ValueSource(vals)] float modifierValue)
		{
			// Arrange
			Stat stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<StatModifierData>(modifierValue, new StatModifierData(StatModifierType.Add)));

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
		public void Stat_ModifierMult_FinalEqualsMult([ValueSource(vals)] float baseValue, [ValueSource(vals)] float baseValue2, [ValueSource(vals)] float modifierValue)
		{
			// Arrange
			Stat stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<StatModifierData>(modifierValue, new StatModifierData(StatModifierType.Mult)));

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
		public void Stat_ModifierMultTotal_FinalEqualsMult([ValueSource(vals)] float baseValue, [ValueSource(vals)] float baseValue2, [ValueSource(vals)] float modifierValue)
		{
			// Arrange
			Stat stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<StatModifierData>(modifierValue, new StatModifierData(StatModifierType.MultTotal)));

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
		public void Stat_ModifierMax_FinalEqualsMax([ValueSource(vals)] float baseValue, [ValueSource(vals)] float baseValue2, [ValueSource(vals)] float modifierValue)
		{
			// Arrange
			Stat stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<StatModifierData>(modifierValue, new StatModifierData(StatModifierType.Max)));

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
		public void Stat_ModifierMin_FinalEqualsMin([ValueSource(vals)] float baseValue, [ValueSource(vals)] float baseValue2, [ValueSource(vals)] float modifierValue)
		{
			// Arrange
			Stat stat = new(baseValue);

			// Act
			stat.AddModifier(new StatModifier<StatModifierData>(modifierValue, new StatModifierData(StatModifierType.Min)));

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
			Assert.AreEqual(baseValue, stat.BaseValue, delta);
			Assert.AreEqual(expected, stat.FinalValue, delta);

			// Act
			stat.AddModifier(new StatModifier<StatModifierData>(modifierMax, new StatModifierData(StatModifierType.Max)));
			expected = Math.Max(expected, modifierMax);

			// Assert
			Assert.AreEqual(baseValue, stat.BaseValue, delta);
			Assert.AreEqual(expected, stat.FinalValue, delta);

			// Act
			stat.AddModifier(new StatModifier<StatModifierData>(modifierMin, new StatModifierData(StatModifierType.Min)));
			expected = Math.Min(expected, modifierMin);

			// Assert
			Assert.AreEqual(baseValue, stat.BaseValue, delta);
			Assert.AreEqual(expected, stat.FinalValue, delta);
		}

		[Test]
		public void StatAdd_RandomModifiers_CorrectCalc([ValueSource(vals)] float baseValue)
		{
			// Arrange
			const int numIterations = 1_000;
			Stat stat = new(baseValue);
			FieldInfo statContainersField = typeof(Stat).GetField("statContainers", BindingFlags.Instance | BindingFlags.NonPublic);
			StatContainer<StatModifierData>[] containers = (StatContainer<StatModifierData>[])statContainersField.GetValue(stat);

			// Act
			for (int i = 0; i < numIterations; i++)
			{
				float modifierValue = Random.Range(-10f, 10f);
				StatModifierType modifierType = modifierTypes[Random.Range(0, modifierTypes.Length)];
				stat.AddModifier(new StatModifier<StatModifierData>(modifierValue, new StatModifierData(modifierType)));

				float expected = (baseValue + containers[0].Stat.FinalValue) * containers[1].Stat.FinalValue * containers[2].Stat.FinalValue;
				expected = Math.Max(expected, containers[3].Stat.FinalValue);
				expected = Math.Min(expected, containers[4].Stat.FinalValue);

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta);
				Assert.AreEqual(expected, stat.FinalValue, delta);
			}

			// Act
			for (int i = 0; i < stat.ModifiersCount; i++)
			{
				StatModifier<StatModifierData> modifier = stat[i];
				stat.RemoveModifier(modifier);

				float expected = (baseValue + containers[0].Stat.FinalValue) * containers[1].Stat.FinalValue * containers[2].Stat.FinalValue;
				expected = Math.Max(expected, containers[3].Stat.FinalValue);
				expected = Math.Min(expected, containers[4].Stat.FinalValue);

				// Assert
				Assert.AreEqual(baseValue, stat.BaseValue, delta);
				Assert.AreEqual(expected, stat.FinalValue, delta);
			}
		}
	}
}