using System.Runtime.Intrinsics.X86;

namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 14, CodeType.Original)]
public partial class Day_14_Original : IPuzzle
{
	[GeneratedRegex("^(mask = (?<mask>[01X]{36}))|(mem\\[(?<memloc>\\d+)\\] = (?<memval>\\d+))$")]
	private static partial Regex InstructionRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = InstructionRegex();
		var matches = input.Lines
			.Select(x => regex.Match(x))
			.ToArray();

		var part1 = DoPartA(matches);
		var part2 = DoPartB(matches);
		return (part1, part2);
	}

	private static string DoPartA(Match[] matches)
	{
		var memory = new Dictionary<int, ulong>();
		var mask = (and: ulong.MaxValue, or: ulong.MinValue);
		foreach (var m in matches)
		{
			if (m.Groups["mask"].Success)
			{
				var s = m.Groups["mask"].Value;
				mask = (ulong.MaxValue, ulong.MinValue);
				for (var i = 0; i < s.Length; i++)
				{
					if (s[i] == '0')
						mask.and &= ~(1ul << 35 - i);
					else if (s[i] == '1')
						mask.or |= 1ul << 35 - i;
				}
			}
			else
			{
				memory[int.Parse(m.Groups["memloc"].Value)] =
					ulong.Parse(m.Groups["memval"].Value) & mask.and | mask.or;
			}
		}

		return memory.Sum(kvp => (long)kvp.Value).ToString();
	}

	private static string DoPartB(Match[] matches)
	{
		var memory = new Dictionary<ulong, ulong>();
		var mask = (fl: ulong.MinValue, or: ulong.MinValue);
		foreach (var m in matches)
		{
			if (m.Groups["mask"].Success)
			{
				var s = m.Groups["mask"].Value;
				mask = (ulong.MinValue, ulong.MinValue);
				for (var i = 0; i < s.Length; i++)
				{
					if (s[i] == 'X')
						mask.fl |= 1ul << 35 - i;
					else if (s[i] == '1')
						mask.or |= 1ul << 35 - i;
				}
			}
			else
			{
				static IEnumerable<ulong> getValues(ulong baseValue, ulong fl)
				{
					var lowest = Bmi1.X64.ExtractLowestSetBit(fl);
					if (lowest == 0) return SuperEnumerable.Return(baseValue);
					fl &= ~lowest;
					return getValues(baseValue, fl).Concat(
						getValues(baseValue | lowest, fl));
				}

				var baseLocation = (ulong.Parse(m.Groups["memloc"].Value) | mask.or) & ~mask.fl;
				var value = ulong.Parse(m.Groups["memval"].Value);
				foreach (var v in getValues(baseLocation, mask.fl))
					memory[v] = value;
			}
		}

		return memory.Sum(kvp => (long)kvp.Value).ToString();
	}
}
