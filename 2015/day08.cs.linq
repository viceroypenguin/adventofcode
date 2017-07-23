<Query Kind="Program" />

Regex decodeRegex = new Regex(@"""(?<char>\\x.{2}|\\\\|\\\""|\w)*""", RegexOptions.Compiled);

int GetDecodedLength(string str)
{
	return decodeRegex
		.Match(str)
		.Groups["char"]
		.Captures
		.Count;
}

void Main()
{
	var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day08.input.txt"));
	var inputLength = input.Select(s => s.Length).Sum();

	var decodedLength = input.Select(GetDecodedLength).Sum();
	(inputLength - decodedLength).Dump("Part A");
	
	var encodedVariance = input
		.Select(s =>
			s.Where(c => c == '\\' || c == '\"').Count() + 2)
		.Sum();
	encodedVariance.Dump("Part B");
}

// Define other methods and classes here