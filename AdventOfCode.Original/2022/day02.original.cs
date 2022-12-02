namespace AdventOfCode;

public class Day_2022_02_Original : Day
{
	public override int Year => 2022;
	public override int DayNumber => 2;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		int GetRps(string s) =>
			s switch
			{
				"A" or "X" => 1,
				"B" or "Y" => 2,
				"C" or "Z" => 3,
			};

		int GamePoints(int a, int b) =>
			(a, b) switch
			{
				(1, 1) => 3,
				(2, 2) => 3,
				(3, 3) => 3,
				(1, 2) => 6,
				(2, 3) => 6,
				(3, 1) => 6,
				_ => 0,
			};

		PartA = input.GetLines()
			.Select(x => x.Split())
			.Select(x => (GetRps(x[0]), GetRps(x[1])))
			.Select(x => GamePoints(x.Item1, x.Item2) + x.Item2)
			.Sum()
			.ToString();

		int GetWld(string s) =>
			s switch
			{
				"X" => 0,
				"Y" => 3,
				"Z" => 6
			};

		int GetShape(int a, int b) =>
			(a, b) switch
			{
				(1, 0) => 3,
				(1, 3) => 1,
				(1, 6) => 2,
				(2, 0) => 1,
				(2, 3) => 2,
				(2, 6) => 3,
				(3, 0) => 2,
				(3, 3) => 3,
				(3, 6) => 1,
			};

		PartB = input.GetLines()
			.Select(x => x.Split())
			.Select(x => (GetRps(x[0]), GetWld(x[1])))
			.Select(x => GetShape(x.Item1, x.Item2) + x.Item2)
			.Sum()
			.ToString();
	}
}
