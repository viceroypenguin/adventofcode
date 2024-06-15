namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 23, CodeType.Original)]
public class Day_23_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var bytes = input.Bytes;

		var list = new int[10];
		list[bytes[8] - 0x30] = bytes[0] - 0x30;
		for (var i = 0; i < 8; i++)
			list[bytes[i] - 0x30] = bytes[i + 1] - 0x30;

		var idx = bytes[0] - 0x30;
		for (var i = 0; i < 100; i++)
			idx = Step(list, idx, 9);

		Span<char> output = stackalloc char[8];
		var ptr = output;
		idx = list[1];
		do
		{
			ptr[0] = (char)(idx + 0x30);
			ptr = ptr[1..];
			idx = list[idx];
		} while (idx != 1);

		var part1 = new string(output);

		// so list[1_000_000] is valid
		list = new int[1_000_001];
		for (var i = 0; i < 8; i++)
			list[bytes[i] - 0x30] = bytes[i + 1] - 0x30;
		list[bytes[8] - 0x30] = 10;

		for (var i = 10; i < 1_000_000; i++)
			list[i] = i + 1;
		list[1_000_000] = bytes[0] - 0x30;

		idx = bytes[0] - 0x30;
		for (var i = 0; i < 10_000_000; i++)
			idx = Step(list, idx, 1_000_000);

		var v1 = (ulong)list[1];
		var v2 = (ulong)list[v1];
		var part2 = (v1 * v2).ToString();

		return (part1, part2);
	}

	private static int Step(int[] list, int idx, int maxValue)
	{
		static int DecrementValue(int value, int maxValue) =>
			value == 1 ? maxValue : value - 1;

		// track next three values
		var v1 = list[idx];
		var v2 = list[v1];
		var v3 = list[v2];

		// link-ptr to after v3
		list[idx] = list[v3];

		// find target location
		var dest = DecrementValue(idx, maxValue);
		while (v1 == dest || v2 == dest || v3 == dest)
			dest = DecrementValue(dest, maxValue);

		// insert three nodes into linked-list
		list[v3] = list[dest];
		list[dest] = v1;

		// return next entry in the list
		return list[idx];
	}
}
