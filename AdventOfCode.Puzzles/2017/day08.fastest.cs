namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 08, CodeType.Fastest)]
public class Day_08_Fastest : IPuzzle
{
	private const int REGISTER_COUNT = 32 * 32 * 32;

	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;

		Span<int> registers = stackalloc int[REGISTER_COUNT];
		var maxValue = 0;

		var idx = 0;
		var c = span[idx];
		while (true)
		{
			while ((c = span[idx]) < 'a')
			{
				if (++idx >= span.Length)
					goto done;
			}

			// destination register
			var dst = c - 'a';
			while ((c = span[++idx]) >= 'a')
				dst = (dst << 5) + c - 'a';

			// inc/dec
			var neg = span[idx + 1] == 'd';

			// amt
			c = span[idx += 5];
			if (c == '-')
			{
				neg = !neg;
				c = span[++idx];
			}

			var num = c - '0';
			while ((c = span[++idx]) >= '0')
				num = (num * 10) + c - '0';
			if (neg) num = -num;

			// src
			c = span[idx += 4];
			var src = c - 'a';
			while ((c = span[++idx]) >= 'a')
				src = (src << 5) + c - 'a';

			// op
			var op = (span[idx + 1] << 8) | span[idx + 2];

			if ((c = span[idx += 3]) == ' ')
				c = span[++idx];

			// chk
			neg = false;
			if (c == '-')
			{
				neg = !neg;
				c = span[++idx];
			}

			var chk = c - '0';
			while ((c = span[++idx]) >= '0')
				chk = (chk * 10) + c - '0';
			if (neg) chk = -chk;

			src = registers[src];
			var flag = op switch
			{
				0x3c20 => src < chk,
				0x3e20 => src > chk,
				0x3c3d => src <= chk,
				0x3e3d => src >= chk,
				0x3d3d => src == chk,
				0x213d => src != chk,
				_ => false,
			};

			if (flag)
			{
				chk = registers[dst] += num;
				if (chk > maxValue)
					maxValue = chk;
			}
		}

done:
		var maxEnd = 0;
		for (var i = 0; i < REGISTER_COUNT; i++)
		{
			if (registers[i] > maxEnd)
				maxEnd = registers[i];
		}

		return (
			maxEnd.ToString(),
			maxValue.ToString());
	}
}
