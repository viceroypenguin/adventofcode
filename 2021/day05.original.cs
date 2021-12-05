using System.Collections;

namespace AdventOfCode;

public class Day_2021_05_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 5;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var lines = input.GetLines()
			.Select(l => Regex.Match(l, @"(\d+),(\d+) -> (\d+),(\d+)"))
			.Select(m => (
				x1: Convert.ToInt32(m.Groups[1].Value),
				y1: Convert.ToInt32(m.Groups[2].Value),
				x2: Convert.ToInt32(m.Groups[3].Value),
				y2: Convert.ToInt32(m.Groups[4].Value)))
			.ToList();

		DoPartA(lines);
		DoPartB(lines);
	}

	private void DoPartA(List<(int x1, int y1, int x2, int y2)> lines)
	{
		var visited = new Dictionary<(int x, int y), int>();
		foreach (var (x1, y1, x2, y2) in lines)
		{
			if (x1 != x2 && y1 != y2) continue;

			var xDir = Math.Sign(x2 - x1);
			var yDir = Math.Sign(y2 - y1);
			for (int x = x1, y = y1; x != (x2 + xDir) || y != (y2 + yDir); x += xDir, y += yDir)
				visited[(x, y)] = visited.GetValueOrDefault((x, y)) + 1;
		}

		PartA = visited.Where(kvp => kvp.Value > 1).Count().ToString();
	}

	private void DoPartB(List<(int x1, int y1, int x2, int y2)> lines)
	{
		var visited = new Dictionary<(int x, int y), int>();
		foreach (var (x1, y1, x2, y2) in lines)
		{
			var xDir = Math.Sign(x2 - x1);
			var yDir = Math.Sign(y2 - y1);
			for (int x = x1, y = y1; x != (x2 + xDir) || y != (y2 + yDir); x += xDir, y += yDir)
				visited[(x, y)] = visited.GetValueOrDefault((x, y)) + 1;
		}

		PartB = visited.Where(kvp => kvp.Value > 1).Count().ToString();
	}
}
