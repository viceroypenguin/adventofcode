namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 09, CodeType.Original)]
public partial class Day_09_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var part1 = DoPart1(input);

		var part2 = DoPart2(input);

		return (part1, part2);
	}

	private static Span<ushort> LoadInput(PuzzleInput input)
	{
		Span<ushort> disk = new ushort[20_000 * 9];
		var flag = true;
		ushort id = 0;
		var index = 0;

		foreach (var ch in input.Span)
		{
			if (flag)
			{
				for (var i = 0; i < (ch - '0'); i++)
					disk[index++] = id;
				id++;
			}
			else
			{
				for (var i = 0; i < (ch - '0'); i++)
					disk[index++] = ushort.MaxValue;
			}
			flag ^= true;
		}

		disk = disk[..index];
		return disk;
	}

	private static string DoPart1(PuzzleInput input)
	{
		var disk = LoadInput(input);

		var index = 0;
		for (var i = disk.Length - 1; i >= index; i--)
		{
			if (disk[i] == ushort.MaxValue)
				continue;

			while (index < disk.Length && disk[index] != ushort.MaxValue)
				index++;

			if (index >= disk.Length)
				break;

			disk[index] = disk[i];
			disk = disk[..i];
		}

		var sum = 0L;
		for (var i = 0; i < disk.Length; i++)
			sum += i * disk[i];

		return sum.ToString();
	}

	private static string DoPart2(PuzzleInput input)
	{
		var blocks = input.Bytes[..^1]
			.Chunk(2)
			.Select(x => x.Pad(2))
			.SelectMany((c, i) => new[]
			{
				(id: i, count: c.First() - '0'),
				(id: -1, count: c.Last() - '0'),
			})
			.SkipLast(1)
			.ToList();

		for (var j = blocks.Count - 1; j >= 0; j--)
		{
			if (blocks[j].id == -1)
				continue;

			for (var i = 0; i < j; i++)
			{
				if (blocks[i].id != -1)
					continue;

				if (blocks[j].count > blocks[i].count)
					continue;

				var cnt = blocks[i].count;
				cnt -= blocks[j].count;
				blocks[i] = blocks[j];
				blocks[j] = (-1, blocks[j].count);

				if (j + 1 < blocks.Count && blocks[j + 1].id == -1)
				{
					blocks[j] = (-1, blocks[j].count + blocks[j + 1].count);
					blocks.RemoveAt(j + 1);
				}

				if (blocks[j - 1].id == -1)
				{
					blocks[j - 1] = (-1, blocks[j - 1].count + blocks[j].count);
					blocks.RemoveAt(j);
				}

				if (cnt > 0)
					blocks.Insert(i + 1, (-1, cnt));

				i = j + 1;
			}
		}

		var index = 0;
		var sum = 0L;
		foreach (var (id, count) in blocks)
		{
			for (var i = 0; i < count; i++)
			{
				if (id > 0)
					sum += id * index;
				index++;
			}
		}

		return sum.ToString();
	}
}
