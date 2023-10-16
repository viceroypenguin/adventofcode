namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 11, CodeType.Original)]
public partial class Day_11_Original : IPuzzle
{
	private static readonly Func<long, long>[] Operations =
	[
		i => i * 3,
		i => i + 8,
		i => i + 2,
		i => i + 4,
		i => i * 19,
		i => i + 5,
		i => i * i,
		i => i + 1,
	];

	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var monkeys = input.Lines.Split(string.Empty)
			.Select(m =>
			{
				var id = int.Parse(m.First()[7..].Replace(":", ""));
				var items = m.ElementAt(1)[18..]
					.Split(", ")
					.Select(long.Parse)
					.ToList();
				var operation = Operations[id];
				var test = int.Parse(m.ElementAt(3)[21..]);
				var ifTrue = int.Parse(m.ElementAt(4)[29..]);
				var ifFalse = int.Parse(m.ElementAt(5)[30..]);

				return new Monkey
				{
					Id = id,
					Items = items,
					Operation = operation,
					Test = test,
					IfTrue = ifTrue,
					IfFalse = ifFalse,
				};
			})
			.ToList();

		for (var i = 0; i < 20; i++)
		{
			foreach (var m in monkeys)
			{
				foreach (var item in m.Items)
				{
					var v = m.Operation(item) / 3;
					var next = v % m.Test == 0 ? m.IfTrue : m.IfFalse;
					monkeys[next].Items.Add(v);
				}

				m.Activity += m.Items.Count;
				m.Items.Clear();
			}
		}

		var top2 = monkeys
			.Select(m => m.Activity)
			.PartialSort(2, OrderByDirection.Descending)
			.ToList();
		var part1 = (top2[0] * top2[1]).ToString();

		monkeys = input.Lines.Split(string.Empty)
			.Select(m =>
			{
				var id = int.Parse(m.First()[7..].Replace(":", ""));
				var items = m.ElementAt(1)[18..]
					.Split(", ")
					.Select(long.Parse)
					.ToList();
				var operation = Operations[id];
				var test = int.Parse(m.ElementAt(3)[21..]);
				var ifTrue = int.Parse(m.ElementAt(4)[29..]);
				var ifFalse = int.Parse(m.ElementAt(5)[30..]);

				return new Monkey
				{
					Id = id,
					Items = items,
					Operation = operation,
					Test = test,
					IfTrue = ifTrue,
					IfFalse = ifFalse,
				};
			})
			.ToList();

		var factor = monkeys.Aggregate(1L, (f, m) => f * m.Test);
		for (var i = 0; i < 10_000; i++)
		{
			foreach (var m in monkeys)
			{
				foreach (var item in m.Items)
				{
					var v = m.Operation(item) % factor;
					var next = v % m.Test == 0 ? m.IfTrue : m.IfFalse;
					monkeys[next].Items.Add(v);
				}

				m.Activity += m.Items.Count;
				m.Items.Clear();
			}
		}

		top2 = monkeys
			.Select(m => m.Activity)
			.PartialSort(2, OrderByDirection.Descending)
			.ToList();
		var part2 = (top2[0] * top2[1]).ToString();

		return (part1, part2);
	}

	private sealed class Monkey
	{
		public required int Id { get; init; }
		public required List<long> Items { get; init; }
		public required Func<long, long> Operation { get; init; }
		public required int Test { get; init; }
		public required int IfTrue { get; init; }
		public required int IfFalse { get; init; }
		public long Activity { get; set; }
	}
}
