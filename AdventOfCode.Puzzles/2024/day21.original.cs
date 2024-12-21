namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 21, CodeType.Original)]
public partial class Day_21_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var cache = new Dictionary<(string input, int depth), long>();

		var part1 = input.Lines.Sum(l => GetNumericPadInputLength(cache, 2, l) * int.Parse(l.AsSpan()[..3]));
		var part2 = input.Lines.Sum(l => GetNumericPadInputLength(cache, 25, l) * int.Parse(l.AsSpan()[..3]));

		return (part1.ToString(), part2.ToString());
	}

	private static (int x, int y) GetNumericPadLocation(char ch) =>
		ch switch
		{
			'7' => (0, 0),
			'8' => (1, 0),
			'9' => (2, 0),
			'4' => (0, 1),
			'5' => (1, 1),
			'6' => (2, 1),
			'1' => (0, 2),
			'2' => (1, 2),
			'3' => (2, 2),
			' ' => (0, 3),
			'0' => (1, 3),
			'A' => (2, 3),
			_ => default,
		};

	private static long GetNumericPadInputLength(
		Dictionary<(string input, int depth), long> cache,
		int depth,
		string password
	)
	{
		var location = GetNumericPadLocation('A');
		var length = 0L;
		Span<char> span = stackalloc char[16];

		foreach (var ch in password)
		{
			var newLocation = GetNumericPadLocation(ch);
			var delta = (x: newLocation.x - location.x, y: newLocation.y - location.y);
			var dir = (x: delta.x < 0 ? '<' : '>', y: delta.y < 0 ? '^' : 'v');

			if (delta == (0, 0))
			{
				length++;
				continue;
			}

			if (delta.x == 0)
			{
				span.Fill(dir.y);
				span[Math.Abs(delta.y)] = 'A';
				length += GetDirectionPadInputLength(cache, depth, span[..(Math.Abs(delta.y) + 1)]);
				location = newLocation;
				continue;
			}

			if (delta.y == 0)
			{
				span.Fill(dir.x);
				span[Math.Abs(delta.x)] = 'A';
				length += GetDirectionPadInputLength(cache, depth, span[..(Math.Abs(delta.x) + 1)]);
				location = newLocation;
				continue;
			}

			var numButtons = Math.Abs(delta.x) + Math.Abs(delta.y);
			span[numButtons] = 'A';

			var min = long.MaxValue;
			if (location.y != 3 || newLocation.x != 0)
			{
				for (var x = 0; x < Math.Abs(delta.x); x++)
					span[x] = dir.x;
				for (var y = Math.Abs(delta.x); y < numButtons; y++)
					span[y] = dir.y;

				min = Math.Min(min, GetDirectionPadInputLength(cache, depth, span[..(numButtons + 1)]));
			}

			if (location.x != 0 || newLocation.y != 3)
			{
				for (var y = 0; y < Math.Abs(delta.y); y++)
					span[y] = dir.y;
				for (var x = Math.Abs(delta.y); x < numButtons; x++)
					span[x] = dir.x;

				min = Math.Min(min, GetDirectionPadInputLength(cache, depth, span[..(numButtons + 1)]));
			}

			length += min;
			location = newLocation;
		}

		return length;
	}

	private static (int x, int y) GetDirectionPadLocation(char ch) =>
		ch switch
		{
			' ' => (0, 0),
			'^' => (1, 0),
			'A' => (2, 0),
			'<' => (0, 1),
			'v' => (1, 1),
			'>' => (2, 1),
			_ => default,
		};

	private static long GetDirectionPadInputLength(
		Dictionary<(string input, int depth), long> cache,
		int depth,
		ReadOnlySpan<char> password
	)
	{
		var input = password.ToString();
		if (cache.TryGetValue((input, depth), out var length))
			return length;

		length = 0L;

		var location = GetDirectionPadLocation('A');
		Span<char> span = stackalloc char[16];

		foreach (var ch in password)
		{
			var newLocation = GetDirectionPadLocation(ch);
			var delta = (x: newLocation.x - location.x, y: newLocation.y - location.y);
			var dir = (x: delta.x < 0 ? '<' : '>', y: delta.y < 0 ? '^' : 'v');

			if (depth == 1)
			{
				length += Math.Abs(delta.x) + Math.Abs(delta.y) + 1;
				location = newLocation;
				continue;
			}

			if (delta == (0, 0))
			{
				length++;
				continue;
			}

			if (delta.x == 0)
			{
				span.Fill(dir.y);
				span[Math.Abs(delta.y)] = 'A';
				length += GetDirectionPadInputLength(cache, depth - 1, span[..(Math.Abs(delta.y) + 1)]);
				location = newLocation;
				continue;
			}

			if (delta.y == 0)
			{
				span.Fill(dir.x);
				span[Math.Abs(delta.x)] = 'A';
				length += GetDirectionPadInputLength(cache, depth - 1, span[..(Math.Abs(delta.x) + 1)]);
				location = newLocation;
				continue;
			}

			var numButtons = Math.Abs(delta.x) + Math.Abs(delta.y);
			span[numButtons] = 'A';

			var min = long.MaxValue;
			if (location.y != 0 || newLocation.x != 0)
			{
				for (var x = 0; x < Math.Abs(delta.x); x++)
					span[x] = dir.x;
				for (var y = Math.Abs(delta.x); y < numButtons; y++)
					span[y] = dir.y;

				min = Math.Min(min, GetDirectionPadInputLength(cache, depth - 1, span[..(numButtons + 1)]));
			}

			if (location.x != 0 || newLocation.y != 0)
			{
				for (var y = 0; y < Math.Abs(delta.y); y++)
					span[y] = dir.y;
				for (var x = Math.Abs(delta.y); x < numButtons; x++)
					span[x] = dir.x;

				min = Math.Min(min, GetDirectionPadInputLength(cache, depth - 1, span[..(numButtons + 1)]));
			}

			length += min;
			location = newLocation;
		}

		return cache[(input, depth)] = length;
	}
}
