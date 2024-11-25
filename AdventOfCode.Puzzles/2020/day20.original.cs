using System.Diagnostics;

namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 20, CodeType.Original)]
public class Day_20_Original : IPuzzle
{
	private sealed record Tile
	{
		public required int TileId { get; init; }
		public required int[] ForwardSides { get; init; }
		public required int[] ReverseSides { get; init; }
		public required IReadOnlyList<IReadOnlyList<char>> InnerMap { get; init; }
	}

	private static IEnumerable<IEnumerable<char>> GetRotatedMap(IReadOnlyList<IReadOnlyList<char>> map, int orientation) =>
		orientation switch
		{
			0 => map,
			1 => Enumerable.Range(1, map[0].Count)
				.Select(x => map.Select(s => s[^x])),
			2 => map.Reverse().Select(x => x.Reverse()),
			3 => Enumerable.Range(0, map[0].Count)
				.Select(x => map.Select(s => s[x]).Reverse()),
			4 => map.Select(x => x.Reverse()),
			5 => Enumerable.Range(0, map[0].Count)
				.Select(x => map.Select(s => s[x])),
			6 => map.Reverse(),
			7 => Enumerable.Range(1, map[0].Count)
				.Select(x => map.Select(s => s[^x]).Reverse()),
			_ => throw new UnreachableException(),
		};

	private static readonly string[] s_nessie =
	[
		"                  # ",
		"#    ##    ##    ###",
		" #  #  #  #  #  #   ",
	];

	private Dictionary<int, Tile> _tiles;
	private ILookup<int, int> _tileLookup;
	private int _sideLength;
	private List<(int tileId, int orientation)> _stack;

	public (string, string) Solve(PuzzleInput input)
	{
		var segments = input.Lines
			.Where(s => !string.IsNullOrWhiteSpace(s))
			.Segment(s => s.StartsWith("Tile"))
			.ToList();

		_tiles = segments.Select(ParseTile)
			.ToDictionary(x => x.TileId);
		_sideLength = (int)Math.Sqrt(_tiles.Count);

		_tileLookup = _tiles.Values
			.SelectMany(
				t => t.ForwardSides.Concat(t.ReverseSides),
				(t, s) => (t: t.TileId, s))
			.Distinct()
			.ToLookup(x => x.s, x => x.t);

		BuildGrid();
		var prod =
			(ulong)_stack[0].tileId *
			(ulong)_stack[_sideLength - 1].tileId *
			(ulong)_stack[(_sideLength - 1) * _sideLength].tileId *
			(ulong)_stack[(_sideLength * _sideLength) - 1].tileId;

		var part1 = prod.ToString();

		var map = _stack.Batch(_sideLength)
			.SelectMany(r => r.Select(x => GetRotatedMap(_tiles[x.tileId].InnerMap, x.orientation))
				.Aggregate((l, r) => l.Zip(r, (a, b) => a.Concat(b))))
			.Select(x => x.ToList())
			.ToList();

		for (var i = 0; ; i++)
		{
			var nessie = GetRotatedMap(s_nessie.Select(s => s.ToList()).ToList(), i)
				.Select(x => x.ToList()).ToList();
			var loc = FindNessie(map, nessie, (0, 0));
			if (loc == default) continue;

			while (loc != null)
			{
				ClearNessie(map, nessie, loc.Value);
				loc = FindNessie(map, nessie, loc.Value);
			}

			var part2 = map.SelectMany(x => x).Count(x => x == '#').ToString();
			return (part1, part2);
		}
	}

	private Tile ParseTile(IEnumerable<string> lines)
	{
		var list = lines.ToList();
		var id = int.Parse(list[0].AsSpan()[5..9]);

		var fSides = new int[4];
		var rSides = new int[4];

		(fSides[0], rSides[0]) = ParseSide(list[1]);
		(fSides[1], rSides[3]) = ParseSide(list.Skip(1).Select(s => s[^1]));
		(rSides[2], fSides[2]) = ParseSide(list[10]);
		(rSides[1], fSides[3]) = ParseSide(list.Skip(1).Select(s => s[0]));

		return new Tile
		{
			TileId = id,
			ForwardSides = fSides,
			ReverseSides = rSides,
			InnerMap = list.Take(2..^1).Select(s => s.Take(1..^1).ToList()).ToList(),
		};

		static (int, int) ParseSide(IEnumerable<char> side)
		{
			int i = 0, f = 0, r = 0;
			foreach (var c in side)
			{
				var set = c == '#' ? 1 : 0;
				f |= set << (9 - i);
				r |= set << i;
				i++;
			}

			return (f, r);
		}
	}

