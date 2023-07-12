using System.Diagnostics;

namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 22, CodeType.Original)]
public class Day_22_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var map = new Dictionary<(int x, int y), char>();

		var lines = input.Lines;
		var n = lines.Length / 2;
		foreach (var l in lines.Select((l, i) => (l, i)))
		{
			foreach (var c in l.l.Select((c, j) => (c, j)))
				map[(n - l.i, c.j - n)] = c.c == '#' ? 'I' : 'C';
		}

		var position = (x: 0, y: 0, dir: 'n');
		var enable = 0;

		void StepPartA()
		{
			var flag = map.TryGetValue((position.x, position.y), out var v)
				&& v == 'I';

			position.dir =
				position.dir == 'n' ?
					flag ? 'e' : 'w' :
				position.dir == 's' ?
					flag ? 'w' : 'e' :
				position.dir == 'e' ?
					flag ? 's' : 'n' :
					/* position.dir == 'w' ? */
					flag ? 'n' : 's';

			if (!flag)
				enable++;
			map[(position.x, position.y)] =
				flag ? 'C' : 'I';

			position = position.dir switch
			{
				'n' => (position.x + 1, position.y, position.dir),
				's' => (position.x - 1, position.y, position.dir),
				'e' => (position.x, position.y + 1, position.dir),
				'w' => (position.x, position.y - 1, position.dir),
				_ => throw new UnreachableException(),
			};
		}

		for (var i = 0; i < 10000; i++)
			StepPartA();
		var partA = enable;

		map = new Dictionary<(int x, int y), char>();
		foreach (var l in lines.Select((l, i) => (l, i)))
		{
			foreach (var c in l.l.Select((c, j) => (c, j)))
				map[(n - l.i, c.j - n)] = c.c == '#' ? 'I' : 'C';
		}

		position = (x: 0, y: 0, dir: 'n');
		enable = 0;
		void StepPartB()
		{
			var state = map.GetValueOrDefault((position.x, position.y), 'C');
			position.dir =
				position.dir == 'n' ?
					state == 'C' ? 'w' :
					state == 'W' ? 'n' :
					state == 'I' ? 'e' :
					/*state == 'F' ? */ 's' :
				position.dir == 's' ?
					state == 'C' ? 'e' :
					state == 'W' ? 's' :
					state == 'I' ? 'w' :
					/*state == 'F' ? */ 'n' :
				position.dir == 'e' ?
					state == 'C' ? 'n' :
					state == 'W' ? 'e' :
					state == 'I' ? 's' :
					/*state == 'F' ? */ 'w' :
					/* position.dir == 'w' ? */
					state == 'C' ? 's' :
					state == 'W' ? 'w' :
					state == 'I' ? 'n' :
					/*state == 'F' ? */ 'e';

			var newState =
				state == 'C' ? 'W' :
				state == 'W' ? 'I' :
				state == 'I' ? 'F' :
				/* state == 'F' ? */ 'C';
			if (newState == 'I')
				enable++;
			map[(position.x, position.y)] = newState;

			position = position.dir switch
			{
				'n' => (position.x + 1, position.y, position.dir),
				's' => (position.x - 1, position.y, position.dir),
				'e' => (position.x, position.y + 1, position.dir),
				'w' => (position.x, position.y - 1, position.dir),
				_ => throw new UnreachableException(),
			};
		}

		for (var i = 0; i < 10000000; i++)
			StepPartB();
		var partB = enable;

		return (partA.ToString(), partB.ToString());
	}
}
