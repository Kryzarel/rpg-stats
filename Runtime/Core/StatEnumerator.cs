using System.Collections;
using System.Collections.Generic;

namespace Kryz.RPG.Stats
{
	public struct StatEnumerator<T> : IEnumerator<StatModifier<T>> where T : struct, IStatModifierData<T>
	{
		private readonly IStat<T> stat;

		private int index;
		private StatModifier<T> current;

		public readonly StatModifier<T> Current => current;
		readonly object IEnumerator.Current => current;

		public StatEnumerator(IStat<T> stat)
		{
			this.stat = stat;
			index = 0;
			current = default;
		}

		public bool MoveNext()
		{
			if (index++ < stat.ModifiersCount)
			{
				current = stat.GetModifier(index);
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