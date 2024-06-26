﻿namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 18, CodeType.Original)]
public class Day_18_Original : IPuzzle
{
	private char[][] _map;

	private string GetmapState() =>
		new(_map.SelectMany(l => l).ToArray());

	public (string, string) Solve(PuzzleInput input)
	{
		_map = input.Lines
			.Select(s => s.ToArray())
			.ToArray();

		for (var i = 0; i < 10; i++)
		{
			DoIteration();
		}

		var cellTypes = _map.SelectMany(l => l)
			.GroupBy(c => c)
			.ToDictionary(c => c.Key, c => c.Count());
		var part1 = cellTypes['|'] * cellTypes['#'];

		var maxIter = 1_000_000_000;
		var flag = false;
		var seenStates = new Dictionary<string, int>();
		for (var i = 10; i < maxIter; i++)
		{
			DoIteration();

			if (!flag)
			{
				var state = GetmapState();
				if (seenStates.TryGetValue(state, out var value))
				{
					var cycleLength = i - value;
					i = maxIter - ((maxIter - i) % cycleLength);
					flag = true;
				}
				else
				{
					seenStates[state] = i;
				}
			}
		}

		cellTypes = _map.SelectMany(l => l)
			.GroupBy(c => c)
			.ToDictionary(c => c.Key, c => c.Count());
		var part2 = cellTypes['|'] * cellTypes['#'];

		return (part1.ToString(), part2.ToString());
	}

	private void DoIteration()
	{
		_map = Enumerable.Range(0, _map.Length)
			.Select(x => Enumerable.Range(0, _map[0].Length)
				.Select(y => GetNewValue(x, y))
				.ToArray())
			.ToArray();
	}

	private char GetNewValue(int x, int y)
	{
		var neighbors = (
				from x2 in Enumerable.Range(x - 1, 3)
				where x2 >= 0 && x2 < _map.Length
				from y2 in Enumerable.Range(y - 1, 3)
				where y2 >= 0 && y2 < _map[0].Length
				where x2 != x || y2 != y
				group new { } by _map[x2][y2]
			)
			.ToDictionary(c => c.Key, c => c.Count());

		return _map[x][y] switch
		{
			'.' => neighbors.TryGetValue('|', out var value)
					&& value >= 3
					? '|' : '.',
			'|' => neighbors.TryGetValue('#', out var value)
					&& value >= 3
					? '#' : '|',
			'#' => neighbors.TryGetValue('|', out var value)
					&& value >= 1
					&& neighbors.TryGetValue('#', out var value2)
					&& value2 >= 1
					? '#' : '.',
			_ => throw new InvalidOperationException(),
		};
	}
}
