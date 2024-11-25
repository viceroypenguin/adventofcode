namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 16, CodeType.Original)]
public partial class Day_16_Original : IPuzzle
{
	[GeneratedRegex(
		"^((?<spin>s(?<amt>\\d+))|(?<xchg>x(?<xchg_a>\\d+)/(?<xchg_b>\\d+))|(?<partner>p(?<part_a>\\w)/(?<part_b>\\w)))$",
		RegexOptions.ExplicitCapture
	)]
	private static partial Regex IsntructionRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = IsntructionRegex();
		var instructions = input.Text
			.Split(',')
			.Select(inst => regex.Match(inst))
			.ToList();

		const int Length = 16;
		var programs = Enumerable.Range(0, Length)
			.Select(i => (char)(i + 'a'))
			.ToArray();

		char[] Round(char[] @in)
		{
			var @out = @in.ToArray();

			foreach (var m in instructions)
			{
				if (m.Groups["spin"].Success)
				{
					var amt = Convert.ToInt32(m.Groups["amt"].Value);
					@out = @out.Skip(Length - amt).Concat(@out.Take(Length - amt)).ToArray();
				}
				else if (m.Groups["xchg"].Success)
				{
					var idx_a = Convert.ToInt32(m.Groups["xchg_a"].Value);
					var idx_b = Convert.ToInt32(m.Groups["xchg_b"].Value);
					(@out[idx_b], @out[idx_a]) = (@out[idx_a], @out[idx_b]);
				}
				else if (m.Groups["partner"].Success)
				{
					var a = m.Groups["part_a"].Value[0];
					var b = m.Groups["part_b"].Value[0];
					for (var i = 0; i < Length; i++)
					{
						if (@out[i] == a) @out[i] = b;
						else if (@out[i] == b) @out[i] = a;
					}
				}
			}

			return @out;
		}

		programs = Round(programs);
		var partA = string.Join("", programs);

		var k = 1;
		var programs_dbl = Round(programs);
		var l = 2;

		while (true)
		{
			programs = Round(programs);
			programs_dbl = Round(Round(programs_dbl));
			k++;
			l += 2;

			if (programs.SequenceEqual(programs_dbl))
				break;
		}

		var final_round = 1_000_000_000;
		var remainder = final_round % k;
		for (var i = 0; i < remainder; i++)
			programs = Round(programs);

		var partB = string.Join("", programs);

		return (partA, partB);
	}
}
