namespace Kryz.RPG.Stats4
{
	public class SimpleStatAdd<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		public SimpleStatAdd(float baseValue = 0) : base(baseValue) { }

		protected override float AddOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			return currentValue + modifier.Value;
		}

		protected override float RemoveOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			return currentValue - modifier.Value;
		}
	}
}