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

		var paths = SuperEnumerable
			.TraverseDepthFirst(
				(cur: "start", visited: ImmutableHashSet<string>.Empty),
				l => edges[l.cur]
					.Where(e => e == "end"
						|| char.IsUpper(e[0])
						|| !l.visited.Contains(e))
					.Select(e => (e, char.IsLower(e[0]) ? l.visited.Add(e) : l.visited)))
			.Where(e => e.cur == "end")
			.Count();

		PartA = paths.ToString();

		paths = SuperEnumerable
			.TraverseDepthFirst(
				(cur: "start", visitedTwice: false, visited: ImmutableHashSet<string>.Empty),
				l => edges[l.cur]
					.Where(e => e == "end"
						|| !l.visitedTwice
						|| char.IsUpper(e[0])
						|| !l.visited.Contains(e))
					.Select(e => (
						e,
						l.visitedTwice || (char.IsLower(e[0]) && l.visited.Contains(e)), 
						char.IsLower(e[0]) ? l.visited.Add(e) : l.visited)))
			.Where(e => e.cur == "end")
			.Count();

		PartB = paths.ToString();
	}
}
