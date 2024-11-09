namespace Kryz.RPG.Stats4
{
	public interface IStatOperation<T> where T : struct, IStatModifierData<T>
	{
		float Calculate(float outerStatValue, IStat<T> innerStat);
	}
}