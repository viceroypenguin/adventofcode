using System.Diagnostics;

namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 20, CodeType.Original)]
public partial class Day_20_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = BuildMap(input.Span[1..^2]);

		var distances = SuperEnumerable.GetShortestPaths<(int x, int y), int>(
			(0, 0),
			(pos, cost) => map[pos].Select(q => (q, cost + 1)));

		var part1 = distances.Max(d => d.Value.cost);
		var part2 = distances.Count(d => d.Value.cost >= 1000);

		return (part1.ToString(), part2.ToString());
	}

	private static ILookup<(int x, int y), (int x, int y)> BuildMap(ReadOnlySpan<byte> input)
	{
		var set = new HashSet<((int x, int y) from, (int x, int y) to)>();

		var pos = (x: 0, y: 0);
		var stack = new Stack<(int x, int y)>();
		foreach (var ch in input)
		{
			var prev = pos;
			switch (ch)
			{
				case (byte)'N': pos = (pos.x, pos.y + 1); break;
				case (byte)'S': pos = (pos.x, pos.y - 1); break;
				case (byte)'E': pos = (pos.x + 1, pos.y); break;
				case (byte)'W': pos = (pos.x - 1, pos.y); break;

				case (byte)'(': stack.Push(pos); continue;
				case (byte)'|': pos = stack.Peek(); continue;

				// shortcut because input is known to reset on `)`.
				// will fail if path continues after ')'.
				case (byte)')': pos = stack.Pop(); continue;

				default: throw new UnreachableException();
			}

			_ = set.Add((pos, prev));
			_ = set.Add((prev, pos));
		}

		return set.ToLookup(x => x.from, x => x.to);
	}
}
