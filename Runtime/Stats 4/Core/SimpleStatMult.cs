namespace Kryz.RPG.Stats4
{
	public class SimpleStatMult<T> : SimpleStat<T> where T : struct, IStatModifierData
	{
		public SimpleStatMult(float baseValue = 1) : base(baseValue) { }

		protected override float AddOperation(float currentValue, float modifierValue, T data)
		{
			return currentValue * (1 + modifierValue);
		}

		protected override float RemoveOperation(float currentValue, float modifierValue, T data)
		{
			return currentValue / (1 + modifierValue);
		}
	}
}