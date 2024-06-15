namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 06, CodeType.Original)]
public class Day_06_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var nums = input.Text
			.Split()
			.Where(x => !string.IsNullOrWhiteSpace(x))
			.Select(x => Convert.ToInt32(x))
			.ToList();

		var history = new List<int[]>
		{
			nums.ToArray(),
		};

		while (true)
		{
			var minElement =
				nums.Select((x, i) => new { x, i, })
					.OrderByDescending(x => x.x)
					.ThenBy(x => x.i)
					.First();

			var allInc = minElement.x / nums.Count;
			var extraInc = minElement.x % nums.Count;

			nums[minElement.i] = 0;
			for (var i = 1; i <= extraInc; i++)
				nums[(minElement.i + i) % nums.Count] += allInc + 1;
			for (var i = extraInc + 1; i <= nums.Count; i++)
				nums[(minElement.i + i) % nums.Count] += allInc;

			var oldInput = history
				.Select((old, idx) => new { idx, isMatch = old.Zip(nums, (o, i) => o == i).All(b => b), })
				.FirstOrDefault(x => x.isMatch);

			if (oldInput != null)
			{
				return (
					history.Count.ToString(),
					(history.Count - oldInput.idx).ToString()
				);
			}

			history.Add(nums.ToArray());
		}
	}
}
