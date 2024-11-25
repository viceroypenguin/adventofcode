using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 06, CodeType.Original)]
public partial class Day_06_Original : IPuzzle
{
	private enum Command
	{
		TurnOn,
		TurnOff,
		Toggle,
	}

	[StructLayout(LayoutKind.Auto)]
	private struct Action
	{
		public Command Command { get; set; }
		public int StartX { get; set; }
		public int StartY { get; set; }
		public int EndX { get; set; }
		public int EndY { get; set; }
	}

	[GeneratedRegex("((?<on>turn on)|(?<off>turn off)|(?<toggle>toggle)) (?<startX>\\d+),(?<startY>\\d+) through (?<endX>\\d+),(?<endY>\\d+)", RegexOptions.ExplicitCapture)]
	private static partial Regex InstructionRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = InstructionRegex();

		var actions = input.Lines
			.Select(l => regex.Match(l))
			.Select(m => new Action
			{
				Command =
					m.Groups["on"].Success ? Command.TurnOn :
					m.Groups["off"].Success ? Command.TurnOff :
					m.Groups["toggle"].Success ? Command.Toggle :
					throw new InvalidOperationException(),

				StartX = int.Parse(m.Groups["startX"].Value),
				StartY = int.Parse(m.Groups["startY"].Value),
				EndX = int.Parse(m.Groups["endX"].Value),
				EndY = int.Parse(m.Groups["endY"].Value),
			})
			.ToList();

		int ProcessActions(Func<Command, Func<int, int>> getLightProcessor)
		{
			var lights = new int[1000, 1000];

			foreach (var a in actions)
			{
				var lightProcessor = getLightProcessor(a.Command);

				for (var x = a.StartX; x <= a.EndX; x++)
				{
					for (var y = a.StartY; y <= a.EndY; y++)
						lights[x, y] = lightProcessor(lights[x, y]);
				}
			}

			var cnt = 0;
			for (var x = 0; x < 1000; x++)
			{
				for (var y = 0; y < 1000; y++)
					cnt += lights[x, y];
			}

			return cnt;
		}

		var partA = ProcessActions(c =>
			c switch
			{
				Command.TurnOn => i => 1,
				Command.TurnOff => i => 0,
				Command.Toggle => i => 1 - i,
				_ => throw new UnreachableException(),
			});
		var partB = ProcessActions(c =>
			c switch
			{
				Command.TurnOn => i => i + 1,
				Command.TurnOff => i => Math.Max(i - 1, 0),
				Command.Toggle => i => i + 2,
				_ => throw new UnreachableException(),
			});

		return (partA.ToString(), partB.ToString());
	}
}
