namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 07, CodeType.Fastest)]
public sealed partial class Day_07_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;

		var n = span.Count((byte)'\n');
		Span<ulong> cardsP1 = stackalloc ulong[n];
		Span<ulong> cardsP2 = stackalloc ulong[n];

		Span<byte> counts = stackalloc byte[16];

		n = 0;
		foreach (var line in span.EnumerateLines())
		{
			if (line.Length == 0) break;

			(var bid, _) = line[6..].AtoI();
			var p1 = (ulong)bid;
			var p2 = p1;

			counts.Clear();
			var numCards = 0u;
			var maxOfAKind = 0u;

			for (var i = 0; i < 5; i++)
			{
				var card = line[i];
				var value = card switch
				{
					(byte)'A' => 14,
					(byte)'K' => 13,
					(byte)'Q' => 12,
					(byte)'J' => 11,
					(byte)'T' => 10,
					_ => (byte)(card & 0x0f),
				};

				var cnt = ++counts[value];
				if (cnt == 1)
					numCards++;

				p1 |= (ulong)value << (12 + (4 * (5 - i)));

				if (value == 11)
					value = 1;
				else if (cnt > maxOfAKind)
					maxOfAKind = cnt;

				p2 |= (ulong)value << (12 + (4 * (5 - i)));
			}

			var jokers = counts[11];
			p1 |= GetHandType(Math.Max(maxOfAKind, jokers), numCards) << 36;
			p2 |= GetHandType(maxOfAKind + jokers, numCards - (uint)Math.Sign(jokers)) << 36;

			cardsP1[n] = p1;
			cardsP2[n] = p2;
			n++;
		}

		cardsP1.Sort();
		cardsP2.Sort();

		var part1 = 0;
		var part2 = 0;
		for (n = 0; n < cardsP1.Length; n++)
		{
			part1 += (n + 1) * (int)(cardsP1[n] & 0xffff);
			part2 += (n + 1) * (int)(cardsP2[n] & 0xffff);
		}

		return (part1.ToString(), part2.ToString());
	}

	private static ulong GetHandType(uint maxOfAKind, uint numCards) =>
		(maxOfAKind, numCards) switch
		{
			(5, _) => 6,
			(4, _) => 5,
			(3, 2) => 4,
			(3, _) => 3,
			(2, 3) => 2,
			(2, _) => 1,
			_ => 0,
		};
}
