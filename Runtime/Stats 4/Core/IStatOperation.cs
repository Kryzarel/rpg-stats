namespace Kryz.RPG.Stats4
{
	public interface IStatOperation
	{
		float Calculate(float statValue, IStat innerStat);
	}
}