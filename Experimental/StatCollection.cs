using System;
using System.Collections.Generic;

namespace Experimental
{
	public class StatCollection : EnumStatCollection<ModifierType>
	{
		private readonly Dictionary<(ModifierType, int), List<Modifier>> modifiers;

		private readonly int flatOffset;
		private readonly int percentAddOffset;
		private readonly int percentMulOffset;
		private readonly int minOffset;
		private readonly int maxOffset;

		public StatCollection(int statCount) : base(statCount)
		{
			modifiers = new(ModifierTypeCount * StatCount);

			flatOffset = GetTypeOffset(ModifierType.Flat);
			percentAddOffset = GetTypeOffset(ModifierType.PercentAdd);
			percentMulOffset = GetTypeOffset(ModifierType.PercentMul);
			minOffset = GetTypeOffset(ModifierType.Min);
			maxOffset = GetTypeOffset(ModifierType.Max);
		}

		public override int GetTypeOffset(ModifierType modifierType) => (int)modifierType * StatCount;

		public override void Recalculate()
		{
			for (int i = 0; i < StatCount; i++)
			{
				if (dirtyStats[i])
				{
					float value = baseStats[i];
					value += accumulators[flatOffset + i];
					value *= 1 + accumulators[percentAddOffset + i];
					value *= accumulators[percentMulOffset + i];
					value = Math.Max(value, accumulators[minOffset + i]);
					value = Math.Min(value, accumulators[maxOffset + i]);

					finalStats[i] = value;
					dirtyStats[i] = false;
				}
			}
		}

		public void AddModifier(Modifier modifier, int stat)
		{
			(ModifierType Type, int stat) key = (modifier.Type, stat);
			if (!modifiers.TryGetValue(key, out List<Modifier> mods))
			{
				modifiers[key] = mods = new List<Modifier>();
			}

			mods.Add(modifier);
			SetDirty(stat);
		}

		public bool RemoveModifier(Modifier modifier, int stat)
		{
			(ModifierType Type, int stat) key = (modifier.Type, stat);
			if (modifiers.TryGetValue(key, out List<Modifier> mods) && mods.Remove(modifier))
			{
				SetDirty(stat);
				return true;
			}
			return false;
		}

		public void RecalculateAccumulators()
		{
			for (int stat = 0; stat < StatCount; stat++)
			{
				if (!IsDirty(stat))
					continue;

				modifiers.TryGetValue((ModifierType.Flat, stat), out List<Modifier> mods);
				accumulators[flatOffset + stat] = Add(0, mods);

				modifiers.TryGetValue((ModifierType.PercentAdd, stat), out mods);
				accumulators[percentAddOffset + stat] = Add(0, mods);

				modifiers.TryGetValue((ModifierType.PercentMul, stat), out mods);
				accumulators[percentMulOffset + stat] = Multiply(1, mods);

				modifiers.TryGetValue((ModifierType.Min, stat), out mods);
				accumulators[minOffset + stat] = Min(float.MinValue, mods);

				modifiers.TryGetValue((ModifierType.Max, stat), out mods);
				accumulators[maxOffset + stat] = Max(float.MaxValue, mods);
			}
		}

		private static float Add(float value, IReadOnlyList<Modifier> modifiers)
		{
			if (modifiers == null) return value;

			for (int i = 0, count = modifiers.Count; i < count; i++)
			{
				value += modifiers[i].Value;
			}
			return value;
		}

		private static float Multiply(float value, IReadOnlyList<Modifier> modifiers)
		{
			if (modifiers == null) return value;

			for (int i = 0, count = modifiers.Count; i < count; i++)
			{
				value += modifiers[i].Value;
			}
			return value;
		}

		private static float Min(float value, IReadOnlyList<Modifier> modifiers)
		{
			if (modifiers == null) return value;

			for (int i = 0, count = modifiers.Count; i < count; i++)
			{
				value = Math.Min(value, modifiers[i].Value);
			}
			return value;
		}

		private static float Max(float value, IReadOnlyList<Modifier> modifiers)
		{
			if (modifiers == null) return value;

			for (int i = 0, count = modifiers.Count; i < count; i++)
			{
				value = Math.Max(value, modifiers[i].Value);
			}
			return value;
		}
	}
}