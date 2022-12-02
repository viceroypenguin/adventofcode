namespace AdventOfCode;

public class Day_2018_02_Original : Day
{
	public override int Year => 2018;
	public override int DayNumber => 2;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var ids = input.GetLines();

		var counts = ids
			.Select(id =>
			{
				var chars = id
					.GroupBy(c => c)
					.Select(x => x.Count())
					.ToList();
				return (two: chars.Any(x => x == 2), three: chars.Any(x => x == 3));
			})
			.Aggregate((twos: 0, threes: 0), (acc, next) =>
				(acc.twos + (next.two ? 1 : 0), acc.threes + (next.three ? 1 : 0)));

		Dump('A', counts.twos * counts.threes);

		Dump('B',
			new string(
				ids
					.OrderBy(x => x)
					.Window(2)
					.Select(pair => (
						pair,
						letters: pair[0].Zip(pair[1], (l, r) => (l, r))))
					.Where(x => x.letters.Count(y => y.l != y.r) == 1)
					.SelectMany(x => x.letters.Where(y => y.l == y.r))
					.Select(x => x.l)
					.ToArray()));
	}
}
