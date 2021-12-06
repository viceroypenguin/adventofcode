using System.Collections;

namespace AdventOfCode;

public class Day_2021_06_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 6;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		DoPartA(input);
		DoPartB(input);
	}

	private void DoPartA(byte[] input)
	{
		var fish = input.GetString()
			.Split(',')
			.Select(int.Parse)
			.ToList();

		for (int i = 0; i < 80; i++)
		{
			var newFish = fish.Where(x => x == 0).Select(x => 8).ToList();
			fish = fish.Select(x => x == 0 ? 6 : x - 1).Concat(newFish).ToList();
		}

		PartA = fish.Count().ToString();
	}

	private void DoPartB(byte[] input)
	{
		var fish = input.GetString()
			.Split(',')
			.Select(int.Parse)
			.GroupBy(x => x)
			.ToDictionary(g => g.Key, g => (long)g.Count());

		for (int i = 0; i < 256; i++)
		{
			var newDict = new Dictionary<int, long>();
			newDict[8] = fish.GetValueOrDefault(0);
			newDict[7] = fish.GetValueOrDefault(8);
			newDict[6] = fish.GetValueOrDefault(0) + fish.GetValueOrDefault(7);
			newDict[5] = fish.GetValueOrDefault(6);
			newDict[4] = fish.GetValueOrDefault(5);
			newDict[3] = fish.GetValueOrDefault(4);
			newDict[2] = fish.GetValueOrDefault(3);
			newDict[1] = fish.GetValueOrDefault(2);
			newDict[0] = fish.GetValueOrDefault(1);
			fish = newDict;
		}

		PartB = fish.Values.Sum().ToString();
	}
}
