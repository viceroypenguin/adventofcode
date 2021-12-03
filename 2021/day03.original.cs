using System.Collections;

namespace AdventOfCode;

public class Day_2021_03_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 3;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var lines = input.GetLines();

		var (epsilon, gamma) = lines
			.Aggregate(new int[lines[0].Length], (x, p) =>
				x.Zip(p, (a, b) => a + (b == '1' ? 1 : 0)).ToArray())
			.Aggregate((epsilon: 0, gamma: 0), (x, cnt) =>
				cnt < (lines.Length / 2)
					? ((x.epsilon << 1) + 1, x.gamma << 1)
					: (x.epsilon << 1, (x.gamma << 1) + 1));

		PartA = (epsilon * gamma).ToString();

		var tmp = lines.ToList();
		for (int i = 0; i < lines.Length; i++)
		{
			if (tmp.Count == 1)
				break;

			var cnt = tmp.Count(s => s[i] == '1');
			if (cnt >= tmp.Count / 2.0)
				tmp.RemoveAll(x => x[i] == '0');
			else
				tmp.RemoveAll(x => x[i] == '1');
		}
		var oxygenCount = Convert.ToInt32(tmp[0], 2);

		tmp = lines.ToList();
		for (int i = 0; i < lines.Length; i++)
		{
			if (tmp.Count == 1)
				break;

			var cnt = tmp.Count(s => s[i] == '0');
			if (cnt <= tmp.Count / 2.0)
				tmp.RemoveAll(x => x[i] == '1');
			else
				tmp.RemoveAll(x => x[i] == '0');
		}
		var coScrub = Convert.ToInt32(tmp[0], 2);

		PartB = (oxygenCount * coScrub).ToString();
	}
}
