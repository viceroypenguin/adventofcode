namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 03, CodeType.Original)]
public partial class Day_03_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = input.Lines
			.Select(l =>
			{
				var max = l[..^1].Max();
				var firstIndexOf = l.IndexOf(max);
				var secondMax = l[(firstIndexOf + 1)..].Max();
				var num = ((max - '0') * 10) + (secondMax - '0');
				return num;
			})
			.Sum();

		var part2 = input.Lines
			.Select(l =>
			{
				var arr = l.ToList();
				while (arr.Count > 12)
				{
					var flag = false;
					for (var i = 0; !flag && i < arr.Count - 1; i++)
					{
						if (arr[i] < arr[i + 1])
						{
							flag = true;
							arr.RemoveAt(i);
						}
					}

					if (!flag)
						arr.Remove(arr.Min());
				}

				return long.Parse(string.Join("", arr));
			})
			.Sum();

		return (part1.ToString(), part2.ToString());
	}
}
