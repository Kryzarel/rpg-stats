namespace Kryz.RPG.Stats4
{
	public class SimpleStatMult<T> : SimpleStat<T> where T : struct, IStatModifierData<T>
	{
		public SimpleStatMult(float baseValue = 1) : base(baseValue) { }

		protected override float AddOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			return currentValue * (1 + modifier.Value);
		}

		protected override float RemoveOperation(float baseValue, float currentValue, StatModifier<T> modifier)
		{
			return currentValue / (1 + modifier.Value);
		}
	}
}