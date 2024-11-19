namespace Kryz.RPG.Stats.Core
{
	public interface IStatOperation<T> where T : struct, IStatModifierData<T>
	{
		float Calculate(float outerStatValue, IStat<T> innerStat);
	}
}