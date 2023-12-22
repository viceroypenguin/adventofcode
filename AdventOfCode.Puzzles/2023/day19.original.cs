namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 19, CodeType.Original)]
public partial class Day_19_Original : IPuzzle
{
	private sealed record Rule(string Destination)
	{
		public char? Category { get; init; }
		public bool? IsGreater { get; init; }
		public int? Value { get; init; }
	}

	public (string, string) Solve(PuzzleInput input)
	{
		var workflowRegex = new Regex(@"^(?<name>\w+)\{(?<rules>.*)\}$");
		var ruleRegex = new Regex(@"^(?<chk>(?<cat>\w)(?<gr>[<>])(?<num>\d+)\:)?(?<dest>\w+)$");

		var segments = input.Lines.Split(string.Empty).ToList();
		var workflows = segments[0]
			.Select(l => workflowRegex.Match(l))
			.Select(l => new
			{
				Name = l.Groups["name"].Value,
				Rules = l.Groups["rules"].Value
					.Split(',')
					.Select(x => ruleRegex.Match(x))
					.Select(x => x.Groups["chk"].Success
						? new Rule(x.Groups["dest"].Value)
						{
							Category = x.Groups["cat"].Value[0],
							IsGreater = x.Groups["gr"].Value == ">",
							Value = int.Parse(x.Groups["num"].Value),
						}
						: new Rule(x.Groups["dest"].Value))
					.ToList(),
			})
			.ToDictionary(x => x.Name);

		var ratingRegex = new Regex(@"^\{x=(?<x>\d+),m=(?<m>\d+),a=(?<a>\d+),s=(?<s>\d+)\}");
		var part1 = 0;
		foreach (var l in segments[1])
		{
			var m = ratingRegex.Match(l);
			var rating = new Dictionary<char, int>()
			{

				['x'] = int.Parse(m.Groups["x"].Value),
				['m'] = int.Parse(m.Groups["m"].Value),
				['s'] = int.Parse(m.Groups["s"].Value),
				['a'] = int.Parse(m.Groups["a"].Value),
			};

			var flow = workflows["in"];
			while (flow != null)
			{
				foreach (var rule in flow.Rules)
				{
					if (rule.Category != null)
					{
						var value = rating[rule.Category.Value];
						var check = rule.IsGreater!.Value
							? value > rule.Value!.Value
							: value < rule.Value!.Value;
						if (!check)
							continue;
					}

					var dest = rule.Destination;
					if (dest == "R")
					{
						flow = null;
						break;
					}

					if (dest == "A")
					{
						part1 += rating.Values.Sum();
						flow = null;
						break;
					}

					flow = workflows[dest];
					break;
				}
			}
		}

		var part2 = 0L;

		var queue = new Queue<State>();
		queue.Enqueue(new("in", new Dictionary<char, (int start, int end)>()
		{
			['x'] = (1, 4000),
			['m'] = (1, 4000),
			['s'] = (1, 4000),
			['a'] = (1, 4000),
		}));
		while (queue.TryDequeue(out var state))
		{
			var potentials = state.Potentials;
			if (state.Flow == "R")
				continue;

			if (state.Flow == "A")
			{
				part2 += potentials.Aggregate(1L, (a, b) => a * (b.Value.end - b.Value.start + 1));
				continue;
			}

			foreach (var rule in workflows[state.Flow].Rules)
			{
				if (rule.Category != null)
				{
					var (start, end) = potentials[rule.Category.Value];
					var value = rule.Value!.Value;

					if (rule.IsGreater!.Value)
					{
						if (start > value)
						{
							queue.Enqueue(new(rule.Destination, potentials));
							break;
						}
						else if (end > value)
						{
							potentials[rule.Category.Value] = (start, value);
							start = value + 1;
						}
					}
					else
					{
						if (end < value)
						{
							queue.Enqueue(new(rule.Destination, potentials));
							break;
						}
						else if (start < value)
						{
							potentials[rule.Category.Value] = (value, end);
							end = value - 1;
						}
					}

					var newPotentials = potentials.ToDictionary();
					newPotentials[rule.Category.Value] = (start, end);
					queue.Enqueue(new(rule.Destination, newPotentials));
				}
				else
				{
					queue.Enqueue(new(rule.Destination, potentials));
				}
			}
		}

		return (part1.ToString(), part2.ToString());
	}

	private sealed record State(string Flow, Dictionary<char, (int start, int end)> Potentials);
}
