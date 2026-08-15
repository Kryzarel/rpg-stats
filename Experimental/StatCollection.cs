using System;
using System.Collections.Generic;

namespace Experimental
{
	public class StatCollection : EnumStatCollection<ModifierType>
	{
		private readonly List<Modifier>[] modifiers;
		private readonly ModifierTypeMask[] dirtyAccumulators;

		private readonly int flatOffset;
		private readonly int percentAddOffset;
		private readonly int percentMulOffset;
		private readonly int minOffset;
		private readonly int maxOffset;

		public StatCollection(int statCount) : base(statCount)
		{
			modifiers = new List<Modifier>[ModifierTypeCount * statCount];
			dirtyAccumulators = new ModifierTypeMask[statCount];

			flatOffset = GetTypeOffset(ModifierType.Flat);
			percentAddOffset = GetTypeOffset(ModifierType.PercentAdd);
			percentMulOffset = GetTypeOffset(ModifierType.PercentMul);
			minOffset = GetTypeOffset(ModifierType.Min);
			maxOffset = GetTypeOffset(ModifierType.Max);

			for (int stat = 0; stat < statCount; stat++)
			{
				accumulators[flatOffset + stat] = 0;
				accumulators[percentAddOffset + stat] = 1;
				accumulators[percentMulOffset + stat] = 1;
				accumulators[minOffset + stat] = float.PositiveInfinity;
				accumulators[maxOffset + stat] = float.NegativeInfinity;
			}
		}

		public override int GetTypeOffset(ModifierType modifierType) => (int)modifierType * StatCount;

		public void AddModifier(Modifier modifier, int stat)
		{
			ref List<Modifier> mods = ref modifiers[GetTypeOffset(modifier.Type) + stat];

			mods ??= new List<Modifier>();

			mods.Add(modifier);
			HandleAdd(modifier, stat);
			dirtyStats[stat] = true;
		}

		public bool RemoveModifier(Modifier modifier, int stat)
		{
			List<Modifier> mods = modifiers[GetTypeOffset(modifier.Type) + stat];

			if (mods != null && mods.Remove(modifier))
			{
				HandleRemove(modifier, stat);
				dirtyStats[stat] = true;
				return true;
			}
			return false;
		}

		protected override void Recalculate(int stat)
		{
			ModifierTypeMask dirty = dirtyAccumulators[stat];

			if (dirty != ModifierTypeMask.None)
			{
				RecalculateAccumulators(stat, dirty);
				dirtyAccumulators[stat] = ModifierTypeMask.None;
			}

			finalStats[stat] = RecalculateStat(stat);
			dirtyStats[stat] = false;
		}

		private void HandleAdd(Modifier modifier, int stat)
		{
			switch (modifier.Type)
			{
				case ModifierType.Flat:
					accumulators[flatOffset + stat] += modifier.Value;
					break;

				case ModifierType.PercentAdd:
					accumulators[percentAddOffset + stat] += modifier.Value;
					break;

				case ModifierType.PercentMul:
					accumulators[percentMulOffset + stat] *= 1f + modifier.Value;
					break;

				case ModifierType.Min:
					float current = accumulators[minOffset + stat];
					accumulators[minOffset + stat] = Math.Min(current, modifier.Value);
					break;

				case ModifierType.Max:
					current = accumulators[maxOffset + stat];
					accumulators[maxOffset + stat] = Math.Max(current, modifier.Value);
					break;

				default:
					dirtyAccumulators[stat] |= (ModifierTypeMask)(1 << (int)modifier.Type);
					break;
			}
		}

		private void HandleRemove(Modifier modifier, int stat)
		{
			switch (modifier.Type)
			{
				case ModifierType.Flat:
					accumulators[flatOffset + stat] -= modifier.Value;
					break;

				case ModifierType.PercentAdd:
					accumulators[percentAddOffset + stat] -= modifier.Value;
					break;

				case ModifierType.PercentMul:
					float value = 1f + modifier.Value;
					if (value != 0)
						accumulators[percentMulOffset + stat] /= value;
					else
						dirtyAccumulators[stat] |= ModifierTypeMask.PercentMul;
					break;

				case ModifierType.Min:
					if (accumulators[minOffset + stat] == modifier.Value)
						dirtyAccumulators[stat] |= ModifierTypeMask.Min;
					break;

				case ModifierType.Max:
					if (accumulators[maxOffset + stat] == modifier.Value)
						dirtyAccumulators[stat] |= ModifierTypeMask.Max;
					break;

				default:
					dirtyAccumulators[stat] |= (ModifierTypeMask)(1 << (int)modifier.Type);
					break;
			}
		}

		private float RecalculateStat(int stat)
		{
			float value = baseStats[stat];
			value += accumulators[flatOffset + stat];
			value *= accumulators[percentAddOffset + stat];
			value *= accumulators[percentMulOffset + stat];
			value = Math.Max(value, accumulators[minOffset + stat]);
			value = Math.Min(value, accumulators[maxOffset + stat]);
			return value;
		}

		private void RecalculateAccumulators(int stat, ModifierTypeMask dirty)
		{
			if ((dirty & ModifierTypeMask.Flat) != 0)
			{
				List<Modifier> mods = modifiers[flatOffset + stat];
				accumulators[flatOffset + stat] = Add(0, mods);
			}

			if ((dirty & ModifierTypeMask.PercentAdd) != 0)
			{
				List<Modifier> mods = modifiers[percentAddOffset + stat];
				accumulators[percentAddOffset + stat] = Add(1, mods);
			}

			if ((dirty & ModifierTypeMask.PercentMul) != 0)
			{
				List<Modifier> mods = modifiers[percentMulOffset + stat];
				accumulators[percentMulOffset + stat] = Multiply(1, mods);
			}

			if ((dirty & ModifierTypeMask.Min) != 0)
			{
				List<Modifier> mods = modifiers[minOffset + stat];
				accumulators[minOffset + stat] = Min(float.PositiveInfinity, mods);
			}

			if ((dirty & ModifierTypeMask.Max) != 0)
			{
				List<Modifier> mods = modifiers[maxOffset + stat];
				accumulators[maxOffset + stat] = Max(float.NegativeInfinity, mods);
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
				value *= 1 + modifiers[i].Value;
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