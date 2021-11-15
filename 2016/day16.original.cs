namespace AdventOfCode;

public class Day_2016_16_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 16;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		ExecutePart(input, 272, 'A');
		ExecutePart(input, 35651584, 'B');
	}

	private void ExecutePart(byte[] input, int length, char part)
	{
		var data = input.Select(c => c == '1').ToList();

		Func<IList<bool>, List<bool>> curveStep = (bits) =>
		{
			return bits.Concat(new[] { false }).Concat(bits.Reverse().Select(b => !b)).ToList();
		};

		while (data.Count < length)
			data = curveStep(data);

		data = data.Take(length).ToList();

		Func<IList<bool>, List<bool>> generateChecksum = (_) => new List<bool>();
		generateChecksum = (bits) =>
		{
			var checksum = new List<bool>();
			for (int i = 0; i < bits.Count; i += 2)
				checksum.Add(!(bits[i] ^ bits[i + 1]));

			if ((checksum.Count % 2) == 0) return generateChecksum(checksum);
			return checksum;
		};

		data = generateChecksum(data);
		Dump(part,
			string.Join("", data.Take(length).Select(b => b ? '1' : '0')));
	}
}
