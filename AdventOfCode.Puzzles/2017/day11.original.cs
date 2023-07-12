namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 11, CodeType.Original)]
public class Day_11_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var dirs = input.Text
			.Trim()
			.Split(',')
			.ToList();

		static (int nw, int n, int ne) Move((int nw, int n, int ne) old, string dir)
		{
			switch (dir)
			{
				case "n":
					if (old.n < 0)
						return (old.nw, old.n + 1, old.ne);
					if (old.ne < 0)
						return (old.nw + 1, old.n, old.ne + 1);
					if (old.nw < 0)
						return (old.nw + 1, old.n, old.ne + 1);
					return (old.nw, old.n + 1, old.ne);

				case "s":
					if (old.n > 0)
						return (old.nw, old.n - 1, old.ne);
					if (old.ne > 0)
						return (old.nw - 1, old.n, old.ne - 1);
					if (old.nw > 0)
						return (old.nw - 1, old.n, old.ne - 1);
					return (old.nw, old.n - 1, old.ne);

				case "ne":
					if (old.ne < 0)
						return (old.nw, old.n, old.ne + 1);
					if (old.nw > 0)
						return (old.nw - 1, old.n + 1, old.ne);
					if (old.n < 0)
						return (old.nw - 1, old.n + 1, old.ne);
					return (old.nw, old.n, old.ne + 1);

				case "nw":
					if (old.nw < 0)
						return (old.nw + 1, old.n, old.ne);
					if (old.ne > 0)
						return (old.nw, old.n + 1, old.ne - 1);
					if (old.n < 0)
						return (old.nw, old.n + 1, old.ne - 1);
					return (old.nw + 1, old.n, old.ne);

				case "sw":
					if (old.ne > 0)
						return (old.nw, old.n, old.ne - 1);
					if (old.nw < 0)
						return (old.nw + 1, old.n - 1, old.ne);
					if (old.n > 0)
						return (old.nw + 1, old.n - 1, old.ne);
					return (old.nw, old.n, old.ne - 1);

				case "se":
					if (old.nw > 0)
						return (old.nw - 1, old.n, old.ne);
					if (old.ne < 0)
						return (old.nw, old.n - 1, old.ne + 1);
					if (old.n > 0)
						return (old.nw, old.n - 1, old.ne + 1);
					return (old.nw - 1, old.n, old.ne);

				default:
					throw new InvalidOperationException($"Unknown direction: {dir}.");
			}
		}

		var positions = dirs
			.Scan((nw: 0, n: 0, ne: 0), Move)
			.Select(x => Math.Abs(x.nw) + Math.Abs(x.n) + Math.Abs(x.ne))
			.ToList();

		var partA = positions[^1];
		var partB = positions.Max();

		return (partA.ToString(), partB.ToString());
	}
}
