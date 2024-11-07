using Kryz.RPG.Stats2.Core;

namespace Kryz.RPG.Stats2
{
	public interface IStat : IStat<StatModifier>
	{
		int RemoveModifiersFromSource(object source);
	}
}