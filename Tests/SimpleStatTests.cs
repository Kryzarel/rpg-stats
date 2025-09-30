using System;
using System.Collections.Generic;
using System.Reflection;
using Kryz.RPG.Stats.Core;
using Kryz.RPG.Stats.Default;
using NUnit.Framework;

namespace Kryz.RPG.Stats.Tests.Editor
{
	public class SimpleStatTests
	{
		private const string valuesSource = nameof(values);

		private static readonly float[] values = new float[] { -10f, -5f, -2f, -1f, -0.5f, 0f, 0.5f, 1f, 2f, 5f, 10f, };

		// ADD
		[Test]
		public void SimpleStatAdd_AddModifier_ChangeBaseValue_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2, [ValueSource(valuesSource)] float modifierValue)
		{
			Stat_AddModifier_ChangeBaseValue_RemoveModifier<SimpleStatAdd<StatModifierData>>(baseValue1, baseValue2, modifierValue, Add);
		}

		[Test]
		public void SimpleStatAdd_AddMultipleModifiers([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			Stat_AddMultipleModifiers<SimpleStatAdd<StatModifierData>>(baseValue1, baseValue2, Add);
		}

		[Test]
		public void SimpleStatAdd_AddRemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			Stat_AddRemoveModifier<SimpleStatAdd<StatModifierData>>(baseValue1, modifierValue, Add);
		}

