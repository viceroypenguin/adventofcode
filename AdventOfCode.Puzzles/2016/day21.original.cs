namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 21, CodeType.Original)]
public partial class Day_21_Original : IPuzzle
{
	[GeneratedRegex(
		"""
		swap\ (
			(?<swap_position>position\ (?<position_x>\d+)\ with\ position\ (?<position_y>\d+)) |
			(?<swap_letter>letter\ (?<letter_x>\w)\ with\ letter\ (?<letter_y>\w))  ) |
		rotate\ (
			(?<rotate_steps>(?<rotate_direction>left|right)\ (?<rotate_count>\d+)\ steps?) |
			(?<rotate_position>based\ on\ position\ of\ letter\ (?<rotate_letter>\w))  ) |
		(?<reverse>reverse\ positions\ (?<reverse_x>\d+)\ through\ (?<reverse_y>\d+)) |
		(?<move>move\ position\ (?<move_x>\d+)\ to\ position\ (?<move_y>\d+))
		""",
		RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture)]
	private static partial Regex InstructionRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = InstructionRegex();

		var instructions =
			input.Lines
				.Select(s => regex.Match(s))
				.ToList();

		return (
			DoPartA(instructions),
			DoPartB(instructions));
	}

	private string DoPartA(List<Match> instructions)
	{
		var password = "abcdefgh";

		var working = password.ToList();
		foreach (var i in instructions)
		{
			if (i.Groups["swap_position"].Success)
			{
				var x = Convert.ToInt32(i.Groups["position_x"].Value);
				var y = Convert.ToInt32(i.Groups["position_y"].Value);

				(working[y], working[x]) = (working[x], working[y]);
			}
			else if (i.Groups["swap_letter"].Success)
			{
				var x = i.Groups["letter_x"].Value[0];
				var y = i.Groups["letter_y"].Value[0];

				var x_index = working.FindIndex(c => c == x);
				var y_index = working.FindIndex(c => c == y);

				working[x_index] = y;
				working[y_index] = x;
			}
			else if (i.Groups["rotate_steps"].Success)
			{
				var dir = i.Groups["rotate_direction"].Value;
				var steps = Convert.ToInt32(i.Groups["rotate_count"].Value) % working.Count;

				if (dir == "left")
				{
					working = working.Skip(steps).Concat(working.Take(steps)).ToList();
				}
				else
				{
					steps = working.Count - steps;
					working = working.Skip(steps).Concat(working.Take(steps)).ToList();
				}
			}
			else if (i.Groups["rotate_position"].Success)
			{
				var letter = i.Groups["rotate_letter"].Value[0];
				var steps = working.FindIndex(c => c == letter);

				if (steps >= 4) steps++;
				steps++;

				steps %= working.Count;

				steps = working.Count - steps;
				working = working.Skip(steps).Concat(working.Take(steps)).ToList();

			}
			else if (i.Groups["reverse"].Success)
			{
				var x = Convert.ToInt32(i.Groups["reverse_x"].Value);
				var y = Convert.ToInt32(i.Groups["reverse_y"].Value);

				working.Reverse(x, y - x + 1);
			}
			else if (i.Groups["move"].Success)
			{
				var x = Convert.ToInt32(i.Groups["move_x"].Value);
				var y = Convert.ToInt32(i.Groups["move_y"].Value);

				var chr = working[x];
				working.RemoveAt(x);
				working.Insert(y, chr);
			}
		}

		return string.Join("", working);
	}

	private string DoPartB(List<Match> instructions)
	{
		var scramble = "fbgdceah";

		var working = scramble.ToList();
		foreach (var i in instructions.AsEnumerable().Reverse())
		{
			if (i.Groups["swap_position"].Success)
			{
				var x = Convert.ToInt32(i.Groups["position_x"].Value);
				var y = Convert.ToInt32(i.Groups["position_y"].Value);

				(working[y], working[x]) = (working[x], working[y]);
			}
			else if (i.Groups["swap_letter"].Success)
			{
				var x = i.Groups["letter_x"].Value[0];
				var y = i.Groups["letter_y"].Value[0];

				var x_index = working.FindIndex(c => c == x);
				var y_index = working.FindIndex(c => c == y);

				working[x_index] = y;
				working[y_index] = x;
			}
			else if (i.Groups["rotate_steps"].Success)
			{
				var dir = i.Groups["rotate_direction"].Value;
				var steps = Convert.ToInt32(i.Groups["rotate_count"].Value) % working.Count;

				if (dir == "left")
				{
					steps = working.Count - steps;
					working = working.Skip(steps).Concat(working.Take(steps)).ToList();
				}
				else
				{
					working = working.Skip(steps).Concat(working.Take(steps)).ToList();
				}
			}
			else if (i.Groups["rotate_position"].Success)
			{
				var letter = i.Groups["rotate_letter"].Value[0];
				var endsUpAt = working.FindIndex(c => c == letter);

				var originallyAt = 0;
				for (; originallyAt < working.Count; originallyAt++)
				{
					var newPosition =
						originallyAt + // original position
						1 + // plus 1
						originallyAt + // plus shift by index
						(originallyAt >= 4 ? 1 : 0); // plus one more if four or more

					if (newPosition % working.Count == endsUpAt)
						break;
				}

				if (endsUpAt > originallyAt)
				{
					var steps = endsUpAt - originallyAt;
					working = working.Skip(steps).Concat(working.Take(steps)).ToList();
				}
				else
				{
					var steps = working.Count - (originallyAt - endsUpAt);
					working = working.Skip(steps).Concat(working.Take(steps)).ToList();
				}
			}
			else if (i.Groups["reverse"].Success)
			{
				var x = Convert.ToInt32(i.Groups["reverse_x"].Value);
				var y = Convert.ToInt32(i.Groups["reverse_y"].Value);

				working.Reverse(x, y - x + 1);
			}
			else if (i.Groups["move"].Success)
			{
				var x = Convert.ToInt32(i.Groups["move_x"].Value);
				var y = Convert.ToInt32(i.Groups["move_y"].Value);

				var chr = working[y];
				working.RemoveAt(y);
				working.Insert(x, chr);
			}
		}

		return string.Join("", working);
	}
}
