namespace Kryz.RPG.Stats3
{
	public interface IStatModifierType
	{
		bool IsListValid(IStatModifierList list);
		IStatModifierList CreateList();
	}

	public interface IStatModifierType<T> : IStatModifierType where T : struct, IStatModifier
	{
		new IStatModifierList<T> CreateList();
	}
}