using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 14, CodeType.Original)]
public partial class Day_14_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		return (
			ExecutePart(input.Lines[0], 1).ToString(),
			ExecutePart(input.Lines[0], 2017).ToString());
	}

	[GeneratedRegex("(\\w|\\d)\\1{2}", RegexOptions.Compiled)]
	private static partial Regex TripletRegex();

#pragma warning disable CA5351 // Do Not Use Broken Cryptographic Algorithms
#pragma warning disable CA1850 // Prefer static 'HashData' method over 'ComputeHash'
	private static int ExecutePart(string input, int numHashes)
	{
		using var md5 = MD5.Create();

		var threeMatchingRegex = TripletRegex();

		var queue = new Queue<string>();
		var index = -1;
		var numKeys = 0;

		var counter = 0;
		string GetNextHash()
		{
			var hashSrc = input + counter;
			counter++;

			var hashText = hashSrc;
			for (var i = 0; i < numHashes; i++)
			{
				var bytes = Encoding.ASCII.GetBytes(hashText);
				var hash = md5.ComputeHash(bytes);
				hashText = BitConverter.ToString(hash).ToLower().Replace("-", "");
			}

			return hashText;
		}

		void EnsureQueueLength()
		{
			while (queue.Count < 1000)
				queue.Enqueue(GetNextHash());
		}

		EnsureQueueLength();

		while (numKeys < 64)
		{
			var possibleKey = new { hash = queue.Dequeue(), index };
			index++;
			EnsureQueueLength();

			var match = threeMatchingRegex.Match(possibleKey.hash);
			if (!match.Success) continue;

			var fiveLetter = new string(match.Value[0], 5);

			if (queue.Any(h => h.Contains(fiveLetter)))
				numKeys++;
		}

		return index;
	}
#pragma warning restore CA1850 // Prefer static 'HashData' method over 'ComputeHash'
#pragma warning restore CA5351 // Do Not Use Broken Cryptographic Algorithms
}
