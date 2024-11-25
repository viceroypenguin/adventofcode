using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 24, CodeType.Original)]
public class Day_24_Original : IPuzzle
{
	[StructLayout(LayoutKind.Auto)]
	private struct Component
	{
		public int PortA { get; set; }
		public int PortB { get; set; }
	}

	public (string, string) Solve(PuzzleInput input)
	{
		var ports = input.Lines
			.Select(x => x.Split('/'))
			.Select(x => new Component { PortA = Convert.ToInt32(x[0]), PortB = Convert.ToInt32(x[1]), })
			.ToList();

		var map =
			ports.Select(x => new { i = x.PortA, x, })
				.Concat(ports
					.Where(x => x.PortA != x.PortB)
					.Select(x => new { i = x.PortB, x, }))
				.ToLookup(
					x => x.i,
					x => x.x);

		var (maxStrength, _, maxLongestPath) = CalculateStrength(map, [], 0, (0, 0, 0));

		return (
			maxStrength.ToString(),
			maxLongestPath.ToString());
	}

	private static (int maxStrength, int longestPath, int maxLongestPath)
		CalculateStrength(ILookup<int, Component> map, ImmutableList<Component> path, int openConnection, (int maxStrength, int longestPath, int maxLongestPath) x)
	{
		var list = map[openConnection]
			.Where(c => !path.Contains(c))
			.Select(c => CalculateStrength(
				map,
				path.Add(c),
				openConnection == c.PortA ? c.PortB : c.PortA,
				(maxStrength: x.maxStrength + c.PortA + c.PortB, x.longestPath, x.maxLongestPath)))
			.ToList();

#pragma warning disable IDE0046 // Convert to conditional expression
		if (list.Count != 0)
		{
			return (
				list.Max(y => y.maxStrength),
				list.Max(y => y.longestPath),
				list.OrderByDescending(y => y.longestPath)
					.ThenByDescending(y => y.maxLongestPath)
					.First()
					.maxLongestPath);
		}

		return (x.maxStrength, path.Count, x.maxStrength);
#pragma warning restore IDE0046 // Convert to conditional expression
	}
}
