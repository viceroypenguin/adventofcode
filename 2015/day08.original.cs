namespace AdventOfCode;

public class Day_2015_08_Original : Day
{
	public override int Year => 2015;
	public override int DayNumber => 8;
	public override CodeType CodeType => CodeType.Original;

	Regex decodeRegex = new Regex(@"""(?<char>\\x.{2}|\\\\|\\\""|\w)*""", RegexOptions.Compiled);

	int GetDecodedLength(string str)
	{
		return decodeRegex
			.Match(str)
			.Groups["char"]
			.Captures
			.Count;
	}

	protected override void ExecuteDay(byte[] input)
	{
		var lines = input.GetLines();
		var inputLength = lines.Select(s => s.Length).Sum();

		var decodedLength = lines.Select(GetDecodedLength).Sum();
		Dump('A', inputLength - decodedLength);

		var encodedVariance = lines
			.Select(s =>
				s.Where(c => c == '\\' || c == '\"').Count() + 2)
			.Sum();
		Dump('B', encodedVariance);
	}
}
