using System.Collections;

namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 12, CodeType.Original)]
public class Day_12_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var numGenerations = 150;

		var lines = input.Lines;
		var initialStateStr = lines[0].Replace("initial state: ", "");

		var bits = new BitArray(initialStateStr.Length + (numGenerations * 2) + 8);
		foreach (var (c, i) in initialStateStr.Select((c, i) => (c, i)))
			bits[i + numGenerations + 2] = c == '#';

		int BuildInt(IList<bool> arr)
		{
			var ret = 0;
			for (var i = 0; i < arr.Count; i++)
				ret |= arr[i] ? 1 << i : 0;
			return ret;
		}

		var map = lines
			.Skip(2)
			.Select(l => l.Split(" => ", StringSplitOptions.None))
			.ToDictionary(
				l => BuildInt(l[0].Select(c => c == '#').ToList()),
				l => l[1] == "#");

		bool GetValue(IList<bool> arr)
		{
			var key = BuildInt(arr);
			return map.TryGetValue(key, out var ret)
				&& ret;
		}

		for (var gen = 1; gen <= 20; gen++)
		{
			var nextBits = new BitArray(bits.Count);
			foreach (var (w, i) in bits
				.OfType<bool>()
				.Window(5)
				.Select((x, i) => (x, i)))
			{
				nextBits[i + 2] = GetValue(w);
			}

			bits = nextBits;
		}

		var part1 = Enumerable.Range(0, bits.Count)
			.Select(i => (idx: i - (numGenerations + 2), val: bits[i]))
			.Where(x => x.val)
			.Sum(x => x.idx)
			.ToString();

		for (var gen = 20; gen <= numGenerations; gen++)
		{
			var nextBits = new BitArray(bits.Count);
			foreach (var (w, i) in bits
				.OfType<bool>()
				.Window(5)
				.Select((x, i) => (x, i)))
			{
				nextBits[i + 2] = GetValue(w);
			}

			bits = nextBits;
		}

		var part2 = Enumerable.Range(0, bits.Count)
			.Select(i => (idx: i - (numGenerations + 2), val: bits[i]))
			.Where(x => x.val)
			.Sum(x => x.idx + (50_000_000_000L - numGenerations - 1))
			.ToString();

		return (part1, part2);
	}
}
