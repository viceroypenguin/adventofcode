using System.Collections;

namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 17, CodeType.Original)]
public class Day_17_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var total = 150;

		var containers = input.Lines
			.Select(s => Convert.ToInt32(s))
			.ToList();

		var cnt = 0;
		var max = 1 << containers.Count;
		var numCombinations = 0;

		var minPop = int.MaxValue;
		var haveMinPop = 0;
		while (cnt < max)
		{
			var sum = 0;
			var bitstream = new BitArray([cnt]);
			foreach (var _ in bitstream.OfType<bool>().Select((b, i) => new { b, i }))
			{
				if (_.b)
					sum += containers[_.i];
			}

			if (sum == total)
			{
				numCombinations++;

				var pop = bitstream.OfType<bool>().Count(b => b);
				if (pop < minPop)
				{
					minPop = pop;
					haveMinPop = 1;
				}
				else if (pop == minPop)
				{
					haveMinPop++;
				}
			}

			cnt++;
		}

		return (
			numCombinations.ToString(),
			haveMinPop.ToString());
	}
}
