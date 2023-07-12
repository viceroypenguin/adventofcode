using System.Runtime.CompilerServices;

namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 10, CodeType.Fastest)]
public class Day_10_Fastest : IPuzzle
{
	[SkipLocalsInit]
	public (string, string) Solve(PuzzleInput input)
	{
		// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day10.c
		var span = input.GetSpan();

		Span<byte> bytes = stackalloc byte[256];
		for (var i = 0; i < 256; i++)
			bytes[i] = (byte)i;

		var n = 0;
		byte position = 0, skip = 0;
		foreach (var c in span)
		{
			if (c >= '0')
			{
				n = (n * 10) + c - '0';
			}
			else if (c is (byte)',' or (byte)'\n')
			{
				(position, skip) = DoRound(bytes, (byte)n, position, skip);
				n = 0;
			}
		}
		var partA = bytes[0] * bytes[1];

		var newLength = span.Length - 1 + 5;
		Span<byte> seq = stackalloc byte[newLength];
		span.CopyTo(seq);
		seq[^5] = 17;
		seq[^4] = 31;
		seq[^3] = 73;
		seq[^2] = 47;
		seq[^1] = 23;

		for (var i = 0; i < 256; i++)
			bytes[i] = (byte)i;

		position = skip = 0;
		for (var i = 0; i < 64; i++)
		{
			for (var j = 0; j < newLength; j++)
				(position, skip) = DoRound(bytes, seq[j], position, skip);
		}

		var str = new char[32];
		for (var i = 0; i < 256; i += 16)
		{
			var b = bytes[i];
			for (var j = i + 1; j < i + 16; j++)
				b ^= bytes[j];

			str[i >> 3] = ToHex(b >> 4);
			str[(i >> 3) + 1] = ToHex(b & 0xf);
		}
		var partB = new string(str);

		return (partA.ToString(), partB);
	}

	private static (byte position, byte skip) DoRound(Span<byte> bytes, byte length, byte position, byte skip)
	{
		// start in the middle
		byte i = (byte)(position + (length >> 1)),
			// do we need to add one or not
			j = (byte)(i - (~length & 1));

		// twist the rope
		while (i != position)
		{
			i--; j++;
			(bytes[j], bytes[i]) = (bytes[i], bytes[j]);
		}
		skip++; position = (byte)(j + skip);
		return (position, skip);
	}

	private static char ToHex(int val) =>
		val >= 10
			? (char)(val - 10 + 'a')
			: (char)(val + '0');
}
