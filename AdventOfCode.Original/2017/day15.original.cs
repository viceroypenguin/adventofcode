﻿namespace AdventOfCode;

public class Day_2017_15_Original : Day
{
	public override int Year => 2017;
	public override int DayNumber => 15;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var regex = new Regex(@"^Generator (?<gen>\w) starts with (?<init>\d+)$", RegexOptions.Compiled);
		var generators = input.GetLines()
			.Select(l => regex.Match(l))
			.ToDictionary(
				m => m.Groups["gen"].Value,
				m => (ulong)Convert.ToInt32(m.Groups["init"].Value));

		Dump('A',
			Enumerable.Range(0, 40_000_000)
				.ScanEx(
					(a: generators["A"], b: generators["B"]),
					(state, _) => ((state.a * 16807) % 2147483647, (state.b * 48271) % 2147483647))
				.Skip(1)
				.Where(s => (ushort)s.a == (ushort)s.b)
				.Count());

		var aGenerator = SuperEnumerable
			.Generate(generators["A"], a => (a * 16807) % 2147483647)
			.Where(a => a % 4 == 0);
		var bGenerator = SuperEnumerable
			.Generate(generators["B"], b => (b * 48271) % 2147483647)
			.Where(b => b % 8 == 0);

		Dump('B',
			aGenerator.Zip(bGenerator, (a, b) => (a, b))
				.Take(5_000_000)
				.Where(s => (ushort)s.a == (ushort)s.b)
				.Count());
	}
}