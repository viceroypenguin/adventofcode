namespace AdventOfCode;

public class Day_2016_06_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 6;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var words = input.GetLines();

		Func<IEnumerable<char>, int, char> processLetter = (characters, multiplier) =>
			characters
				.GroupBy(
					c => c,
					(c, _) => new { c, cnt = _.Count(), })
				.OrderBy(c => c.cnt * multiplier)
				.Select(c => c.c)
				.First();

		Dump('A',
			string.Join("",
				Enumerable.Range(0, words[0].Length)
					.Select(i => words.Select(w => w[i]))
					.Select(characters => processLetter(characters, -1))));

		Dump('B',
			string.Join("",
				Enumerable.Range(0, words[0].Length)
					.Select(i => words.Select(w => w[i]))
					.Select(characters => processLetter(characters, 1))));
	}
}
