using Kryz.RPG.Stats4;
using NUnit.Framework;
using UnityEngine;

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

		[Test]
		public void TestWithoutModifiers()
		{
			{
				SimpleStatAdd<TestModifierData> statAdd = new(0);
				Assert.AreEqual(0, statAdd.BaseValue, delta: 0);
				Assert.AreEqual(0, statAdd.FinalValue, delta: 0);

				statAdd.BaseValue = 1;
				Assert.AreEqual(1, statAdd.BaseValue, delta: 0);
				Assert.AreEqual(1, statAdd.FinalValue, delta: 0);

				SimpleStatMult<TestModifierData> statMult = new(0);
				Assert.AreEqual(0, statMult.BaseValue, delta: 0);
				Assert.AreEqual(0, statMult.FinalValue, delta: 0);

				statMult.BaseValue = 1;
				Assert.AreEqual(1, statMult.BaseValue, delta: 0);
				Assert.AreEqual(1, statMult.FinalValue, delta: 0);

				SimpleStatOverride<TestModifierData> statOverride = new();
				Assert.AreEqual(0, statOverride.BaseValue, delta: 0);
				Assert.AreEqual(0, statOverride.FinalValue, delta: 0);

				statOverride.BaseValue = 1;
				Assert.AreEqual(1, statOverride.BaseValue, delta: 0);
				Assert.AreEqual(1, statOverride.FinalValue, delta: 0);
			}

			{
				SimpleStatAdd<TestModifierData> statAdd = new(1);
				Assert.AreEqual(1, statAdd.BaseValue, delta: 0);
				Assert.AreEqual(1, statAdd.FinalValue, delta: 0);

				statAdd.BaseValue = 1;
				Assert.AreEqual(1, statAdd.BaseValue, delta: 0);
				Assert.AreEqual(1, statAdd.FinalValue, delta: 0);
			}
		}

		[Test]
		public void TestModifiersAdd()
		{
			SimpleStatAdd<TestModifierData> stat = new(0);

			stat.AddModifier(new StatModifier<TestModifierData>(1, default));
			Assert.AreEqual(0, stat.BaseValue, delta: 0);
			Assert.AreEqual(1, stat.FinalValue, delta: 0);

			stat.BaseValue = 1;
			Assert.AreEqual(1, stat.BaseValue, delta: 0);
			Assert.AreEqual(2, stat.FinalValue, delta: 0);

			float expected = stat.FinalValue;
			const int numIterations = 1000;
			for (int i = 0; i < numIterations; i++)
			{
				float modifierValue = Random.Range(1f, 100f);
				expected += modifierValue;
				stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));
				Assert.AreEqual(1, stat.BaseValue, delta: 0);
				Assert.AreEqual(expected, stat.FinalValue, delta: 0);
			}

			for (int i = 0; i < stat.ModifiersCount; i++)
			{
				StatModifier<TestModifierData> modifier = stat.GetModifier(i);
				if (stat.RemoveModifier(modifier))
				{
					expected -= modifier.Value;
				}
				Assert.AreEqual(1, stat.BaseValue, delta: 0);
				Assert.AreEqual(expected, stat.FinalValue, delta: 0);
			}
		}

		[Test]
		public void TestModifiersMult()
		{
			SimpleStatMult<TestModifierData> stat = new(0);

			stat.AddModifier(new StatModifier<TestModifierData>(1, default));
			Assert.AreEqual(0, stat.BaseValue, delta: 0);
			Assert.AreEqual(0, stat.FinalValue, delta: 0);

			stat.BaseValue = 1;
			Assert.AreEqual(1, stat.BaseValue, delta: 0);
			Assert.AreEqual(2, stat.FinalValue, delta: 0);

			stat.ClearModifiers();

			float expected = stat.BaseValue;
			const int numIterations = 1000;
			for (int i = 0; i < numIterations; i++)
			{
				float modifierValue = Random.Range(1f, 100f);
				expected *= 1 + modifierValue;
				stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));
				Assert.AreEqual(1, stat.BaseValue, delta: 0);
				Assert.AreEqual(expected, stat.FinalValue, delta: 0);
			}

			for (int i = 0; i < stat.ModifiersCount; i++)
			{
				StatModifier<TestModifierData> modifier = stat.GetModifier(i);
				if (stat.RemoveModifier(modifier))
				{
					expected /= 1 + modifier.Value;
				}
				Assert.AreEqual(1, stat.BaseValue, delta: 0);
				Assert.AreEqual(expected, stat.FinalValue, delta: 0);
			}
		}

		[Test]
		public void TestModifiersOverride()
		{
			SimpleStatOverride<TestModifierData> stat = new();

			stat.AddModifier(new StatModifier<TestModifierData>(1, default));
			Assert.AreEqual(0, stat.BaseValue, delta: 0);
			Assert.AreEqual(1, stat.FinalValue, delta: 0);

			stat.BaseValue = 1;
			Assert.AreEqual(1, stat.BaseValue, delta: 0);
			Assert.AreEqual(1, stat.FinalValue, delta: 0);

			stat.AddModifier(new StatModifier<TestModifierData>(10, default));
			Assert.AreEqual(1, stat.BaseValue, delta: 0);
			Assert.AreEqual(10, stat.FinalValue, delta: 0);

			float expected = stat.FinalValue;
			const int numIterations = 10;
			for (int i = 0; i < numIterations; i++)
			{
				float modifierValue = Random.Range(1f, 100f);
				if (modifierValue > expected)
				{
					expected = modifierValue;
				}
				stat.AddModifier(new StatModifier<TestModifierData>(modifierValue, default));
				Assert.AreEqual(1, stat.BaseValue, delta: 0);
				Assert.AreEqual(expected, stat.FinalValue, delta: 0);
			}

			for (int i = 0; i < stat.ModifiersCount; i++)
			{
				StatModifier<TestModifierData> modifier = stat.GetModifier(i);
				stat.RemoveModifier(modifier);
				if (stat.ModifiersCount == 0)
				{
					expected = stat.BaseValue;
				}
				Assert.AreEqual(1, stat.BaseValue, delta: 0);
				Assert.AreEqual(expected, stat.FinalValue, delta: 0);
			}
		}
	}
}