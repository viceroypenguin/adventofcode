using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 05, CodeType.Original)]
public class Day_05_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		return (
			DoPartA(input.Lines[0]),
			DoPartB(input.Lines[0]));
	}

#pragma warning disable CA5351 // Do Not Use Broken Cryptographic Algorithms
#pragma warning disable CA1850 // Prefer static 'HashData' method over 'ComputeHash'
	private static string DoPartA(string key)
	{
		using var md5 = MD5.Create();
		var password = "";

		var cnt = 0L;
		while (password.Length < 8)
		{
			cnt++;
			var hashSrc = key + cnt.ToString();
			var bytes = Encoding.ASCII.GetBytes(hashSrc);
			var hash = md5.ComputeHash(bytes);
			if (hash[0] == 0x00 &&
				hash[1] == 0x00 &&
				(hash[2] & 0xf0) == 0x00)
			{
				password += (hash[2] & 0x0f).ToString("x");
			}
		}

		return password;
	}

	private static string DoPartB(string key)
	{
		using var md5 = MD5.Create();
		var password = new char?[8];
		var result = string.Empty;

		var cnt = 0L;
		while (password.Any(c => !c.HasValue))
		{
			cnt++;
			var hashSrc = key + cnt.ToString();
			var bytes = Encoding.ASCII.GetBytes(hashSrc);
			var hash = md5.ComputeHash(bytes);
			if (hash[0] == 0x00 &&
				hash[1] == 0x00 &&
				(hash[2] & 0xf0) == 0x00)
			{
				var idx = hash[2] & 0x0f;
				if (idx >= 8) continue;
				if (password[idx].HasValue) continue;

				password[idx] = ((hash[3] & 0xf0) >> 4).ToString("x")[0];
				result = string.Join("", password.Select(c => c ?? '_'));
			}
		}

		return result;
	}
#pragma warning restore CA1850 // Prefer static 'HashData' method over 'ComputeHash'
#pragma warning restore CA5351 // Do Not Use Broken Cryptographic Algorithms
}
