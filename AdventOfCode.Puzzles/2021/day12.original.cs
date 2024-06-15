using System.Collections.Immutable;

namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 12, CodeType.Original)]
public class Day_12_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var edges = input.Lines
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

		var part1 = paths.ToString();

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

		var part2 = paths.ToString();

		return (part1, part2);
	}
}
