using System.Diagnostics;
using System.Text;

namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 09, CodeType.Original)]
public class Day_09_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		return (
			DoPartA(input.Lines[0]).ToString(),
			DoPartB(input.Lines[0]).ToString());
	}

	private static int DoPartA(string input)
	{
		var state = "outside";
		var index = 0;

		var output = new StringBuilder();

		var tmp = new StringBuilder();
		var numChars = 0;

		while (index < input.Length)
		{
			switch (state)
			{
				case "outside":
					if (input[index] == '(')
						state = "num_chars";
					else
						_ = output.Append(input[index]);
					break;

				case "num_chars":
					if (input[index] == 'x')
					{
						numChars = Convert.ToInt32(tmp.ToString());
						state = "repeats";
						_ = tmp.Clear();
					}
					else
					{
						_ = tmp.Append(input[index]);
					}

					break;

				case "repeats":
					if (input[index] == ')')
					{
						var repeats = Convert.ToInt32(tmp.ToString());
						_ = tmp.Clear();

						numChars = Math.Min(numChars, input.Length - (index + 1));
						var charsToRepeat = input.Substring(index + 1, numChars);
						_ = output.Append(string.Join("", Enumerable.Range(0, repeats).Select(_ => charsToRepeat)));

						state = "outside";
						index += numChars;
					}
					else
					{
						_ = tmp.Append(input[index]);
					}

					break;

				default:
					throw new UnreachableException();
			}

			index++;
		}

		return output.Length;
	}

	private static long DoPartB(string input) =>
		GetStringLength(input);

	private static long GetStringLength(string str)
	{
		var state = "outside";
		var index = 0;

		var tmp = new StringBuilder();
		var numChars = 0;

		var totalChars = 0L;

		while (index < str.Length)
		{
			switch (state)
			{
				case "outside":
					if (str[index] == '(')
						state = "num_chars";
					else
						totalChars++;
					break;

				case "num_chars":
					if (str[index] == 'x')
					{
						numChars = Convert.ToInt32(tmp.ToString());
						state = "repeats";
						_ = tmp.Clear();
					}
					else
					{
						_ = tmp.Append(str[index]);
					}

					break;

				case "repeats":
					if (str[index] == ')')
					{
						var repeats = Convert.ToInt32(tmp.ToString());
						_ = tmp.Clear();

						numChars = Math.Min(numChars, str.Length - (index + 1));
						var charsToRepeat = str.Substring(index + 1, numChars);
						var realLength = GetStringLength(charsToRepeat);
						totalChars += realLength * repeats;

						state = "outside";
						index += numChars;
					}
					else
					{
						_ = tmp.Append(str[index]);
					}

					break;

				default:
					throw new UnreachableException();
			}

			index++;
		}

		return totalChars;
	}
}
