using System.Numerics;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 25, CodeType.Original)]
public partial class Day_25_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input.Text
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		var items = new[]
		{
			(item: "astrolabe", value: 1 << 0),
			(item: "hologram", value: 1 << 1),
			(item: "space law space brochure", value: 1 << 2),
			(item: "wreath", value: 1 << 3),
			(item: "hypercube", value: 1 << 4),
			(item: "cake", value: 1 << 5),
			(item: "food ration", value: 1 << 6),
			(item: "coin", value: 1 << 7),
		};

		for (var i = 0; ; i++)
		{
			var s = script;
			foreach (var (item, value) in items)
			{
				if ((i & value) != 0)
					s = s + Environment.NewLine + "take " + item;
			}

			s = s + Environment.NewLine + "south";
			var (status, part1) = DoPart(instructions, s);
			if (status == ProgramStatus.Completed)
				return (part1, string.Empty);
		}
	}

	const string script =
@"south
take astrolabe
west
take hologram
south
take space law space brochure
west
take wreath
west
take hypercube
east
east
north
east
south
take cake
west
north
take coin
south
east
east
south
east
take food ration
south
drop astrolabe
drop hologram
drop space law space brochure
drop wreath
drop hypercube
drop cake
drop coin
drop food ration";

	private static (ProgramStatus, string) DoPart(long[] instructions, string scriptCode)
	{
		var pc = new IntCodeComputer(instructions);
		pc.RunProgram();
		pc.Outputs.Clear();

		foreach (var line in scriptCode.Split("\r\n"))
		{
			foreach (var b in Encoding.ASCII.GetBytes(line))
				pc.Inputs.Enqueue(b);
			pc.Inputs.Enqueue(10);

			pc.RunProgram();
			if (pc.ProgramStatus == ProgramStatus.Completed)
			{
				var output = Encoding.ASCII.GetString(
					pc.Outputs.Select(b => (byte)b).ToArray());
				var code = CodeRegex().Match(output).Value;
				return (pc.ProgramStatus, code);
			}
			pc.Outputs.Clear();
		}

		return (pc.ProgramStatus, string.Empty);
	}

	[GeneratedRegex("\\d+")]
	private static partial Regex CodeRegex();
}
