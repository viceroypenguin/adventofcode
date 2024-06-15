using System.Diagnostics;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 17, CodeType.Original)]
public partial class Day_17_Original : IPuzzle
{
	private enum Dir { North, East, South, West, }

	private static readonly (int x, int y, Dir dir)[] s_neighbors =
		[(0, 1, Dir.South), (0, -1, Dir.North), (1, 0, Dir.East), (-1, 0, Dir.West)];

	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetIntMap();

		IEnumerable<((int x, int y, int length, Dir dir), int)> GetP1Neighbors(
			(int x, int y, int length, Dir dir) state, int cost)
		{
			foreach (var (px, py, dir) in s_neighbors)
			{
				if (state.length == 3 && state.dir == dir)
					continue;

				if (state.dir != dir)
				{
					if (state.dir switch
					{
						Dir.East => dir is Dir.West,
						Dir.West => dir is Dir.East,
						Dir.North => dir is Dir.South,
						Dir.South => dir is Dir.North,
						_ => throw new UnreachableException(),
					})
					{ continue; }
				}

				var qx = state.x + px;
				if (!qx.Between(0, map[0].Length - 1))
					continue;

				var qy = state.y + py;
				if (!qy.Between(0, map.Length - 1))
					continue;

				yield return ((
					qx,
					qy,
					dir == state.dir ? state.length + 1 : 1,
					dir),
					cost + map[qy][qx]);
			}
		}

		var cost = SuperEnumerable.GetShortestPathCost<(int x, int y, int length, Dir dir), int>(
			(0, 0, 0, Dir.West),
			GetP1Neighbors,
			st => st.y == map.Length - 1 && st.x == map[0].Length - 1);

		var part1 = cost + 1;

		IEnumerable<((int x, int y, int length, Dir dir), int)> GetP2Neighbors(
			(int x, int y, int length, Dir dir) state, int cost)
		{
			foreach (var (px, py, dir) in s_neighbors)
			{
				if (state.length == 10 && state.dir == dir)
					continue;

				if (state.length < 4 && state.dir != dir)
					continue;

				if (state.dir != dir)
				{
					if (state.dir switch
					{
						Dir.East => dir is Dir.West,
						Dir.West => dir is Dir.East,
						Dir.North => dir is Dir.South,
						Dir.South => dir is Dir.North,
						_ => throw new UnreachableException(),
					})
					{ continue; }
				}

				var qx = state.x + px;
				if (!qx.Between(0, map[0].Length - 1))
					continue;

				var qy = state.y + py;
				if (!qy.Between(0, map.Length - 1))
					continue;

				yield return ((
					qx,
					qy,
					dir == state.dir ? state.length + 1 : 1,
					dir),
					cost + map[qy][qx]);
			}
		}

		cost = SuperEnumerable.GetShortestPathCost<(int x, int y, int length, Dir dir), int>(
			(0, 0, 1, Dir.East),
			GetP2Neighbors,
			st => st.y == map.Length - 1 && st.x == map[0].Length - 1 && st.length >= 4);
		cost = Math.Min(cost, SuperEnumerable.GetShortestPathCost<(int x, int y, int length, Dir dir), int>(
			(0, 0, 1, Dir.South),
			GetP2Neighbors,
			st => st.y == map.Length - 1 && st.x == map[0].Length - 1 && st.length >= 4));

		var part2 = cost;

		return (part1.ToString(), part2.ToString());
	}
}
