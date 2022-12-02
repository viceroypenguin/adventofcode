using System.Runtime.Intrinsics.X86;

namespace AdventOfCode;

public class Day_2020_14_Original : Day
{
	public override int Year => 2020;
	public override int DayNumber => 14;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var regex = new Regex(@"^(mask = (?<mask>[01X]{36}))|(mem\[(?<memloc>\d+)\] = (?<memval>\d+))$");
		var matches = input.GetLines()
			.Select(x => regex.Match(x))
			.ToArray();

		DoPartA(matches);
		DoPartB(matches);
	}

	private void DoPartA(Match[] matches)
	{
		var memory = new Dictionary<int, ulong>();
		var mask = (and: ulong.MaxValue, or: ulong.MinValue);
		foreach (var m in matches)
		{
			if (m.Groups["mask"].Success)
			{
				var s = m.Groups["mask"].Value;
				mask = (ulong.MaxValue, ulong.MinValue);
				for (int i = 0; i < s.Length; i++)
				{
					if (s[i] == '0')
						mask.and &= ~(1ul << (35 - i));
					else if (s[i] == '1')
						mask.or |= 1ul << (35 - i);
				}
			}
			else
			{
				memory[int.Parse(m.Groups["memloc"].Value)] =
					(ulong.Parse(m.Groups["memval"].Value) & mask.and) | mask.or;
			}
		}

		PartA = memory.Sum(kvp => (long)kvp.Value).ToString();
	}

	private void DoPartB(Match[] matches)
	{
		var memory = new Dictionary<ulong, ulong>();
		var mask = (fl: ulong.MinValue, or: ulong.MinValue);
		foreach (var m in matches)
		{
			if (m.Groups["mask"].Success)
			{
				var s = m.Groups["mask"].Value;
				mask = (ulong.MinValue, ulong.MinValue);
				for (int i = 0; i < s.Length; i++)
				{
					if (s[i] == 'X')
						mask.fl |= 1ul << (35 - i);
					else if (s[i] == '1')
						mask.or |= 1ul << (35 - i);
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

		PartB = memory.Sum(kvp => (long)kvp.Value).ToString();
	}
}
