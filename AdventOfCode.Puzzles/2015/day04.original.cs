using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 04, CodeType.Original)]
public class Day_04_Original : IPuzzle
{
	private static bool HasLeadingZeros(int numZeros, byte[] bytes)
	{
		for (var i = 0; i < numZeros; i++)
		{
			var mask = (i % 2 == 0) ? (byte)0xf0 : (byte)0x0f;
			if ((bytes[i / 2] & mask) != 0x00)
				return false;
		}
		return true;
	}

#pragma warning disable CA5351 // Do Not Use Broken Cryptographic Algorithms
#pragma warning disable CA1850 // Prefer static 'HashData' method over 'ComputeHash'
	private static int GetPassword(string input, int numZeros)
	{
		using var md5 = MD5.Create();
		for (var i = 0; ; i++)
		{
			var hashSrc = input + i.ToString();
			var hashSrcBytes = Encoding.ASCII.GetBytes(hashSrc);
			var hash = md5.ComputeHash(hashSrcBytes);
			if (HasLeadingZeros(numZeros, hash))
				return i;
		}
	}
#pragma warning restore CA1850 // Prefer static 'HashData' method over 'ComputeHash'
#pragma warning restore CA5351 // Do Not Use Broken Cryptographic Algorithms

	public (string, string) Solve(PuzzleInput input)
	{
		var inp = input.Lines[0];
		return (
			GetPassword(inp, 5).ToString(),
			GetPassword(inp, 6).ToString());
	}
}
