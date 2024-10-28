using System.Collections.Generic;

namespace Kryz.RPG.Stats3
{
	public class Stat : IStat
	{
		private readonly struct ListContainer
		{
			public readonly int Priority;
			public readonly IStatModifierList List;

			public ListContainer(int priority, IStatModifierList list)
			{
				Priority = priority;
				List = list;
			}
		}

		private class ListContainerComparer : IComparer<ListContainer>
		{
			public int Compare(ListContainer x, ListContainer y)
			{
				int result = x.Priority.CompareTo(y.Priority);
				if (result == 0)
				{
					int listPriority = x.List?.Priority ?? int.MinValue;
					int otherListPriority = y.List?.Priority ?? int.MinValue;
					result = listPriority.CompareTo(otherListPriority);
				}
				return result;
			}
		}

		private static readonly ListContainerComparer comparer = new();
		private readonly List<ListContainer> containers = new();

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
			if (!TryGetModifierList(modifier, out IStatModifierList<T> list, out int index))
			{
				list = CreateModifierList(modifier, index);
			}
			list.Add(modifier);
			CalculateFinalValue();
		}

		public bool RemoveModifier<T>(T modifier) where T : struct, IStatModifier
		{
			if (TryGetModifierList(modifier, out IStatModifierList<T> list, out _) && list.Remove(modifier))
			{
				CalculateFinalValue();
				return true;
			}
			return false;
		}

		public int RemoveModifiersFromSource(object source)
		{
			int numRemoved = 0;
			for (int i = 0; i < containers.Count; i++)
			{
				numRemoved += containers[i].List.RemoveFromSource(source);
			}
			if (numRemoved > 0)
			{
				CalculateFinalValue();
			}
			return numRemoved;
		}

		private bool TryGetModifierList<T>(T modifier, out IStatModifierList<T> list, out int index) where T : struct, IStatModifier
		{
			IStatModifierType<T> type = (IStatModifierType<T>)modifier.Type;

			index = containers.BinarySearch(new ListContainer(modifier.Priority, null!), comparer);

			if (index < 0)
			{
				index = ~index;
			}

			for (int i = index; i < containers.Count; i++)
			{
				ListContainer container = containers[i];
				if (container.Priority > modifier.Priority)
				{
					break;
				}

				if (container.List is IStatModifierList<T> listMatch && type.IsListValid(listMatch))
				{
					list = listMatch;
					return true;
				}
			}

			list = null!;
			return false;
		}

		private IStatModifierList<T> CreateModifierList<T>(T modifier, int startIndex) where T : struct, IStatModifier
		{
			IStatModifierType<T> type = (IStatModifierType<T>)modifier.Type;
			IStatModifierList<T> list = type.CreateList();

			ListContainer container = new(modifier.Priority, list);
			int index = containers.BinarySearch(startIndex, containers.Count - startIndex, container, comparer);
			if (index < 0)
			{
				index = ~index;
			}
			containers.Insert(index, container);

			return list;
		}

		private void CalculateFinalValue()
		{
			finalValue = baseValue;
			for (int i = 0; i < containers.Count; i++)
			{
				finalValue = containers[i].List.Calculate(baseValue, finalValue);
			}
		}

		public void Clear()
		{
			baseValue = 0;
			ClearModifiers();
		}

		public void ClearModifiers()
		{
			for (int i = 0; i < containers.Count; i++)
			{
				containers[i].List.Clear();
			}
			finalValue = baseValue;
		}
	}
}