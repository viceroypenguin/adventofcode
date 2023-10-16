using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 2, CodeType.Original)]
public partial class Day_02_Original : IPuzzle
{
	[GeneratedRegex("(?<dir>forward|down|up) (?<n>\\d+)")]
	private static partial Regex InstructionRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		// `(?<xxx>) defines a group named xxx
		// (x|y|z) = match one of x, y, or z
		// \d+ = digits
		var regex = InstructionRegex();
		var (depth, hor, aim) = input.Lines
			// use regex to collect the direction and number
			.Select(l => regex.Match(l))
			// extract direction and integer conversion
			.Select(m => (
				i: m.Groups["dir"].Value,
				n: Convert.ToInt32(m.Groups["n"].Value)))
			// start at (0, 0, 0), and update for each instruction
			.Aggregate((depth: 0L, hor: 0L, aim: 0L), (p, m) =>
				m.i switch
				{
					// aim remains the same, forward movement based on n
					// depth based on aim
					"forward" => (p.depth + (p.aim * m.n), p.hor + m.n, p.aim),
					// depth and forward remain the same, adjust aim
					"down" => (p.depth, p.hor, p.aim + m.n),
					"up" => (p.depth, p.hor, p.aim - m.n),
					_ => throw new UnreachableException(),
				});

		// aim == parta depth
		return (
			(hor * aim).ToString(),
			(hor * depth).ToString());
	}
}
