using System.Collections;
using System.Collections.Generic;

namespace Kryz.RPG.Stats.Core
{
	public abstract partial class Stat<T>
	{
		public struct Enumerator : IEnumerator<StatModifier<T>>
		{
			private readonly StatContainer<T>[] statContainers;

			private int index;
			private int containerIndex;
			private StatModifier<T> current;

			public readonly StatModifier<T> Current => current;
			readonly object IEnumerator.Current => current;

			public Enumerator(StatContainer<T>[] statContainers)
			{
				this.statContainers = statContainers;
				index = 0;
				containerIndex = 0;
				current = default;
			}

			public bool MoveNext()
			{
				while (containerIndex < statContainers.Length)
				{
					IStat<T> stat = statContainers[containerIndex].Stat;
					if (index < stat.ModifiersCount)
					{
						current = stat.GetModifier(index);
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