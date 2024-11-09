namespace Kryz.RPG.Stats4
{
	public class SimpleStatAdd<T> : SimpleStat<T> where T : struct, IStatModifierData
	{
		public SimpleStatAdd(float baseValue = 0) : base(baseValue) { }

		protected override float AddOperation(float currentValue, float modifierValue, T data)
		{
			return currentValue + modifierValue;
		}

		protected override float RemoveOperation(float currentValue, float modifierValue, T data)
		{
			return currentValue - modifierValue;
		}
	}
}