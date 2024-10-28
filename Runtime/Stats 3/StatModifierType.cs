namespace Kryz.RPG.Stats3
{
	public class StatModifierType<T, TList> : IStatModifierType<T> where T : struct, IStatModifier where TList : IStatModifierList<T>, new()
	{
		public int Priority { get; private set; }

		public StatModifierType(int priority) { Priority = priority; }

		public bool IsListValid(IStatModifierList list)
		{
			return list is TList;
		}

		public IStatModifierList<T> CreateList()
		{
			return new TList();
		}

		IStatModifierList IStatModifierType.CreateList()
		{
			return CreateList();
		}
	}
}