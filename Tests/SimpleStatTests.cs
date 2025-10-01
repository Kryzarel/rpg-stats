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
		private class MatchLessThanOrEqual : IEquatable<StatModifier<StatModifierData>>
		{
			public readonly float Value;
			public MatchLessThanOrEqual(float value) => Value = value;
			public bool Equals(StatModifier<StatModifierData> other) => other.Value <= Value;
		}

		private class MatchGreaterThanOrEqual : IEquatable<StatModifier<StatModifierData>>
		{
			public readonly float Value;
			public MatchGreaterThanOrEqual(float value) => Value = value;
			public bool Equals(StatModifier<StatModifierData> other) => other.Value >= Value;
		}

		private class MatchSource : IEquatable<StatModifier<StatModifierData>>
		{
			public readonly object? Source;
			public MatchSource(object? source) => Source = source;
			public bool Equals(StatModifier<StatModifierData> other) => other.Data.Source == Source;
		}

		private class MatchType : IEquatable<StatModifier<StatModifierData>>
		{
			public readonly StatModifierType Type;
			public MatchType(StatModifierType type) => Type = type;
			public bool Equals(StatModifier<StatModifierData> other) => other.Data.Type == Type;
		}

		private const float delta = 0.00001f;
		private const string valuesSource = nameof(values);
		private const string matchesSource = nameof(matches);

		private static readonly float[] values = { -10f, -5f, -2f, -1f, -0.5f, -0.1f, 0f, 0.1f, 0.5f, 1f, 2f, 5f, 10f, };
		private static readonly IEquatable<StatModifier<StatModifierData>>[] matches = { new MatchLessThanOrEqual(5), new MatchGreaterThanOrEqual(-5), new MatchSource(null), new MatchType(StatModifierType.Add), new MatchType(StatModifierType.Mult) };

		// ADD
		[Test]
		public void SimpleStatAdd_AddModifier_ChangeBaseValue_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2, [ValueSource(valuesSource)] float modifierValue)
		{
			SimpleStat_AddModifier_ChangeBaseValue_RemoveModifier<SimpleStatAdd<StatModifierData>>(baseValue1, baseValue2, modifierValue, Add);
		}

		[Test]
		public void SimpleStatAdd_AddMultipleModifiers_ChangeBaseValue([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			SimpleStat_AddMultipleModifiers_ChangeBaseValue<SimpleStatAdd<StatModifierData>>(baseValue1, baseValue2, Add);
		}

		[Test]
		public void SimpleStatAdd_AddMultipleModifiers_RemoveAll([ValueSource(valuesSource)] float baseValue1, [ValueSource(matchesSource)] IEquatable<StatModifier<StatModifierData>> match)
		{
			SimpleStat_AddMultipleModifiers_RemoveAll<SimpleStatAdd<StatModifierData>>(baseValue1, match, Add);
		}

		[Test]
		public void SimpleStatAdd_AddRemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			SimpleStat_AddRemoveModifier<SimpleStatAdd<StatModifierData>>(baseValue1, modifierValue, Add);
		}

		[Test]
		public void SimpleStatAdd_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			SimpleStat_RemoveModifier<SimpleStatAdd<StatModifierData>>(baseValue1, modifierValue, Add);
		}

		[Test]
		public void SimpleStatAdd_ChangeBaseValue([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			SimpleStat_ChangeBaseValue<SimpleStatAdd<StatModifierData>>(baseValue1, baseValue2, Add);
		}

		// MULT
		[Test]
		public void SimpleStatMult_AddModifier_ChangeBaseValue_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2, [ValueSource(valuesSource)] float modifierValue)
		{
			SimpleStat_AddModifier_ChangeBaseValue_RemoveModifier<SimpleStatMult<StatModifierData>>(baseValue1, baseValue2, modifierValue, Mul);
		}

		[Test]
		public void SimpleStatMult_AddMultipleModifiers_ChangeBaseValue([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			SimpleStat_AddMultipleModifiers_ChangeBaseValue<SimpleStatMult<StatModifierData>>(baseValue1, baseValue2, Mul);
		}

		[Test]
		public void SimpleStatMult_AddMultipleModifiers_RemoveAll([ValueSource(valuesSource)] float baseValue1, [ValueSource(matchesSource)] IEquatable<StatModifier<StatModifierData>> match)
		{
			SimpleStat_AddMultipleModifiers_RemoveAll<SimpleStatMult<StatModifierData>>(baseValue1, match, Mul);
		}

		[Test]
		public void SimpleStatMult_AddRemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			SimpleStat_AddRemoveModifier<SimpleStatMult<StatModifierData>>(baseValue1, modifierValue, Mul);
		}

		[Test]
		public void SimpleStatMult_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			SimpleStat_RemoveModifier<SimpleStatMult<StatModifierData>>(baseValue1, modifierValue, Mul);
		}

		[Test]
		public void SimpleStatMult_ChangeBaseValue([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			SimpleStat_ChangeBaseValue<SimpleStatMult<StatModifierData>>(baseValue1, baseValue2, Mul);
		}

		// MIN
		[Test]
		public void SimpleStatMin_AddModifier_ChangeBaseValue_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2, [ValueSource(valuesSource)] float modifierValue)
		{
			SimpleStat_AddModifier_ChangeBaseValue_RemoveModifier<SimpleStatMin<StatModifierData>>(baseValue1, baseValue2, modifierValue, Min);
		}

		[Test]
		public void SimpleStatMin_AddMultipleModifiers_ChangeBaseValue([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			SimpleStat_AddMultipleModifiers_ChangeBaseValue<SimpleStatMin<StatModifierData>>(baseValue1, baseValue2, Min);
		}

		[Test]
		public void SimpleStatMin_AddMultipleModifiers_RemoveAll([ValueSource(valuesSource)] float baseValue1, [ValueSource(matchesSource)] IEquatable<StatModifier<StatModifierData>> match)
		{
			SimpleStat_AddMultipleModifiers_RemoveAll<SimpleStatMin<StatModifierData>>(baseValue1, match, Min);
		}

		[Test]
		public void SimpleStatMin_AddRemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			SimpleStat_AddRemoveModifier<SimpleStatMin<StatModifierData>>(baseValue1, modifierValue, Min);
		}

		[Test]
		public void SimpleStatMin_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			SimpleStat_RemoveModifier<SimpleStatMin<StatModifierData>>(baseValue1, modifierValue, Min);
		}

		[Test]
		public void SimpleStatMin_ChangeBaseValue([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			SimpleStat_ChangeBaseValue<SimpleStatMin<StatModifierData>>(baseValue1, baseValue2, Min);
		}

		// MAX
		[Test]
		public void SimpleStatMax_AddModifier_ChangeBaseValue_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2, [ValueSource(valuesSource)] float modifierValue)
		{
			SimpleStat_AddModifier_ChangeBaseValue_RemoveModifier<SimpleStatMax<StatModifierData>>(baseValue1, baseValue2, modifierValue, Max);
		}

		[Test]
		public void SimpleStatMax_AddMultipleModifiers_ChangeBaseValue([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			SimpleStat_AddMultipleModifiers_ChangeBaseValue<SimpleStatMax<StatModifierData>>(baseValue1, baseValue2, Max);
		}

		[Test]
		public void SimpleStatMax_AddMultipleModifiers_RemoveAll([ValueSource(valuesSource)] float baseValue1, [ValueSource(matchesSource)] IEquatable<StatModifier<StatModifierData>> match)
		{
			SimpleStat_AddMultipleModifiers_RemoveAll<SimpleStatMax<StatModifierData>>(baseValue1, match, Max);
		}

		[Test]
		public void SimpleStatMax_AddRemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			SimpleStat_AddRemoveModifier<SimpleStatMax<StatModifierData>>(baseValue1, modifierValue, Max);
		}

		[Test]
		public void SimpleStatMax_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue)
		{
			SimpleStat_RemoveModifier<SimpleStatMax<StatModifierData>>(baseValue1, modifierValue, Max);
		}

		[Test]
		public void SimpleStatMax_ChangeBaseValue([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			SimpleStat_ChangeBaseValue<SimpleStatMax<StatModifierData>>(baseValue1, baseValue2, Max);
		}

		private static void SimpleStat_AddModifier_ChangeBaseValue_RemoveModifier<T>(float baseValue1, float baseValue2, float modifierValue, Func<float, float, float> operation) where T : SimpleStat<StatModifierData>
		{
			T stat = CreateSimpleStat<T>(baseValue1);
			SimpleStat_AddModifier(stat, new StatModifier<StatModifierData>(modifierValue, default), operation);
			SimpleStat_ChangeBaseValue(stat, baseValue2, operation);
			SimpleStat_RemoveModifier(stat, new StatModifier<StatModifierData>(modifierValue, default), operation);
		}

		private static void SimpleStat_AddMultipleModifiers_ChangeBaseValue<T>(float baseValue1, float baseValue2, Func<float, float, float> operation) where T : SimpleStat<StatModifierData>
		{
			T stat = CreateSimpleStat<T>(baseValue1);

			for (int i = 0; i < values.Length; i++)
			{
				SimpleStat_AddModifier(stat, new StatModifier<StatModifierData>(values[i], default), operation);
			}

			SimpleStat_ChangeBaseValue(stat, baseValue2, operation);
		}

		private static void SimpleStat_AddMultipleModifiers_RemoveAll<T>(float baseValue1, IEquatable<StatModifier<StatModifierData>> match, Func<float, float, float> operation) where T : SimpleStat<StatModifierData>
		{
			T stat = CreateSimpleStat<T>(baseValue1);

			for (int i = 0; i < values.Length; i++)
			{
				SimpleStat_AddModifier(stat, new StatModifier<StatModifierData>(values[i], default), operation);
			}

			SimpleStat_RemoveAllModifiers(stat, match, operation);
		}

		private static void SimpleStat_AddRemoveModifier<T>(float baseValue1, float modifierValue, Func<float, float, float> operation) where T : SimpleStat<StatModifierData>
		{
			T stat = CreateSimpleStat<T>(baseValue1);
			SimpleStat_AddModifier(stat, new StatModifier<StatModifierData>(modifierValue, default), operation);
			SimpleStat_RemoveModifier(stat, new StatModifier<StatModifierData>(modifierValue, default), operation);
		}

		private static void SimpleStat_RemoveModifier<T>(float baseValue1, float modifierValue, Func<float, float, float> operation) where T : SimpleStat<StatModifierData>
		{
			T stat = CreateSimpleStat<T>(baseValue1);
			SimpleStat_RemoveModifier(stat, new StatModifier<StatModifierData>(modifierValue, default), operation);
		}

		private static void SimpleStat_ChangeBaseValue<T>(float baseValue1, float baseValue2, Func<float, float, float> operation) where T : SimpleStat<StatModifierData>
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
			Assert.AreEqual(stat.FinalValue, GetExpectedValue(stat.BaseValue, modifiersList, operation), delta);
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
			Assert.AreEqual(stat.FinalValue, GetExpectedValue(stat.BaseValue, modifiersList, operation), delta);
		}

		private static void SimpleStat_RemoveAllModifiers<T>(SimpleStat<StatModifierData> stat, T match, Func<float, float, float> operation) where T : IEquatable<StatModifier<StatModifierData>>
		{
			int count = stat.ModifiersCount;
			List<StatModifier<StatModifierData>> modifiersList = new();

			stat.GetModifiers(modifiersList);
			int listCount = modifiersList.Count;

			int removed = stat.RemoveAllModifiers(match);

			modifiersList.Clear();
			stat.GetModifiers(modifiersList);
			Assert.AreEqual(stat.ModifiersCount, count - removed, 0);
			Assert.AreEqual(modifiersList.Count, listCount - removed, 0);
			Assert.AreEqual(stat.FinalValue, GetExpectedValue(stat.BaseValue, modifiersList, operation), delta);
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
			Assert.AreEqual(stat.FinalValue, GetExpectedValue(stat.BaseValue, modifiersList, operation), delta);
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