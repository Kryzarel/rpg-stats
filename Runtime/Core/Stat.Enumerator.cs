using System.Collections;
using System.Collections.Generic;

namespace Kryz.RPG.Stats.Core
{
	public abstract partial class Stat<T>
	{
		public struct Enumerator : IEnumerator<StatModifier<T>>
		{
			private readonly IStat<T>[] stats;

			private int index;
			private int containerIndex;
			private StatModifier<T> current;

			public readonly StatModifier<T> Current => current;
			readonly object IEnumerator.Current => current;

			public Enumerator(IStat<T>[] stats)
			{
				this.stats = stats;
				index = 0;
				containerIndex = 0;
				current = default;
			}

			public bool MoveNext()
			{
				while (containerIndex < stats.Length)
				{
					IStat<T> stat = stats[containerIndex];
					if (index < stat.ModifiersCount)
					{
						current = stat[index];
						return true;
					}
					containerIndex++;
				}
				return false;
			}

			public void Reset()
			{
				index = 0;
				containerIndex = 0;
				current = default;
			}

			public readonly void Dispose() { }
		}
	}
}