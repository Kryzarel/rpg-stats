using System;
using System.Collections.Generic;
using Kryz.SharpUtils;

namespace Kryz.RPG.Stats3
{
	public class Stat : IStat
	{
		private class PriorityBucket : IComparable<PriorityBucket>
		{
			public readonly int Priority;
			public readonly List<IStatModifierList> Lists = new();

			public PriorityBucket(int priority)
			{
				Priority = priority;
			}

			public int CompareTo(PriorityBucket other) => Priority.CompareTo(other.Priority);
		}

		private readonly List<PriorityBucket> buckets = new();

		private float baseValue;
		private float finalValue;

		public float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
		public float FinalValue => finalValue;

		public Stat(float baseValue = 0)
		{
			this.baseValue = baseValue;
			finalValue = baseValue;
		}

		public void AddModifier<T>(T modifier) where T : struct, IStatModifier
		{
			PriorityBucket bucket = GetBucket(modifier.Priority);
			IStatModifierList<T> list = GetStatModifierList(bucket, modifier);

			list.Add(modifier);
			CalculateFinalValue();
		}

		public bool RemoveModifier<T>(T modifier) where T : struct, IStatModifier
		{
			PriorityBucket bucket = GetBucket(modifier.Priority);
			IStatModifierList<T> list = GetStatModifierList(bucket, modifier);

			if (list.Remove(modifier))
			{
				CalculateFinalValue();
				return true;
			}
			return false;
		}

		public int RemoveModifiersFromSource(object source)
		{
			int numRemoved = 0;
			for (int i = 0; i < buckets.Count; i++)
			{
				PriorityBucket bucket = buckets[i];
				for (int j = 0; j < bucket.Lists.Count; j++)
				{
					IStatModifierList list = bucket.Lists[j];
					numRemoved += list.RemoveFromSource(source);
				}
			}
			if (numRemoved > 0)
			{
				CalculateFinalValue();
			}
			return numRemoved;
		}

		private PriorityBucket GetBucket(int priority)
		{
			PriorityBucket bucket;
			int index = buckets.BinarySearchLeftmost(b => b.Priority.CompareTo(priority));

			// Bucket not found, create one
			if (index >= buckets.Count || (bucket = buckets[index]).Priority != priority)
			{
				bucket = new PriorityBucket(priority);
				buckets.Insert(index, bucket);
			}
			return bucket;
		}

		private static IStatModifierList<T> GetStatModifierList<T>(PriorityBucket bucket, T modifier) where T : struct, IStatModifier
		{
			IStatModifierType<T> type = (IStatModifierType<T>)modifier.Type;
			int index = bucket.Lists.BinarySearchLeftmost(l => l.Priority.CompareTo(type.Priority));

			// List not found, create one
			if (index >= bucket.Lists.Count || bucket.Lists[index] is not IStatModifierList<T> list || !type.IsListValid(list))
			{
				list = type.CreateList();
				bucket.Lists.Insert(index, list);
			}
			return list;
		}

		private void CalculateFinalValue()
		{
			finalValue = baseValue;
			for (int i = 0; i < buckets.Count; i++)
			{
				PriorityBucket bucket = buckets[i];
				for (int j = 0; j < bucket.Lists.Count; j++)
				{
					IStatModifierList list = bucket.Lists[j];
					finalValue = list.Calculate(baseValue, finalValue);
				}
			}
		}

		public void Clear()
		{
			baseValue = 0;
			ClearModifiers();
		}

		public void ClearModifiers()
		{
			for (int i = 0; i < buckets.Count; i++)
			{
				PriorityBucket bucket = buckets[i];
				for (int j = 0; j < bucket.Lists.Count; j++)
				{
					IStatModifierList list = bucket.Lists[j];
					list.Clear();
				}
			}
			finalValue = baseValue;
		}
	}
}