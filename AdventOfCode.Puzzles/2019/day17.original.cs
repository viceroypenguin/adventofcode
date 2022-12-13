using System.Text;

namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 17, CodeType.Original)]
public class Day_17_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input.Text
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		var pc = new IntCodeComputer(instructions);
		pc.RunProgram();
		var mapData = pc.Outputs.ToList();

		var map = mapData
			.Batch(mapData.IndexOf('\n') + 1)
			.Select(r => r.Select(b => (char)b).ToArray())
			.ToArray();

		var intersections = Enumerable.Range(1, map.Length - 2)
			.SelectMany(y => Enumerable.Range(1, map[0].Length - 2)
				.Where(x => map[y][x] == '#')
				.Where(x => (map[y][x - 1] == '#') && (x < map[0].Length && map[y][x + 1] == '#'))
				.Where(x => (map[y - 1][x] == '#') && (y < map.Length && map[y + 1][x] == '#'))
				.Select(x => (x, y)))
			.ToList();
		var part1 = intersections
			.Sum(p => p.x * p.y)
			.ToString();

		instructions[0] = 2;
		pc = new IntCodeComputer(instructions);
		foreach (var b in Encoding.ASCII.GetBytes(script).Where(b => b != '\r'))
			pc.Inputs.Enqueue(b);

		pc.RunProgram();
		var part2 = pc.Outputs.Last().ToString();

		return (part1, part2);
	}

	const string script =
@"A,C,A,B,C,B,A,C,A,B
R,6,L,10,R,8,R,8
R,12,L,10,R,6,L,10
R,12,L,8,L,10
n
";
}