	private void BuildGrid()
	{
		_stack = [];
		foreach (var tile in _tiles.Values)
		{
			for (var o = 0; o < 8; o++)
			{
				_stack.Add((tile.TileId, o));
				var gridBuilt = BuildGrid(0, 1);
				if (gridBuilt) return;

				_stack.RemoveAt(0);
			}
		}

		throw new InvalidOperationException("???");
	}

	private int GetSide(
		int index,
		int side)
	{
		var (id, orientation) = _stack[index];
		return GetSide(_tiles[id], orientation, side);
	}

	private static int GetSide(
		Tile tile,
		int orientation,
		int side)
	{
		var arr = orientation >= 4
			? tile.ReverseSides
			: tile.ForwardSides;
		return arr[(orientation + side) % 4];
	}

	private bool BuildGrid(
		int row, int col)
	{
		if (_stack.Count == _tiles.Count) return true;

		if (col == 0)
		{
			var aboveIdx = (row - 1) * _sideLength;
			var aboveSide = GetSide(aboveIdx, 2);
			foreach (var tileIdx in _tileLookup[aboveSide])
			{
				if (_stack.Any(x => x.tileId == tileIdx)) continue;

				var tile = _tiles[tileIdx];

				for (var o = 0; o < 8; o++)
				{
					var side = GetSide(tile, o, 2);
					if (side != aboveSide) continue;

					_stack.Add((tileIdx, ((10 - o) % 4) + ((o & 4) ^ 4)));
					var gridBuilt = BuildGrid(row, 1);
					if (gridBuilt) return true;

					_stack.RemoveAt(_stack.Count - 1);
				}
			}

			return false;
		}
		else
		{
			var nextRow = row;
			var nextCol = col + 1;
			if (nextCol == _sideLength)
				(nextRow, nextCol) = (row + 1, 0);

			var leftIdx = (row * _sideLength) + col - 1;
			var leftSide = GetSide(leftIdx, 1);
			foreach (var tileIdx in _tileLookup[leftSide])
			{
				if (_stack.Exists(x => x.tileId == tileIdx))
					continue;

				var tile = _tiles[tileIdx];
				for (var o = 0; o < 8; o++)
				{
					var side = GetSide(tile, o, 3);
					if (side != leftSide) continue;

					if (row > 0)
					{
						var aboveIdx = leftIdx - _sideLength + 1;
						var aboveSide = GetSide(aboveIdx, 2);
						side = GetSide(tile, o, 2);
						if (side != aboveSide) continue;
					}

					_stack.Add((tileIdx, ((10 - o) % 4) + ((o & 4) ^ 4)));
					var gridBuilt = BuildGrid(nextRow, nextCol);
					if (gridBuilt) return true;

					_stack.RemoveAt(_stack.Count - 1);
				}
			}

			return false;
		}
	}

	private static (int x, int y)? FindNessie(List<List<char>> map, List<List<char>> nessie, (int x, int y) loc)
	{
		for (var y = loc.y; y < map.Count - nessie.Count; y++)
		{
			for (var x = loc.x; x < map[y].Count - nessie[0].Count; x++)
			{
				if (IsNessieHere(map, nessie, (x, y)))
					return (x, y);
			}

			loc.x = 0;
		}

		return default;
	}

	private static bool IsNessieHere(List<List<char>> map, List<List<char>> nessie, (int x, int y) loc)
	{
		for (var y = 0; y < nessie.Count; y++)
		{
			for (var x = 0; x < nessie[y].Count; x++)
			{
				if (nessie[y][x] == '#' && map[y + loc.y][x + loc.x] != '#')
					return false;
			}
		}

		return true;
	}

	private static void ClearNessie(List<List<char>> map, List<List<char>> nessie, (int x, int y) loc)
	{
		for (var y = 0; y < nessie.Count; y++)
		{
			for (var x = 0; x < nessie[y].Count; x++)
			{
				if (nessie[y][x] == '#')
					map[y + loc.y][x + loc.x] = '.';
			}
		}
	}
}
