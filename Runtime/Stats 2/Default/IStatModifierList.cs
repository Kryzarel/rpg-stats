using Kryz.RPG.Stats2.Core;

namespace Kryz.RPG.Stats2
{
	public interface IStatModifierList : IStatModifierList<StatModifier>
	{
		int RemoveModifiersFromSource(object source);
	}
}
