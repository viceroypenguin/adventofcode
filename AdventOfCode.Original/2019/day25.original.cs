using System.Numerics;
using System.Threading.Tasks.Dataflow;

namespace AdventOfCode;

public class Day_2019_25_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 25;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var instructions = input.GetString()
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

		for (int i = 0; i < 256; i++)
		{
			var s = script;
			foreach (var (item, value) in items)
			{
				if ((i & value) != 0)
					s = s + Environment.NewLine + "take " + item;
			}

			s = s + Environment.NewLine + "south";
			var status = DoPart(instructions, s);
			if (status == ProgramStatus.Completed)
				return;
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

	private static ProgramStatus DoPart(long[] instructions, string scriptCode)
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
				Console.WriteLine(Encoding.ASCII.GetString(
					pc.Outputs.Select(b => (byte)b).ToArray()));
			pc.Outputs.Clear();
		}

		return pc.ProgramStatus;
	}
}
