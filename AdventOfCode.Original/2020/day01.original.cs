using System.Collections;

namespace AdventOfCode;

public class Day_2020_01_Original : Day
{
	public override int Year => 2020;
	public override int DayNumber => 1;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var numbers = input.GetLines()
			.Select(x => int.Parse(x))
			.ToArray();

		var bitArray = new BitArray(2020);
		foreach (var n in numbers)
		{
			if (bitArray[2020 - n])
				PartA = ((2020 - n) * n).ToString();
			bitArray[n] = true;
		}

		for (var xi = 0; xi < numbers.Length; xi++)
		{
			var x = numbers[xi];
			for (var yi = xi + 1; yi < numbers.Length; yi++)
			{
				var y = numbers[yi];
				var z = 2020 - x - y;
				if (z >= 0 && bitArray[z])
				{
					PartB = (x * y * z).ToString();
					return;
				}
			}
		}
	}
}
