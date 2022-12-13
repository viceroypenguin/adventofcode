using System.Text;

namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 21, CodeType.Original)]
public class Day_21_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input.Text
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		return (
			DoPart(instructions, springScriptA),
			DoPart(instructions, springScriptB));
	}

		const string springScriptA =
@"NOT A J
NOT B T
OR T J
NOT C T
OR T J
AND D J
WALK
";

		const string springScriptB =
@"NOT A J
NOT B T
OR T J
NOT C T
OR T J
AND D J
NOT E T
NOT T T
OR H T
AND T J
RUN
";

	private static string DoPart(long[] instructions, string scriptCode)
	{
		var pc = new IntCodeComputer(instructions);
		foreach (var b in Encoding.ASCII.GetBytes(scriptCode).Where(b => b != '\r'))
			pc.Inputs.Enqueue(b);

		pc.RunProgram();

		return pc.Outputs.Any(o => o > 255)
			? pc.Outputs.Where(o => o > 255).First().ToString()
			: Encoding.ASCII.GetString(
				pc.Outputs.Select(b => (byte)b).ToArray());
	}
}
