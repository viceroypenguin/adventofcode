using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
	public unsafe class Day_2017_13_Fastest : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 13;
		public override CodeType CodeType => CodeType.Fastest;

		struct ModMask
		{
			public ulong CycleLength;
			// because max cycle length is > 32, we need 64
			public ulong Mask;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day13.c
			var layers = stackalloc ModMask[input.Length / 4];
			var p = layers;
			uint depth = 0, range = 0, severity = 0;
			foreach (var c in input)
			{
				if (c == '\r')
				{
					var cycleLength = (range - 1) * 2;
					var mod = depth % cycleLength;
					if (mod == 0)
						severity += depth * range;

					p = GetLayer(layers, cycleLength);
					p->CycleLength = cycleLength;
					// set bits are places where we *can't* be
					for (uint i = mod == 0 ? 0 : cycleLength - mod; i < 64; i += cycleLength)
						p->Mask |= 1UL << (int)i;

					depth = range = 0;
				}
				else if (c == ':')
				{
					depth = range;
					range = 0;
				}
				else if (c >= '0')
					range = range * 10 + c - '0';
			}
			PartA = severity.ToString();

			// coalesce cycle-length of 2 into -4, -6, etc.
			for (p = layers; p->CycleLength != 0; p++)
			{
				var q = layers;
				var removeP = false;
				for (; q->CycleLength != 0; q++)
					if ((q->CycleLength > p->CycleLength) && (q->CycleLength % p->CycleLength == 0))
					{
						q->Mask |= p->Mask;
						removeP = true;
					}

				if (removeP)
				{
					*p-- = *--q;
					q->CycleLength = 0;
				}
			}

			var end = p;
			uint delay = 0, fullCycle = 1;
			// if a cycle-length has only one possibility, then
			// we know that the answer % CycleLength == possibility
			// so build up cycle-lengths and delays to match
			for (p = layers; p < end; p++)
			{
				var m = ~p->Mask & ((1UL << (int)p->CycleLength) - 1);
				// only one possibility in cycle-length?
				if (Popcnt.X64.PopCount(m) == 1)
				{
					// what is our mod?
					var validMod = Bmi1.X64.TrailingZeroCount(~p->Mask);

					// increment by calculated cycle-length until we have
					// a matching possibility
					while (delay % p->CycleLength != validMod)
						delay += fullCycle;

					// increase cycle length to accomodate new information
					// cycle will be lcm of all used lengths so far,
					// so that mod of each will remain the same
					fullCycle *= (uint)p->CycleLength / GreatestCommonDivisor(fullCycle, (uint)p->CycleLength);
					*p-- = *--end;
				}
			}

			for (var flag = false; !flag; delay += fullCycle)
			{
				flag = true;
				// ok, now we check the delay against remaining
				// cycle-lengths, to see if we land on an open spot;
				// delay must be open on all layers
				for (p = layers; p < end && flag; p++)
					flag = ((1UL << (int)(delay % p->CycleLength)) & p->Mask) == 0;
			}
			// using for loop, we add fullCycle at end once too many
			PartB = (delay - fullCycle).ToString();
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static uint GreatestCommonDivisor(uint a, uint b)
		{
			while (b != 0) b = a % (a = b);
			return a;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static ModMask* GetLayer(ModMask* p, uint cycleLength)
		{
			while (p->CycleLength != 0 && p->CycleLength != cycleLength)
				p++;
			return p;
		}
	}
}
