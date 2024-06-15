using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using static AdventOfCode.Common.Extensions.NumberExtensions;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 08, CodeType.Fastest)]
public sealed partial class Day_08_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;

		var instructions = span[..span.IndexOf((byte)'\n')];
		var insLength = instructions.Length;
		span = span[(insLength + 2)..];

		var mapLength = span.Count((byte)'\n');
		Span<(int from, int idx, int left, int right)> map =
			stackalloc (int, int, int, int)[mapLength];
		var i = 0;
		foreach (var l in span.EnumerateLines())
		{
			if (l.Length == 0)
				break;

			var from = MemoryMarshal.Cast<byte, int>(l)[0] & 0x00_ff_ff_ff;
			var left = MemoryMarshal.Cast<byte, int>(l[7..])[0] & 0x00_ff_ff_ff;
			var right = MemoryMarshal.Cast<byte, int>(l[12..])[0] & 0x00_ff_ff_ff;

			map[i] = (from, i, left, right);
			i++;
		}

		map.Sort();

		Span<int> startIndices = stackalloc int[8];
		var p1Start = 0;

		var j = 0;
		for (i = 0; i < mapLength; i++)
		{
			var (from, index, left, right) = map[i];

			left = ~map.BinarySearch((left, 0, 0, 0));
			right = ~map.BinarySearch((right, 0, 0, 0));

			map[i] = (from, index, left, right);

			if ((from & 0x00_ff_00_00) == 0x00_41_00_00)
			{
				if (from == 0x00_41_41_41)
					p1Start = j;
				startIndices[j++] = i;
			}
		}

		var state = Vector256.LoadUnsafe(ref startIndices[0]);
		var mask = Vector256<int>.Zero;
		for (i = 0; i < j; i++)
			mask = mask.WithElement(i, -1);

		var count = Vector256<int>.One;

		var part1 = 0;
		var part2 = 1L;
		unsafe
		{
			var baseAddr = Unsafe.AsPointer(ref map[0]);

			for (
				i = 0;
				mask != Vector256<int>.Zero;
				i = ++i == insLength ? 0 : i, count += Vector256<int>.One)
			{
				var nextBase = instructions[i] == 'L'
					? Unsafe.Add<int>(baseAddr, 2)
					: Unsafe.Add<int>(baseAddr, 3);

				var gathered = Avx2.GatherMaskVector256(mask, (int*)nextBase, Avx2.ShiftLeftLogical(state, 4), mask, 1);
				var from = Avx2.GatherMaskVector256(mask, (int*)baseAddr, Avx2.ShiftLeftLogical(gathered, 4), mask, 1);

				var isEnd = Avx2.CompareEqual(
					Avx2.And(from, Vector256.Create<int>(0x00_ff_00_00)),
					Vector256.Create<int>(0x00_5A_00_00));
				if (isEnd != Vector256<int>.Zero)
				{
					for (j = 0; j < Vector256<int>.Count; j++)
					{
						if (isEnd[j] != 0)
						{
							part2 = Lcm(part2, count[j]);
							if (j == p1Start)
								part1 = count[j];
							mask = mask.WithElement(j, 0);
						}
					}
				}

				state = gathered;
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
