using System.Collections;

namespace AdventOfCode;

public class Day_2021_02_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 2;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		// `(?<xxx>) defines a group named xxx
		// (x|y|z) = match one of x, y, or z
		// \d+ = digits
		var regex = new Regex(@"(?<dir>forward|down|up) (?<n>\d+)");
		var (depth, hor, aim) = input.GetLines()
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
				});

		// aim == parta depth
		PartA = (hor * aim).ToString();
		PartB = (hor * depth).ToString();
	}
}
