using System.Numerics;

namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 13, CodeType.Original)]
public class Day_2017_13_Fastest : IPuzzle
{
	private struct ModMask
	{
		public ulong CycleLength;
		// because max cycle length is > 32, we need 64
		public ulong Mask;
	}

	public (string, string) Solve(PuzzleInput input)
	{
		// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day13.c
		var span = input.Span;

		Span<ModMask> layers = stackalloc ModMask[span.Length / 4];
		var idx = 0;
		uint depth = 0, range = 0, severity = 0;
		foreach (var c in span)
		{
			if (c == '\n')
			{
				var cycleLength = (range - 1) * 2;
				var mod = depth % cycleLength;
				if (mod == 0)
					severity += depth * range;

				idx = GetLayer(layers, idx, cycleLength);
				layers[idx].CycleLength = cycleLength;
				// set bits are places where we *can't* be
				for (var i = mod == 0 ? 0 : cycleLength - mod; i < 64; i += cycleLength)
					layers[idx].Mask |= 1UL << (int)i;

				depth = range = 0;
			}
			else if (c == ':')
			{
				depth = range;
				range = 0;
			}
			else if (c >= '0')
			{
				range = (range * 10) + c - '0';
			}
		}
		var partA = severity.ToString();

		// coalesce cycle-length of 2 into -4, -6, etc.
		for (idx = 0; layers[idx].CycleLength != 0; idx++)
		{
			var jdx = 0;
			var removeP = false;
			for (; layers[jdx].CycleLength != 0; jdx++)
			{
				if ((layers[jdx].CycleLength > layers[idx].CycleLength) && (layers[jdx].CycleLength % layers[idx].CycleLength == 0))
				{
					layers[jdx].Mask |= layers[idx].Mask;
					removeP = true;
				}
			}

			if (removeP)
			{
				layers[idx--] = layers[--jdx];
				layers[jdx].CycleLength = 0;
			}
		}

		var end = idx;
		uint delay = 0, fullCycle = 1;
		// if a cycle-length has only one possibility, then
		// we know that the answer % CycleLength == possibility
		// so build up cycle-lengths and delays to match
		for (idx = 0; idx < end; idx++)
		{
			var m = ~layers[idx].Mask & ((1UL << (int)layers[idx].CycleLength) - 1);
			// only one possibility in cycle-length?
			if (BitOperations.PopCount(m) == 1)
			{
				// what is our mod?
				var validMod = (uint)BitOperations.TrailingZeroCount(~layers[idx].Mask);

				// increment by calculated cycle-length until we have
				// a matching possibility
				while (delay % layers[idx].CycleLength != validMod)
					delay += fullCycle;

				// increase cycle length to accomodate new information
				// cycle will be lcm of all used lengths so far,
				// so that mod of each will remain the same
				fullCycle *= (uint)layers[idx].CycleLength / GreatestCommonDivisor(fullCycle, (uint)layers[idx].CycleLength);
				layers[idx--] = layers[--end];
			}
		}

		for (var flag = false; !flag; delay += fullCycle)
		{
			flag = true;
			// ok, now we check the delay against remaining
			// cycle-lengths, to see if we land on an open spot;
			// delay must be open on all layers
			for (idx = 0; idx < end && flag; idx++)
				flag = ((1UL << (int)(delay % layers[idx].CycleLength)) & layers[idx].Mask) == 0;
		}
		// using for loop, we add fullCycle at end once too many
		var partB = (delay - fullCycle).ToString();

		return (partA.ToString(), partB.ToString());
	}

	private static uint GreatestCommonDivisor(uint a, uint b)
	{
		while (b != 0) b = a % (a = b);
		return a;
	}

	private static int GetLayer(Span<ModMask> span, int i, uint cycleLength)
	{
		while (span[i].CycleLength != 0 && span[i].CycleLength != cycleLength)
			i++;
		return i;
	}
}
