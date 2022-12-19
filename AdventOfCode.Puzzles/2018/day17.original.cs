namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 17, CodeType.Original)]
public class Day_17_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		(int start, int end) ParseDesc(string str)
		{
			var split = str.Split(new[] { "..", }, StringSplitOptions.None);
			var start = Convert.ToInt32(split[0]);
			var end = split.Length == 1 ? start : Convert.ToInt32(split[1]);
			return (start, end);
		}

		var data = input.Lines
			.Select(l => l.Split(new[] { ", ", }, StringSplitOptions.None))
			.Select(l =>
			{
				var xDesc = l[0].StartsWith("x") ? l[0] : l[1];
				var yDesc = l[0].StartsWith("y") ? l[0] : l[1];

				return (x: ParseDesc(xDesc.Substring(2)), y: ParseDesc(yDesc.Substring(2)));
			})
			.ToList();

		var (xMin, xMax, yMin, yMax) = data.Aggregate(
			(xMin: int.MaxValue, xMax: 0, yMin: int.MaxValue, yMax: 0),
			(agg, el) => (
				xMin: Math.Min(agg.xMin, el.x.start),
				xMax: Math.Max(agg.xMax, el.x.end),
				yMin: Math.Min(agg.yMin, el.y.start),
				yMax: Math.Max(agg.yMax, el.y.end)));

		// cover water falling on outside
		xMin--;
		xMax++;

		var ground = Enumerable.Range(0, yMax - yMin + 1)
			.Select(_ => Enumerable.Repeat('.', xMax - xMin + 1).ToArray())
			.ToArray();

		foreach (var l in data)
		{
			for (int y = l.y.start; y <= l.y.end; y++)
			{
				var row = ground[y - yMin];
				for (int x = l.x.start; x <= l.x.end; x++)
					row[x - xMin] = '#';
			}
		}

		var sources = new Queue<(int x, int y)>();
		sources.Enqueue((500 - xMin, 0));

		while (sources.Count > 0)
		{
			var s = sources.Dequeue();

			// water falling
			while (s.y < ground.Length && ground[s.y][s.x] == '.')
			{
				ground[s.y][s.x] = '|';
				s.y++;
			}

			if (s.y >= ground.Length)
				continue;


			// hit clay or water; move up a row
			if (ground[s.y][s.x] == '#' || ground[s.y][s.x] == '~')
				s.y--;

			var row = ground[s.y];

			var below = ground[s.y + 1];
			int x = s.x;

			while (row[x] != '#' &&
					(below[x] == '#' || below[x] == '~'))
				row[x++] = '|';

			var right = x;
			var rightWall = row[x] == '#';

			x = s.x;
			while (row[x] != '#' &&
					(below[x] == '#' || below[x] == '~'))
				row[x--] = '|';

			var left = x;
			var leftWall = row[x] == '#';

			if (rightWall && leftWall)
			{
				for (x = left + 1; x <= right - 1; x++)
					row[x] = '~';
				sources.Enqueue((s.x, s.y - 1));
			}
			else
			{
				if (!leftWall && below[left] == '.')
					sources.Enqueue((left, s.y));
				if (!rightWall && below[right] == '.')
					sources.Enqueue((right, s.y));
			}
		}

		// DumpMap();

		var tileTypes = ground.SelectMany(r => r)
			.GroupBy(c => c)
			.ToDictionary(c => c.Key, c => c.Count());

		var part1 = tileTypes['~'] + tileTypes['|'];
		var part2 = tileTypes['~'];

		return (part1.ToString(), part2.ToString());
	}
}
