using System.Collections;
using System.Collections.Immutable;

namespace AdventOfCode;

public class Day_2021_12_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 12;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var edges = input.GetLines()
			.Select(l => l.Split('-'))
			.SelectMany(l => (new[] { (l[0], l[1]), (l[1], l[0]), }))
			.Where(x => x.Item2 != "start")
			.Where(x => x.Item1 != "end")
			.ToLookup(x => x.Item1, x => x.Item2);

		var paths = MoreEnumerable
			.TraverseBreadthFirst(
				ImmutableList<string>.Empty.Add("start"),
				l => edges[l.Last()]
					.Where(e => e == "end"
						|| char.IsUpper(e[0])
						|| !l.Contains(e))
					.Select(e => l.Add(e)))
			.Where(e => e.Last() == "end")
			.Count();

		PartA = paths.ToString();


		paths = MoreEnumerable
			.TraverseBreadthFirst(
				(path: ImmutableList<string>.Empty.Add("start"), visitedTwice: false),
				l => edges[l.path.Last()]
					.Where(e => e == "end"
						|| !l.visitedTwice
						|| char.IsUpper(e[0])
						|| !l.path.Contains(e))
					.Select(e => (l.path.Add(e), l.visitedTwice || (char.IsLower(e[0]) && l.path.Contains(e)))))
			.Where(e => e.path.Last() == "end")
			.Count();

		PartB = paths.ToString();
	}
}
