using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 11, CodeType.Original)]
public class Day_11_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input.Text
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		var map = RunPart(instructions, 0);
		var part1 = map.Count.ToString();

		map = RunPart(instructions, 1);
		var minX = map.Keys.Select(p => (int)(p >> 32)).Min();
		var maxX = map.Keys.Select(p => (int)(p >> 32)).Max();
		var minY = map.Keys.Select(p => (int)(p & 0xFFFFFFFF)).Min();
		var maxY = map.Keys.Select(p => (int)(p & 0xFFFFFFFF)).Max();

		var part2 = string.Join(Environment.NewLine,
			Enumerable.Range(minY, maxY - minY + 1)
				.Select(y => string.Join("", Enumerable.Range(minX, maxX - minX + 1)
					.Select(x => map.GetValueOrDefault(GetCoordinate(x, y)) == 0 ? ' ' : '█')))
				.Reverse());

		return (part1, part2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static long GetCoordinate(int x, int y) =>
		((long)x << 32) | (uint)y;

	private static Dictionary<long, long> RunPart(long[] instructions, int initialPointValue)
	{
		var pc = new IntCodeComputer([.. instructions], size: 640 * 1024);

		var map = new Dictionary<long, long>();
		var coord = (x: 0, y: 0);
		var dir = 0;

		map[GetCoordinate(coord.x, coord.y)] = initialPointValue;

		while (true)
		{
			var coordValue = GetCoordinate(coord.x, coord.y);
			pc.Inputs.Enqueue(map.GetValueOrDefault(coordValue));

			if (pc.RunProgram() == ProgramStatus.Completed)
				return map;

			map[coordValue] = pc.Outputs.Dequeue();
			var turn = pc.Outputs.Dequeue();
			dir = turn switch
			{
				0 => (dir + 3) % 4,
				1 => (dir + 1) % 4,
				_ => throw new UnreachableException(),
			};
			coord = dir switch
			{
				0 => (coord.x, coord.y + 1),
				1 => (coord.x + 1, coord.y),
				2 => (coord.x, coord.y - 1),
				3 => (coord.x - 1, coord.y),
				_ => throw new UnreachableException(),
			};
		}
	}
}
