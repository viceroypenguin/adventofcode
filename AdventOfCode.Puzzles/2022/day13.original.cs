using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 13, CodeType.Original)]
public partial class Day_13_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var packets = input.Lines
			.Where(x => !string.IsNullOrEmpty(x))
			.Select(l => JsonSerializer.Deserialize<JsonNode>(l))
			.ToList();

		var part1 = packets
			.Batch(2)
			.Index()
			.Where(x => CompareItem(x.Item[0], x.Item[1]) < 0)
			.Select(x => x.Index + 1)
			.Sum()
			.ToString();

		var part2 = new[] { (JsonValue)2, (JsonValue)6, }
			.Select((s, i) => packets.Count(p => CompareItem(p, s) < 0) + 1 + i)
			.Aggregate(1, (a, b) => a * b)
			.ToString();

		return (part1, part2);
	}

	private int CompareItem(JsonNode? left, JsonNode? right) =>
		(left, right) switch
		{
			(JsonNode l, null) => +1,
			(null, JsonNode r) => -1,
			(JsonValue l, JsonValue r) =>
				Comparer<int>.Default.Compare(l.GetValue<int>(), r.GetValue<int>()),
			(JsonValue l, JsonArray r) =>
				CompareItem(new JsonArray(l.GetValue<int>()), r),
			(JsonArray l, JsonValue r) =>
				CompareItem(l, new JsonArray(r.GetValue<int>())),
			(JsonArray l, JsonArray r) =>
				l.ZipLongest(r, CompareItem)
					.SkipWhile(x => x == 0)
					.FirstOrDefault(),
			_ => throw new UnreachableException(),
		};
}

