namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 08, CodeType.Original)]
public class Day_08_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var data = input.Text
			.Trim()
			.Split()
			.Select(int.Parse)
			.ToList();

		int index = 0;
		int GetMetadataValue()
		{
			var childNodes = data[index++];
			var metadataNodes = data[index++];

			return
				Enumerable.Range(0, childNodes)
					.Sum(_ => GetMetadataValue()) +
				Enumerable.Range(0, metadataNodes)
					.Sum(_ => data[index++]);
		}

		var part1 = GetMetadataValue().ToString();

		index = 0;
		int GetNodeValue()
		{
			var childNodes = data[index++];
			var metadataNodes = data[index++];

			if (childNodes == 0)
				return Enumerable.Range(0, metadataNodes)
					.Sum(_ => data[index++]);

			var nodes = Enumerable.Range(0, childNodes)
				.Select(_ => GetNodeValue())
				.ToList();

			return Enumerable.Range(0, metadataNodes)
				.Select(_ => data[index++])
				.Sum(i => i <= nodes.Count
					? nodes[i - 1]
					: 0);
		}

		var part2 = GetNodeValue().ToString();

		return (part1, part2);
	}
}