		[Test]
		public void SimpleStatAdd_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			Stat_RemoveModifier<SimpleStatAdd<StatModifierData>>(baseValue1, modifierValue, Add);
		}

		[Test]
		public void SimpleStatAdd_ChangeBaseValue([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			Stat_ChangeBaseValue<SimpleStatAdd<StatModifierData>>(baseValue1, baseValue2, Add);
		}

		// MULT
		[Test]
		public void SimpleStatMult_AddModifier_ChangeBaseValue_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2, [ValueSource(valuesSource)] float modifierValue)
		{
			Stat_AddModifier_ChangeBaseValue_RemoveModifier<SimpleStatMult<StatModifierData>>(baseValue1, baseValue2, modifierValue, Mul);
		}

		[Test]
		public void SimpleStatMult_AddMultipleModifiers([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			Stat_AddMultipleModifiers<SimpleStatMult<StatModifierData>>(baseValue1, baseValue2, Mul);
		}

		[Test]
		public void SimpleStatMult_AddRemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			Stat_AddRemoveModifier<SimpleStatMult<StatModifierData>>(baseValue1, modifierValue, Mul);
		}

		[Test]
		public void SimpleStatMult_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			Stat_RemoveModifier<SimpleStatMult<StatModifierData>>(baseValue1, modifierValue, Mul);
		}

		[Test]
		public void SimpleStatMult_ChangeBaseValue([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			Stat_ChangeBaseValue<SimpleStatMult<StatModifierData>>(baseValue1, baseValue2, Mul);
		}

		// MIN
		[Test]
		public void SimpleStatMin_AddModifier_ChangeBaseValue_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2, [ValueSource(valuesSource)] float modifierValue)
		{
			Stat_AddModifier_ChangeBaseValue_RemoveModifier<SimpleStatMin<StatModifierData>>(baseValue1, baseValue2, modifierValue, Min);
		}

		[Test]
		public void SimpleStatMin_AddMultipleModifiers([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			Stat_AddMultipleModifiers<SimpleStatMin<StatModifierData>>(baseValue1, baseValue2, Min);
		}

		[Test]
		public void SimpleStatMin_AddRemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			Stat_AddRemoveModifier<SimpleStatMin<StatModifierData>>(baseValue1, modifierValue, Min);
		}

		[Test]
		public void SimpleStatMin_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			Stat_RemoveModifier<SimpleStatMin<StatModifierData>>(baseValue1, modifierValue, Min);
		}

		[Test]
		public void SimpleStatMin_ChangeBaseValue([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			Stat_ChangeBaseValue<SimpleStatMin<StatModifierData>>(baseValue1, baseValue2, Min);
		}

		// MAX
		[Test]
		public void SimpleStatMax_AddModifier_ChangeBaseValue_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2, [ValueSource(valuesSource)] float modifierValue)
		{
			Stat_AddModifier_ChangeBaseValue_RemoveModifier<SimpleStatMax<StatModifierData>>(baseValue1, baseValue2, modifierValue, Max);
		}

		[Test]
		public void SimpleStatMax_AddMultipleModifiers([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			Stat_AddMultipleModifiers<SimpleStatMax<StatModifierData>>(baseValue1, baseValue2, Max);
		}

		[Test]
		public void SimpleStatMax_AddRemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			Stat_AddRemoveModifier<SimpleStatMax<StatModifierData>>(baseValue1, modifierValue, Max);
		}

		[Test]
		public void SimpleStatMax_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			Stat_RemoveModifier<SimpleStatMax<StatModifierData>>(baseValue1, modifierValue, Max);
		}

		[Test]
		public void SimpleStatMax_ChangeBaseValue([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			Stat_ChangeBaseValue<SimpleStatMax<StatModifierData>>(baseValue1, baseValue2, Max);
		}

		private static void Stat_AddModifier_ChangeBaseValue_RemoveModifier<T>(float baseValue1, float baseValue2, float modifierValue, Func<float, float, float> operation) where T : SimpleStat<StatModifierData>
		{
			T stat = CreateSimpleStat<T>(baseValue1);
			SimpleStat_AddModifier(stat, new StatModifier<StatModifierData>(modifierValue, default), operation);
			SimpleStat_ChangeBaseValue(stat, baseValue2, operation);
			SimpleStat_RemoveModifier(stat, new StatModifier<StatModifierData>(modifierValue, default), operation);
		}

		private static void Stat_AddMultipleModifiers<T>(float baseValue1, float baseValue2, Func<float, float, float> operation) where T : SimpleStat<StatModifierData>
		{
			T stat = CreateSimpleStat<T>(baseValue1);

			for (int i = 0; i < values.Length; i++)
			{
				SimpleStat_AddModifier(stat, new StatModifier<StatModifierData>(values[i], default), operation);
			}

			SimpleStat_ChangeBaseValue(stat, baseValue2, operation);
		}

		private static void Stat_AddRemoveModifier<T>(float baseValue1, float modifierValue, Func<float, float, float> operation) where T : SimpleStat<StatModifierData>
		{
			T stat = CreateSimpleStat<T>(baseValue1);
			SimpleStat_AddModifier(stat, new StatModifier<StatModifierData>(modifierValue, default), operation);
			SimpleStat_RemoveModifier(stat, new StatModifier<StatModifierData>(modifierValue, default), operation);
		}

		private static void Stat_RemoveModifier<T>(float baseValue1, float modifierValue, Func<float, float, float> operation) where T : SimpleStat<StatModifierData>
		{
			T stat = CreateSimpleStat<T>(baseValue1);
			SimpleStat_RemoveModifier(stat, new StatModifier<StatModifierData>(modifierValue, default), operation);
		}

		private static void Stat_ChangeBaseValue<T>(float baseValue1, float baseValue2, Func<float, float, float> operation) where T : SimpleStat<StatModifierData>
		{
			T stat = CreateSimpleStat<T>(baseValue1);
			SimpleStat_ChangeBaseValue(stat, baseValue2, operation);
		}

		private static T CreateSimpleStat<T>(float baseValue) where T : SimpleStat<StatModifierData>
		{
			ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { typeof(float) });

			T stat = (T)constructor.Invoke(new object[] { baseValue });

			Assert.AreEqual(stat.BaseValue, baseValue, 0);
			Assert.AreEqual(stat.FinalValue, baseValue, 0);
			Assert.AreEqual(stat.ModifiersCount, 0, 0);

			IReadOnlyStat readonlyStat = stat;
			Assert.AreEqual(readonlyStat.Stats.Count, 0, 0);

			List<StatModifier<StatModifierData>> modifiersList = new();

			stat.GetModifiers(modifiersList);
			Assert.AreEqual(modifiersList.Count, 0, 0);

			return stat;
		}

		private static void SimpleStat_AddModifier(SimpleStat<StatModifierData> stat, StatModifier<StatModifierData> modifier, Func<float, float, float> operation)
		{
			int count = stat.ModifiersCount;
			List<StatModifier<StatModifierData>> modifiersList = new();

			stat.GetModifiers(modifiersList);
			int listCount = modifiersList.Count;
			int modifiersCount = CountModifiersEqualTo(modifiersList, modifier);

			stat.AddModifier(modifier);

			modifiersList.Clear();
			stat.GetModifiers(modifiersList);
			Assert.AreEqual(stat.ModifiersCount, count + 1, 0);
			Assert.IsTrue(modifiersList.Contains(modifier));
			Assert.AreEqual(modifiersList.Count, listCount + 1, 0);
			Assert.AreEqual(CountModifiersEqualTo(modifiersList, modifier), modifiersCount + 1, 0);
			Assert.AreEqual(stat.FinalValue, GetExpectedValue(stat.BaseValue, modifiersList, operation), 0);
		}

		private static void SimpleStat_RemoveModifier(SimpleStat<StatModifierData> stat, StatModifier<StatModifierData> modifier, Func<float, float, float> operation)
		{
			int count = stat.ModifiersCount;
			List<StatModifier<StatModifierData>> modifiersList = new();

			stat.GetModifiers(modifiersList);
			int listCount = modifiersList.Count;
			int modifiersCount = CountModifiersEqualTo(modifiersList, modifier);

			int removed = stat.RemoveModifier(modifier) ? 1 : 0;

			modifiersList.Clear();
			stat.GetModifiers(modifiersList);
			Assert.AreEqual(stat.ModifiersCount, count - removed, 0);
			Assert.AreEqual(modifiersList.Count, listCount - removed, 0);
			Assert.AreEqual(CountModifiersEqualTo(modifiersList, modifier), modifiersCount - removed, 0);
			Assert.AreEqual(stat.FinalValue, GetExpectedValue(stat.BaseValue, modifiersList, operation), 0);
		}

		private static void SimpleStat_ChangeBaseValue(SimpleStat<StatModifierData> stat, float baseValue, Func<float, float, float> operation)
		{
			int count = stat.ModifiersCount;
			List<StatModifier<StatModifierData>> modifiersList = new();

			stat.GetModifiers(modifiersList);
			int listCount = modifiersList.Count;

			stat.BaseValue = baseValue;

			modifiersList.Clear();
			stat.GetModifiers(modifiersList);
			Assert.AreEqual(stat.BaseValue, baseValue, 0);
			Assert.AreEqual(stat.ModifiersCount, count, 0);
			Assert.AreEqual(modifiersList.Count, listCount, 0);
			Assert.AreEqual(stat.FinalValue, GetExpectedValue(stat.BaseValue, modifiersList, operation), 0);
		}

		private static float GetExpectedValue(float baseValue, IReadOnlyList<StatModifier<StatModifierData>> modifiers, Func<float, float, float> operation)
		{
			float value = baseValue;
			for (int i = 0; i < modifiers.Count; i++)
			{
				value = operation(value, modifiers[i].Value);
			}
			return value;
		}

		private static int CountModifiersEqualTo(IReadOnlyList<StatModifier<StatModifierData>> modifiers, StatModifier<StatModifierData> modifier)
		{
			int count = 0;
			for (int i = 0; i < modifiers.Count; i++)
			{
				if (modifiers[i] == modifier)
				{
					count++;
				}
			}
			return count;
		}

		private static float Add(float a, float b) => a + b;
		private static float Mul(float a, float b) => a * (1 + b);
		private static float Min(float a, float b) => Math.Min(a, b);
		private static float Max(float a, float b) => Math.Max(a, b);
	}
}