using System;

namespace Experimental
{
	public abstract class EnumStatCollection<T> : BaseStatCollection where T : Enum
	{
		protected EnumStatCollection(int statCount) : base(statCount, Enum.GetValues(typeof(T)).Length)
		{
		}

		public abstract int GetTypeOffset(T modifierType);
	}
}