using System.Numerics;
using System.Threading.Tasks.Dataflow;

namespace AdventOfCode;

public class Day_2019_24_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 24;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		DoPartA(input);
		DoPartB(input);
	}

	private void DoPartA(byte[] input)
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
		for (int i = 0; i < 24 * 60; i++)
		{
			var up = state >> 5;
			var right = (state << 1) & 0b11110_11110_11110_11110_11110;
			var down = state << 5;
			var left = (state >> 1) & 0b01111_01111_01111_01111_01111;

			var newState = 0;
			for (var b = 1; b <= (1 << 24); b <<= 1)
			{
				int isSet(int v) => (v & b) != 0 ? 1 : 0;
				var cnt = isSet(up) + isSet(right) + isSet(down) + isSet(left);
				var cell = cnt == 1 || (cnt == 2 && (state & b) == 0) ? b : 0;
				newState |= cell;
			}

			if (seen.Contains(newState))
			{
				PartA = newState.ToString();
				return;
			}
			seen.Add(state = newState);
		}
	}

	private void DoPartB(byte[] input)
	{
		var state = new HashSet<(int x, int y, int z)>();
		{
			int x = 1, y = 1;
			for (int i = 0; i < input.Length; i++)
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

		for (int i = 0; i < 200; i++)
		{
			var counts = new Dictionary<(int x, int y, int z), int>();
			foreach (var (x, y, z) in state)
			{
				if (x - 1 == 3 && y == 3)
				{
					for (int _y = 1; _y <= 5; _y++)
						counts[(5, _y, z + 1)] = counts.GetValueOrDefault((5, _y, z + 1)) + 1;
				}
				else if (x == 1)
					counts[(2, 3, z - 1)] = counts.GetValueOrDefault((2, 3, z - 1)) + 1;
				else
					counts[(x - 1, y, z)] = counts.GetValueOrDefault((x - 1, y, z)) + 1;

				if (y - 1 == 3 && x == 3)
				{
					for (int _x = 1; _x <= 5; _x++)
						counts[(_x, 5, z + 1)] = counts.GetValueOrDefault((_x, 5, z + 1)) + 1;
				}
				else if (y == 1)
					counts[(3, 2, z - 1)] = counts.GetValueOrDefault((3, 2, z - 1)) + 1;
				else
					counts[(x, y - 1, z)] = counts.GetValueOrDefault((x, y - 1, z)) + 1;

				if (x + 1 == 3 && y == 3)
				{
					for (int _y = 1; _y <= 5; _y++)
						counts[(1, _y, z + 1)] = counts.GetValueOrDefault((1, _y, z + 1)) + 1;
				}
				else if (x == 5)
					counts[(4, 3, z - 1)] = counts.GetValueOrDefault((4, 3, z - 1)) + 1;
				else
					counts[(x + 1, y, z)] = counts.GetValueOrDefault((x + 1, y, z)) + 1;

				if (y + 1 == 3 && x == 3)
				{
					for (int _x = 1; _x <= 5; _x++)
						counts[(_x, 1, z + 1)] = counts.GetValueOrDefault((_x, 1, z + 1)) + 1;
				}
				else if (y == 5)
					counts[(3, 4, z - 1)] = counts.GetValueOrDefault((3, 4, z - 1)) + 1;
				else
					counts[(x, y + 1, z)] = counts.GetValueOrDefault((x, y + 1, z)) + 1;
			}

			var newState = new HashSet<(int x, int y, int z)>();
			foreach (var (pos, c) in counts)
				if (c == 1 || (c == 2 && !state.Contains(pos)))
					newState.Add(pos);

			state = newState;
		}

		PartB = state.Count.ToString();
	}
}
