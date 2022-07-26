namespace AdventOfCode;

public class Day_2016_09_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 9;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var txt = input.GetString();
		DoPartA(txt);
		DoPartB(txt);
	}

	private void DoPartA(string input)
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
						output.Append(input[index]);
					break;

				case "num_chars":
					if (input[index] == 'x')
					{
						numChars = Convert.ToInt32(tmp.ToString());
						state = "repeats";
						tmp.Clear();
					}
					else
						tmp.Append(input[index]);
					break;

				case "repeats":
					if (input[index] == ')')
					{
						var repeats = Convert.ToInt32(tmp.ToString());
						tmp.Clear();

						numChars = Math.Min(numChars, input.Length - (index + 1));
						var charsToRepeat = input.Substring(index + 1, numChars);
						output.Append(string.Join("", Enumerable.Range(0, repeats).Select(_ => charsToRepeat)));

						state = "outside";
						index += numChars;
					}
					else
						tmp.Append(input[index]);
					break;
			}

			index++;
		}

		//output.ToString().Dump();
		Dump('A', output.Length);
	}

	private void DoPartB(string input)
	{
		Dump('B', GetStringLength(input));
	}

	private long GetStringLength(string str)
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
						tmp.Clear();
					}
					else
						tmp.Append(str[index]);
					break;

				case "repeats":
					if (str[index] == ')')
					{
						var repeats = Convert.ToInt32(tmp.ToString());
						tmp.Clear();

						numChars = Math.Min(numChars, str.Length - (index + 1));
						var charsToRepeat = str.Substring(index + 1, numChars);
						var realLength = GetStringLength(charsToRepeat);
						totalChars += realLength * repeats;

						state = "outside";
						index += numChars;
					}
					else
						tmp.Append(str[index]);
					break;
			}
			index++;
		}

		return totalChars;
	}
}
