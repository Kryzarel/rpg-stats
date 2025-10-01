using System;
using System.Collections.Generic;
using Kryz.RPG.Stats.Core;
using Kryz.RPG.Stats.Default;
using NUnit.Framework;

namespace Kryz.RPG.Stats.Tests.Editor
{
	public class StatTests
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
		private const string modifierTypesSource = nameof(modifierTypes);
		private const string modifierSourcesSource = nameof(modifierSources);

		private static readonly float[] values = { -5f, -1f, -0.5f, -0.1f, 0f, 0.1f, 0.5f, 1f, 5f, };
		private static readonly IEquatable<StatModifier<StatModifierData>>[] matches = { new MatchLessThanOrEqual(2), new MatchGreaterThanOrEqual(-2), new MatchSource(null), new MatchType(StatModifierType.Add) };
		private static readonly StatModifierType[] modifierTypes = (StatModifierType[])Enum.GetValues(typeof(StatModifierType));
		private static readonly object?[] modifierSources = { null, new() };

		// ADD
		[Test]
		public void Stat_AddModifier_ChangeBaseValue_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2, [ValueSource(valuesSource)] float modifierValue, [ValueSource(modifierTypesSource)] StatModifierType type, [ValueSource(modifierSourcesSource)] object? source)
		{
			Stat stat = CreateStat(baseValue1);
			Stat_AddModifier(stat, new StatModifier<StatModifierData>(modifierValue, new StatModifierData(type, source)));
			Stat_ChangeBaseValue(stat, baseValue2);
			Stat_RemoveModifier(stat, new StatModifier<StatModifierData>(modifierValue, new StatModifierData(type, source)));
		}

		[Test]
		public void Stat_AddMultipleModifiers_ChangeBaseValue([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2, [ValueSource(modifierTypesSource)] StatModifierType type, [ValueSource(modifierSourcesSource)] object? source)
		{
			Stat stat = CreateStat(baseValue1);

			for (int i = 0; i < values.Length; i++)
			{
				Stat_AddModifier(stat, new StatModifier<StatModifierData>(values[i], new StatModifierData(type, source)));
			}

			Stat_ChangeBaseValue(stat, baseValue2);
		}

		[Test]
		public void Stat_AddMultipleModifiers_RemoveAll([ValueSource(valuesSource)] float baseValue1, [ValueSource(modifierTypesSource)] StatModifierType type, [ValueSource(modifierSourcesSource)] object? source, [ValueSource(matchesSource)] IEquatable<StatModifier<StatModifierData>> match)
		{
			Stat stat = CreateStat(baseValue1);

			for (int i = 0; i < values.Length; i++)
			{
				Stat_AddModifier(stat, new StatModifier<StatModifierData>(values[i], new StatModifierData(type, source)));
			}

			Stat_RemoveAllModifiers(stat, match);
		}

		[Test]
		public void Stat_AddRemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue, [ValueSource(modifierTypesSource)] StatModifierType type, [ValueSource(modifierSourcesSource)] object? source)
		{
			Stat stat = CreateStat(baseValue1);
			Stat_AddModifier(stat, new StatModifier<StatModifierData>(modifierValue, new StatModifierData(type, source)));
			Stat_RemoveModifier(stat, new StatModifier<StatModifierData>(modifierValue, new StatModifierData(type, source)));
		}

		[Test]
		public void Stat_RemoveModifier([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float modifierValue, [ValueSource(modifierTypesSource)] StatModifierType type, [ValueSource(modifierSourcesSource)] object? source)
		{
			Stat stat = CreateStat(baseValue1);
			Stat_RemoveModifier(stat, new StatModifier<StatModifierData>(modifierValue, new StatModifierData(type, source)));
		}

		[Test]
		public void Stat_ChangeBaseValue([ValueSource(valuesSource)] float baseValue1, [ValueSource(valuesSource)] float baseValue2)
		{
			Stat stat = CreateStat(baseValue1);
			Stat_ChangeBaseValue(stat, baseValue2);
		}

		private static Stat CreateStat(float baseValue)
		{
			Stat stat = new(baseValue);

			Assert.AreEqual(stat.BaseValue, baseValue, 0);
			Assert.AreEqual(stat.FinalValue, baseValue, 0);
			Assert.AreEqual(stat.ModifiersCount, 0, 0);
			Assert.AreEqual(stat.Stats.Count, modifierTypes.Length, 0);

			List<StatModifier<StatModifierData>> modifiersList = new();

			stat.GetModifiers(modifiersList);
			Assert.AreEqual(modifiersList.Count, 0, 0);

			return stat;
		}

		private static void Stat_AddModifier(Stat stat, StatModifier<StatModifierData> modifier)
		{
			int count = stat.ModifiersCount;
			int innerModifierCount = stat.Stats[(int)modifier.Data.Type].ModifiersCount;
			List<StatModifier<StatModifierData>> modifiersList = new();

			stat.GetModifiers(modifiersList);
			int listCount = modifiersList.Count;
			int modifiersCount = CountModifiersEqualTo(modifiersList, modifier);

			stat.AddModifier(modifier);

			modifiersList.Clear();
			stat.GetModifiers(modifiersList);
			Assert.AreEqual(stat.ModifiersCount, count + 1, 0);
			Assert.AreEqual(stat.Stats[(int)modifier.Data.Type].ModifiersCount, innerModifierCount + 1, 0);
			Assert.IsTrue(modifiersList.Contains(modifier));
			Assert.AreEqual(modifiersList.Count, listCount + 1, 0);
			Assert.AreEqual(CountModifiersEqualTo(modifiersList, modifier), modifiersCount + 1, 0);
			Assert.AreEqual(stat.FinalValue, GetExpectedValue(stat), delta);
		}

		private static void Stat_RemoveModifier(Stat stat, StatModifier<StatModifierData> modifier)
		{
			int count = stat.ModifiersCount;
			int innerModifierCount = stat.Stats[(int)modifier.Data.Type].ModifiersCount;
			List<StatModifier<StatModifierData>> modifiersList = new();

			stat.GetModifiers(modifiersList);
			int listCount = modifiersList.Count;
			int modifiersCount = CountModifiersEqualTo(modifiersList, modifier);

			int removed = stat.RemoveModifier(modifier) ? 1 : 0;

			modifiersList.Clear();
			stat.GetModifiers(modifiersList);
			Assert.AreEqual(stat.ModifiersCount, count - removed, 0);
			Assert.AreEqual(stat.Stats[(int)modifier.Data.Type].ModifiersCount, innerModifierCount - removed, 0);
			Assert.AreEqual(modifiersList.Count, listCount - removed, 0);
			Assert.AreEqual(CountModifiersEqualTo(modifiersList, modifier), modifiersCount - removed, 0);
			Assert.AreEqual(stat.FinalValue, GetExpectedValue(stat), delta);
		}

		private static void Stat_RemoveAllModifiers<T>(Stat stat, T match) where T : IEquatable<StatModifier<StatModifierData>>
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
			Assert.AreEqual(stat.FinalValue, GetExpectedValue(stat), delta);
		}

		private static void Stat_ChangeBaseValue(Stat stat, float baseValue)
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
			Assert.AreEqual(stat.FinalValue, GetExpectedValue(stat), delta);
		}

		private static float GetExpectedValue(Stat stat)
		{
			float finalValue = stat.BaseValue;
			finalValue += stat.Stats[(int)StatModifierType.Add].FinalValue;
			finalValue *= stat.Stats[(int)StatModifierType.Mult].FinalValue;
			finalValue *= stat.Stats[(int)StatModifierType.MultTotal].FinalValue;
			finalValue = Math.Max(finalValue, stat.Stats[(int)StatModifierType.Max].FinalValue);
			finalValue = Math.Min(finalValue, stat.Stats[(int)StatModifierType.Min].FinalValue);
			return finalValue;
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
	}
}