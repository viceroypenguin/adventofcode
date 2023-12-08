using System.Runtime.InteropServices;
using static AdventOfCode.Common.Extensions.NumberExtensions;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 08, CodeType.Fastest)]
public sealed partial class Day_08_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;

		var instructions = span.Slice(0, span.IndexOf((byte)'\n'));
		span = span.Slice(instructions.Length + 2);

		var mapLength = span.Count((byte)'\n');
		Span<(int from, int left, int right)> map =
			stackalloc (int, int, int)[mapLength];
		var i = 0;
		foreach (var l in span.EnumerateLines())
		{
			if (l.Length == 0)
				break;

			var from = MemoryMarshal.Cast<byte, int>(l)[0] & 0x00_ff_ff_ff;
			var left = MemoryMarshal.Cast<byte, int>(l.Slice(7))[0] & 0x00_ff_ff_ff;
			var right = MemoryMarshal.Cast<byte, int>(l.Slice(12))[0] & 0x00_ff_ff_ff;

			map[i++] = (from, left, right);
		}

		map.Sort();

		Span<int> startIndices = stackalloc int[8];
		var p1Start = 0;

		var j = 0;
		for (i = 0; i < mapLength; i++)
		{
			var (from, left, right) = map[i];
			left = ~map.BinarySearch((left, 0, 0));
			right = ~map.BinarySearch((right, 0, 0));

			map[i] = (from, left, right);

			if ((from & 0x00_ff_00_00) == 0x00_41_00_00)
			{
				startIndices[j++] = i;
				if (from == 0x00_41_41_41)
					p1Start = i;
			}
		}

		startIndices = startIndices[..j];

		var part1 = 0;
		var point = p1Start;
		var destination = 0x00_5A_5A_5A;
		var insLength = instructions.Length;
		for (
			i = 0;
			map[point].from != destination;
			i = ++i == insLength ? 0 : i, part1++)
		{
			var (_, left, right) = map[point];
			point = instructions[i] == 'L' ? left : right;
		}

		var part2 = 1L;
		foreach (var k in startIndices)
		{
			point = k;
			var count = 0;
			for (
				i = 0;
				(map[point].from & 0x00_ff_00_00) != 0x00_5A_00_00;
				i = ++i == insLength ? 0 : i, count++)
			{
				var (_, left, right) = map[point];
				point = instructions[i] == 'L' ? left : right;
			}

			part2 = Lcm(part2, count);
		}

		return (part1.ToString(), part2.ToString());
	}
}
