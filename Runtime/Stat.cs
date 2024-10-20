using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Kryz.RPG.Stats
{
	public class Stat
	{
		private static readonly StatModifierType[] modifierTypes = (StatModifierType[])Enum.GetValues(typeof(StatModifierType));

		private readonly NativeArray<NativeList<float>> modifiers = new(modifierTypes.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);

		private float baseValue;
		private float finalValue;
		private bool isDirty;

		public float BaseValue { get => baseValue; set => baseValue = value; }
		public float FinalValue => finalValue;

		public Stat(float baseValue = 0)
		{
			for (int i = 0; i < modifiers.Length; i++)
			{
				modifiers[i] = new NativeList<float>(10, Allocator.Persistent);
			}
			this.baseValue = baseValue;
			finalValue = baseValue;
		}

		private void Calculate()
		{
			AddJob addJob = new() { Modifiers = modifiers[0] };
			JobHandle addJobHandle = addJob.Schedule();

			MultiplyBaseJob multiplyBaseJob = new() { Modifiers = modifiers[1], BaseValue = baseValue };
			JobHandle multiplyBaseJobHandle = multiplyBaseJob.Schedule();

			addJobHandle.Complete();
			multiplyBaseJobHandle.Complete();

			MultiplyTotalJob multiplyTotalJob = new() { Modifiers = modifiers[2], StartingValue = addJob.Result + multiplyBaseJob.Result };
			multiplyTotalJob.Run();

			finalValue = multiplyTotalJob.Result;
		}

		[BurstCompile]
		private struct AddJob : IJob
		{
			[ReadOnly]
			public NativeList<float> Modifiers;
			[WriteOnly]
			public float Result;

			public void Execute()
			{
				float result = 0;
				for (int i = 0; i < Modifiers.Length; i++)
				{
					result += Modifiers[i];
				}
				Result = result;
			}
		}

		[BurstCompile]
		private struct MultiplyBaseJob : IJob
		{
			[ReadOnly]
			public float BaseValue;
			[ReadOnly]
			public NativeList<float> Modifiers;
			[WriteOnly]
			public float Result;

			public void Execute()
			{
				float result = 0;
				for (int i = 0; i < Modifiers.Length; i++)
				{
					result += BaseValue * Modifiers[i];
				}
				Result = result;
			}
		}

		[BurstCompile]
		private struct MultiplyTotalJob : IJob
		{
			[ReadOnly]
			public float StartingValue;
			[ReadOnly]
			public NativeList<float> Modifiers;
			[WriteOnly]
			public float Result;

			public void Execute()
			{
				float result = StartingValue;
				for (int i = 0; i < Modifiers.Length; i++)
				{
					result += result * Modifiers[i];
				}
				Result = result;
			}
		}
	}
}