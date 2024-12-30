using System.Collections;
using System.Collections.Generic;

namespace Kryz.RPG.Stats.Core
{
	public partial interface IReadOnlyStat<T> where T : struct, IStatModifierData<T>
	{
		public struct Enumerator : IEnumerator<StatModifier<T>>
		{
			private readonly IReadOnlyStat<T> stat;

			private int index;
			private StatModifier<T> current;

			public readonly StatModifier<T> Current => current;
			readonly object IEnumerator.Current => current;

			public Enumerator(IReadOnlyStat<T> stat)
			{
				this.stat = stat;
				index = 0;
				current = default;
			}

			public bool MoveNext()
			{
				if (index < stat.ModifiersCount)
				{
					current = stat[index++];
					return true;
				}
				return false;
			}

			public void Reset()
			{
				index = 0;
				current = default;
			}

			public readonly void Dispose() { }
		}
	}
}