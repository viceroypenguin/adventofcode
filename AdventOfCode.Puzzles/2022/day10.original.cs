namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 10, CodeType.Original)]
public partial class Day_10_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var x = 1;
		var cycle = 0;
		var sum = 0;

		foreach (var l in input.Lines)
		{
			if (l == "noop")
			{
				cycle++;
				if (cycle % 40 == 20)
					sum += x * cycle;
			}
			else
			{
				cycle += 2;
				if (cycle % 40 == 20)
				{
					sum += x * cycle;
				}
				else if (cycle % 40 == 21)
				{
					sum += x * (cycle - 1);
				}

				var (amt, _) = l.AsSpan()[5..].AtoI();
				x += amt;
			}

			if (cycle > 220) break;
		}

		var screen = new[]
		{
			new char[40],
			new char[40],
			new char[40],
			new char[40],
			new char[40],
			new char[40],
		};
		var e = input.Lines.GetEnumerator();
		e.MoveNext();
		var inAdd = false;
		for (cycle = 0, x = 1; cycle < 240; cycle++)
		{
			var c = Math.Abs((cycle % 40) - x) <= 1 ? '█' : ' ';
			screen[cycle / 40][cycle % 40] = c;

			if (e.Current.ToString() == "noop")
			{
				e.MoveNext();
			}
			else if (!inAdd)
			{
				inAdd = true;
			}
			else
			{
				inAdd = false;
				x += int.Parse(e.Current.ToString()![5..]);
				e.MoveNext();
			}
		}

		var part2 = string.Join(Environment.NewLine,
			screen.Select(l => string.Join("", l)));
		return (sum.ToString(), part2);
	}
}
