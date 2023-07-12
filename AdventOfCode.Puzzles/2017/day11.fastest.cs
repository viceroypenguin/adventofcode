using System.Diagnostics;

namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 11, CodeType.Fastest)]
public class Day_11_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		int dir = 0, nw = 0, n = 0, ne = 0, max = 0;
		foreach (var c in input.GetSpan())
		{
			if (c >= 'a')
			{
				dir = (dir << 8) + c;
			}
			else
			{
				switch (dir)
				{
					case 0x6e: // n
						if (n < 0) n++;
						else if (nw < 0 || ne < 0) { ne++; nw++; }
						else n++;
						break;

					case 0x73: // s
						if (n > 0) n--;
						else if (nw > 0 || ne > 0) { nw--; ne--; }
						else n--;
						break;

					case 0x6e65: //ne
						if (ne < 0) ne++;
						else if (n < 0 || nw > 0) { nw--; n++; }
						else ne++;
						break;

					case 0x6e77: //nw
						if (nw < 0) nw++;
						else if (n < 0 || ne > 0) { ne--; n++; }
						else nw++;
						break;

					case 0x7377: //sw
						if (ne > 0) ne--;
						else if (n > 0 || nw < 0) { nw++; n--; }
						else ne--;
						break;

					case 0x7365: //se
						if (nw > 0) nw--;
						else if (n > 0 || ne < 0) { ne++; n--; }
						else nw--;
						break;

					default:
						throw new UnreachableException();
				}
				max = Math.Max(max, Math.Abs(nw) + Math.Abs(n) + Math.Abs(ne));
				dir = 0;
			}
		}

		var partA = Math.Abs(nw) + Math.Abs(n) + Math.Abs(ne);
		return (partA.ToString(), max.ToString());
	}
}
