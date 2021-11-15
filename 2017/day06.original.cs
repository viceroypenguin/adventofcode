namespace AdventOfCode;

public class Day_2017_06_Original : Day
{
	public override int Year => 2017;
	public override int DayNumber => 6;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var nums = input.GetString()
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
			for (int i = 1; i <= extraInc; i++)
				nums[(minElement.i + i) % nums.Count] += allInc + 1;
			for (int i = extraInc + 1; i <= nums.Count; i++)
				nums[(minElement.i + i) % nums.Count] += allInc;

			var oldInput = history
				.Select((old, idx) => new { idx, isMatch = old.Zip(nums, (o, i) => o == i).All(b => b), })
				.Where(x => x.isMatch)
				.FirstOrDefault();
			if (oldInput != null)
			{
				Dump('A', history.Count);
				Dump('B', history.Count - oldInput.idx);
				break;
			}

			history.Add(nums.ToArray());
		}
	}
}
