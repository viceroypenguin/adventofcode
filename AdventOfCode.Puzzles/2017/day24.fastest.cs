namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 24, CodeType.Fastest)]
public class Day_24_Fastest : IPuzzle
{
	private sealed class Component
	{
		public int PortA { get; set; }
		public int PortB { get; set; }
		public bool Used { get; set; }
	}

	private List<Component>[] lookupByA;
	private List<Component>[] lookupByB;

	private int maxStrength = -1;
	private int longestPath = -1;
	private int maxLongestPath = -1;

	public (string, string) Solve(PuzzleInput input)
	{
		// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day24.c
		var span = input.Span;

		var ports = new List<Component>(span.Length / 4);
		{
			int a = 0, n = 0;
			foreach (var c in span)
			{
				if (c == '/')
				{
					a = n;
					n = 0;
				}
				else if (c == '\n')
				{
					ports.Add(new Component { PortA = a, PortB = n, });
					n = 0;
				}
				else if (c >= '0')
				{
					n = (n * 10) + c - '0';
				}
			}
		}

		var maxPort = -1;
		for (var i = 0; i < ports.Count; i++)
		{
			var p = ports[i];
			if (p.PortA > maxPort)
				maxPort = p.PortA;
			if (p.PortB > maxPort)
				maxPort = p.PortB;
		}

		var byA = new List<Component>[maxPort + 1];
		var byB = new List<Component>[maxPort + 1];
		for (var i = 0; i < ports.Count; i++)
		{
			var p = ports[i];
			var arr = byA[p.PortA] = byA[p.PortA] ?? new List<Component>();
			arr.Add(p);

			if (p.PortA != p.PortB)
			{
				arr = byB[p.PortB] = byB[p.PortB] ?? new List<Component>();
				arr.Add(p);
			}
		}
		lookupByA = byA;
		lookupByB = byB;

		CalculateStrength(0, 0, 0);

		return (
			maxStrength.ToString(),
			maxLongestPath.ToString());
	}

	private void CalculateStrength(int port, int curStrength, int curLength)
	{
		if (curStrength > maxStrength)
		{
			maxStrength = curStrength;
		}

		if (curLength > longestPath)
		{
			maxLongestPath = curStrength;
			longestPath = curLength;
		}
		else if (curLength == longestPath && curStrength > maxLongestPath)
		{
			maxLongestPath = curStrength;
		}

		curLength++;
		curStrength += port;

		var arr = lookupByA[port];
		for (int i = 0; i < arr?.Count; i++)
		{
			var p = arr[i];
			if (!p.Used)
			{
				p.Used = true;
				CalculateStrength(p.PortB, curStrength + p.PortB, curLength);
				p.Used = false;
			}
		}

		arr = lookupByB[port];
		for (int i = 0; i < arr?.Count; i++)
		{
			var p = arr[i];
			if (!p.Used)
			{
				p.Used = true;
				CalculateStrength(p.PortA, curStrength + p.PortA, curLength);
				p.Used = false;
			}
		}
	}
}
