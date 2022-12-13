using System.Text.Json;

namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 13, CodeType.Original)]
public partial class Day_13_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var packets = input.Lines
			.Where(x => x != string.Empty)
			.Select(Parse)
			.ToList();

		var part1 = packets.Batch(2)
			.Index()
			.Where(x => CompareItem(x.item[0], x.item[1]) < 0)
			.Select(x => x.index + 1)
			.Sum()
			.ToString();

		var p1 = Parse("[[2]]");
		var p2 = Parse("[[6]]");

		var ordered = packets
			.Append(p1)
			.Append(p2)
			.Order(Comparer<Packet>.Create(CompareItem))
			.ToList();

		var part2 = ((ordered.IndexOf(p1) + 1) * (ordered.IndexOf(p2) + 1)).ToString();

		return (part1, part2);
	}

	private abstract record Packet;

	private record IntPacket(int Value) : Packet;
	private record ListPacket(List<Packet> Packets) : Packet;

	private static Packet Parse(string value) =>
		Parse(JsonSerializer.Deserialize<JsonElement>(value));

	private static Packet Parse(JsonElement element) =>
		element.ValueKind == JsonValueKind.Number
			? new IntPacket(element.GetInt32())
			: new ListPacket(element.EnumerateArray()
				.Select(Parse)
				.ToList());

	private int CompareItem(Packet left, Packet right) =>
		(left, right) switch
		{
			(Packet l, null) => +1,
			(null, Packet r) => -1,
			(IntPacket l, IntPacket r) =>
				Comparer<int>.Default.Compare(l.Value, r.Value),
			(IntPacket l, ListPacket r) =>
				CompareItem(new ListPacket(new() { l }), r),
			(ListPacket l, IntPacket r) =>
				CompareItem(l, new ListPacket(new() { r })),
			(ListPacket l, ListPacket r) =>
				l.Packets.ZipLongest(r.Packets, CompareItem)
					.SkipWhile(x => x == 0)
					.FirstOrDefault(),
		};
}

