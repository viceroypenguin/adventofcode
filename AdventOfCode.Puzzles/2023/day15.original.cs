namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 15, CodeType.Original)]
public partial class Day_15_Original : IPuzzle
{
	private sealed record Lens
	{
		public required string Name { get; set; }
		public int Length { get; set; }
	}

	private static readonly char[] separator = ['-', '='];

	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input.Lines[0]
			.Split(',')
			.ToList();

		var part1 = instructions
			.Select(x => x.Aggregate(0, Hash))
			.Sum();

		var boxes = new Dictionary<int, List<Lens>>();
		foreach (var i in instructions)
		{
			var label = i.Split(separator)[0];
			var hash = label.Aggregate(0, Hash);
			var list = boxes.GetOrAdd(hash, _ => new());

			if (i[^1] == '-')
			{
				_ = list.RemoveAll(x => x.Name == label);
			}
			else
			{
				var length = int.Parse(i.Split('=')[1]);
				var lens = list.Find(x => x.Name == label);
				if (lens != null)
					lens!.Length = length;
				else
					list.Add(new() { Name = label, Length = length, });
			}
		}

		var part2 = boxes
			.Select(kvp => kvp.Value
				.Index()
				.Select(x => (long)(x.index + 1) * x.item.Length)
				.Sum() * (kvp.Key + 1))
			.Sum();

		return (part1.ToString(), part2.ToString());
	}

	private static int Hash(int a, char b)
	{
		return ((a + (byte)b) * 17) % 256;
	}
}
