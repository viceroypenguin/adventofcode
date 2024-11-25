using System.Runtime.InteropServices;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 14, CodeType.Fastest)]
public sealed partial class Day_14_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = input.Bytes.ToArray().AsSpan();
		var width = map.IndexOf((byte)'\n') + 1;

		TiltMapNorth(map, width);
		var part1 = GetLoad(map, width);

		TiltMapWest(map, width);
		TiltMapSouth(map, width);
		TiltMapEast(map, width);

		var it = 1;

		Span<int> scores = stackalloc int[192];
		scores[it] = GetLoad(map, width);

		var hashes = new Dictionary<int, int>(192)
		{
			[GetHashCode(map)] = it,
		};

		var part2 = 0;
		while (true)
		{
			TiltMap(map, width);
			it++;

			var hash = GetHashCode(map);
			ref var dest = ref CollectionsMarshal.GetValueRefOrAddDefault(hashes, hash, out var exists);
			if (exists)
			{
				var cycle = it - dest;
				var remainder = (1_000_000_000 - it) % cycle;
				part2 = scores[dest + remainder];
				break;
			}

			scores[it] = GetLoad(map, width);
			dest = it;
		}

		return (part1.ToString(), part2.ToString());
	}

	private static int GetHashCode(Span<byte> map)
	{
		var code = new HashCode();
		code.AddBytes(map);
		return code.ToHashCode();
	}

	private static void TiltMap(Span<byte> map, int width)
	{
		TiltMapNorth(map, width);
		TiltMapWest(map, width);
		TiltMapSouth(map, width);
		TiltMapEast(map, width);
	}

	private static void TiltMapNorth(Span<byte> map, int width)
	{
		for (var x = 0; x < width - 1; x++)
		{
			var fallTo = x;
			while (map[fallTo] is not (byte)'.')
				fallTo += width;

			for (var y = fallTo + width; y < map.Length; y += width)
			{
				if (map[y] == '#')
				{
					fallTo = y + width;
				}
				else if (map[y] == 'O')
				{
					if (y != fallTo)
					{
						map[fallTo] = (byte)'O';
						map[y] = (byte)'.';
					}

					fallTo += width;
				}
			}
		}
	}

	private static void TiltMapSouth(Span<byte> map, int width)
	{
		for (var x = map.Length - 2; map[x] != '\n'; x--)
		{
			var fallTo = x;
			while (map[fallTo] is not (byte)'.')
				fallTo -= width;

			for (var y = fallTo - width; y >= 0; y -= width)
			{
				if (map[y] == '#')
				{
					fallTo = y - width;
				}
				else if (map[y] == 'O')
				{
					if (y != fallTo)
					{
						map[fallTo] = (byte)'O';
						map[y] = (byte)'.';
					}

					fallTo -= width;
				}
			}
		}
	}

	private static void TiltMapWest(Span<byte> map, int width)
	{
		for (var y = 0; y < map.Length; y += width)
		{
			var fallTo = y;
			while (map[fallTo] is not (byte)'.')
				fallTo++;

			for (var x = fallTo + 1; map[x] != '\n'; x++)
			{
				if (map[x] == '#')
				{
					fallTo = x + 1;
				}
				else if (map[x] == 'O')
				{
					if (x != fallTo)
					{
						map[fallTo] = (byte)'O';
						map[x] = (byte)'.';
					}

					fallTo++;
				}
			}
		}
	}

	private static void TiltMapEast(Span<byte> map, int width)
	{
		for (var y = map.Length - 2; y > 0; y -= width)
		{
			var fallTo = y;
			while (map[fallTo] is not (byte)'.')
				fallTo--;

			for (var x = fallTo - 1; x >= 0 && map[x] != '\n'; x--)
			{
				if (map[x] == '#')
				{
					fallTo = x - 1;
				}
				else if (map[x] == 'O')
				{
					if (x != fallTo)
					{
						map[fallTo] = (byte)'O';
						map[x] = (byte)'.';
					}

					fallTo--;
				}
			}
		}
	}

	private static int GetLoad(Span<byte> map, int width)
	{
		var count = 0;
		var y = map.Count((byte)'\n');
		while (map.Length > 0)
		{
			count += map[..width].Count((byte)'O') * y;
			map = map[width..];
			y--;
		}

		return count;
	}
}
