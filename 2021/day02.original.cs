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

		var regex = new Regex(@"(?<dir>forward|down|up) (?<n>\d+)");
		var (depth, hor) = input.GetLines()
			.Select(l => regex.Match(l))
			.Select(m => (i: m.Groups["dir"].Value, n: Convert.ToInt32(m.Groups["n"].Value)))
			.Aggregate((depth: 0L, hor: 0L), (p, m) =>
				m.i switch
				{
					"forward" => (p.depth, p.hor + m.n),
					"down" => (p.depth + m.n, p.hor),
					"up" => (p.depth - m.n, p.hor),
				});

		PartA = (hor * depth).ToString();

		(depth, hor, var aim) = input.GetLines()
			.Select(l => regex.Match(l))
			.Select(m => (i: m.Groups["dir"].Value, n: Convert.ToInt32(m.Groups["n"].Value)))
			.Aggregate((depth: 0L, hor: 0L, aim: 0L), (p, m) =>
				m.i switch
				{
					"forward" => (p.depth + (p.aim * m.n), p.hor + m.n, p.aim),
					"down" => (p.depth, p.hor, p.aim + m.n),
					"up" => (p.depth, p.hor, p.aim - m.n),
				});

		PartB = (hor * depth).ToString();
	}
}
