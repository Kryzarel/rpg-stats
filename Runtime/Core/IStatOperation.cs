namespace Kryz.RPG.Stats
{
	public interface IStatOperation<T> where T : struct, IStatModifierData<T>
	{
		float Calculate(float outerStatValue, IStat<T> innerStat);
	}
}