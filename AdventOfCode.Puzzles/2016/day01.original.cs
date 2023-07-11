using System.Diagnostics;

namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 01, CodeType.Original)]
public class Day_01_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input.Text.Split(',').Select(x => x.Trim()).ToList();
		var position = (x: 0, y: 0, dir: 0);

		var set = new HashSet<(int x, int y)>() { (0, 0), };
		var partB = int.MinValue;

		foreach (var i in instructions)
		{
			var newDirection = i[0] == 'L' ?
				position.dir - 1 :
				position.dir + 1;

			newDirection = (newDirection + 4) % 4;
			var distance = Convert.ToInt32(i[1..]);

			if (partB == int.MinValue)
			{
				var path = newDirection switch
				{
					0 => Enumerable.Range(1, distance).Select(i => (position.x, y: position.y + i)),
					1 => Enumerable.Range(1, distance).Select(i => (x: position.x + i, position.y)),
					2 => Enumerable.Range(1, distance).Select(i => (position.x, y: position.y - i)),
					3 => Enumerable.Range(1, distance).Select(i => (x: position.x - i, position.y)),
					_ => throw new UnreachableException(),
				};

				foreach (var p in path)
				{
					if (set.Contains(p))
						partB = Math.Abs(p.x) + Math.Abs(p.y);
					set.Add(p);
				}
			}

			position = newDirection switch
			{
				0 => (position.x, position.y + distance, newDirection),
				1 => (position.x + distance, position.y, newDirection),
				2 => (position.x, position.y - distance, newDirection),
				3 => (position.x - distance, position.y, newDirection),
				_ => throw new UnreachableException(),
			};
		}

		var partA = Math.Abs(position.x) + Math.Abs(position.y);
		return (partA.ToString(), partB.ToString());
	}
}
