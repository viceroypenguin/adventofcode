using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
	public class Day_2017_16_Original : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 16;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var regex = new Regex(@"^((?<spin>s(?<amt>\d+))|(?<xchg>x(?<xchg_a>\d+)/(?<xchg_b>\d+))|(?<partner>p(?<part_a>\w)/(?<part_b>\w)))$", RegexOptions.Compiled);
			var instructions = input.GetString()
				.Split(',')
				.Select(inst => regex.Match(inst))
				.ToList();

			const int length = 16;
			var programs = Enumerable.Range(0, length)
				.Select(i => (char)(i + (int)'a'))
				.ToArray();

			char[] Round(char[] @in)
			{
				var @out = @in.ToArray();

				foreach (var m in instructions)
				{
					if (m.Groups["spin"].Success)
					{
						var amt = Convert.ToInt32(m.Groups["amt"].Value);
						@out = @out.Skip(length - amt).Concat(@out.Take(length - amt)).ToArray();
					}
					else if (m.Groups["xchg"].Success)
					{
						var idx_a = Convert.ToInt32(m.Groups["xchg_a"].Value);
						var idx_b = Convert.ToInt32(m.Groups["xchg_b"].Value);
						var tmp = @out[idx_a];
						@out[idx_a] = @out[idx_b];
						@out[idx_b] = tmp;
					}
					else if (m.Groups["partner"].Success)
					{
						var a = m.Groups["part_a"].Value[0];
						var b = m.Groups["part_b"].Value[0];
						for (int i = 0; i < length; i++)
							if (@out[i] == a) @out[i] = b;
							else if (@out[i] == b) @out[i] = a;
					}
				}

				return @out;
			}

			programs = Round(programs);
			Dump('A', string.Join("", programs));

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
			for (int i = 0; i < remainder; i++)
				programs = Round(programs);

			Dump('B', string.Join("", programs));
		}
	}
}
