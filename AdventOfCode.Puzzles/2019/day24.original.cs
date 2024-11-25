using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 24, CodeType.Original)]
public class Day_24_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input) =>
		(
			DoPartA(input.Bytes),
			DoPartB(input.Bytes));

	private static string DoPartA(byte[] input)
	{
		var state = 0;
		for (int i = 0, b = 1; i < input.Length; i++)
		{
			if (input[i] != '\n')
			{
				state |= input[i] == '#' ? b : 0;
				b <<= 1;
			}
		}

		var seen = new HashSet<int>() { state, };
		for (var i = 0; ; i++)
		{
			var up = state >> 5;
			var right = (state << 1) & 0b11110_11110_11110_11110_11110;
			var down = state << 5;
			var left = (state >> 1) & 0b01111_01111_01111_01111_01111;

			var newState = 0;
			for (var b = 1; b <= (1 << 24); b <<= 1)
			{
				int IsSet(int v) => (v & b) != 0 ? 1 : 0;
				var cnt = IsSet(up) + IsSet(right) + IsSet(down) + IsSet(left);
				var cell = cnt == 1 || (cnt == 2 && (state & b) == 0) ? b : 0;
				newState |= cell;
			}

			if (seen.Contains(newState))
			{
				return newState.ToString();
			}

			seen.Add(state = newState);
		}
	}

	[SuppressMessage("Style", "IDE1006:Naming Styles")]
	private static string DoPartB(byte[] input)
	{
		var state = new HashSet<(int x, int y, int z)>();
		{
			int x = 1, y = 1;
			for (var i = 0; i < input.Length; i++)
			{
				if (input[i] != '\n')
				{
					if (input[i] == '#')
						state.Add((x, y, 0));
					x++;
					if (x == 6)
						(x, y) = (1, y + 1);
				}
			}

			state.Remove((3, 3, 0));
		}

		for (var i = 0; i < 200; i++)
		{
			var counts = new Dictionary<(int x, int y, int z), int>();
			foreach (var (x, y, z) in state)
			{
				if (x - 1 == 3 && y == 3)
				{
					for (var _y = 1; _y <= 5; _y++)
						Increment(counts, 5, _y, z + 1);
				}
				else if (x == 1)
				{
					Increment(counts, 2, 3, z - 1);
				}
				else
				{
					Increment(counts, x - 1, y, z);
				}

				if (y - 1 == 3 && x == 3)
				{
					for (var _x = 1; _x <= 5; _x++)
						Increment(counts, _x, 5, z + 1);
				}
				else if (y == 1)
				{
					Increment(counts, 3, 2, z - 1);
				}
				else
				{
					Increment(counts, x, y - 1, z);
				}

				if (x + 1 == 3 && y == 3)
				{
					for (var _y = 1; _y <= 5; _y++)
						Increment(counts, 1, _y, z + 1);
				}
				else if (x == 5)
				{
					Increment(counts, 4, 3, z - 1);
				}
				else
				{
					Increment(counts, x + 1, y, z);
				}

				if (y + 1 == 3 && x == 3)
				{
					for (var _x = 1; _x <= 5; _x++)
						Increment(counts, _x, 1, z + 1);
				}
				else if (y == 5)
				{
					Increment(counts, 3, 4, z - 1);
				}
				else
				{
					Increment(counts, x, y + 1, z);
				}
			}

			var newState = new HashSet<(int x, int y, int z)>();
			foreach (var (pos, c) in counts)
			{
				if (c == 1 || (c == 2 && !state.Contains(pos)))
					newState.Add(pos);
			}

			state = newState;
		}

		return state.Count.ToString();
	}

	private static void Increment(Dictionary<(int x, int y, int z), int> counts, int x, int y, int z)
	{
		ref var cnt = ref CollectionsMarshal.GetValueRefOrAddDefault(counts, (x, y, z), out var _);
		cnt++;
	}
}
