namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 16, CodeType.Original)]
public class Day_16_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		return (
			ExecutePart(input.Lines[0], 272),
			ExecutePart(input.Lines[0], 35651584));
	}

	private static string ExecutePart(string input, int length)
	{
		var data = input.Select(c => c == '1').ToList();

		static List<bool> CurveStep(IList<bool> bits) =>
			bits.Concat(new[] { false }).Concat(bits.Reverse().Select(b => !b)).ToList();

		while (data.Count < length)
			data = CurveStep(data);

		data = data.Take(length).ToList();

		data = GenerateChecksum(data);
		return string.Join("", data.Take(length).Select(b => b ? '1' : '0'));
	}

	private static List<bool> GenerateChecksum(List<bool> bits)
	{
		var checksum = new List<bool>();
		for (var i = 0; i < bits.Count; i += 2)
			checksum.Add(!(bits[i] ^ bits[i + 1]));

		return (checksum.Count % 2) == 0
			? GenerateChecksum(checksum)
			: checksum;
	}
}
