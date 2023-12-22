using static AdventOfCode.Common.Extensions.NumberExtensions;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 20, CodeType.Original)]
public partial class Day_20_Original : IPuzzle
{
	private enum ModuleType { None, Broadcast, FlipFlop, Conjunction, };
	private enum PulseType { None, Low, High, };
	private class Module
	{
		public required string Name { get; init; }
		public required IReadOnlyList<string> Destinations { get; init; }

		public virtual PulseType SendPulse(PulseType pulseType, string source) => pulseType;
	}

	private sealed class FlipFlipModule : Module
	{
		private bool state;
		public sealed override PulseType SendPulse(PulseType pulseType, string source)
		{
			if (pulseType == PulseType.High)
				return PulseType.None;

			state = !state;
			return state ? PulseType.High : PulseType.Low;
		}
	}

	private sealed class ConjunctionModule : Module
	{
		public IReadOnlyList<string> Inputs { get; set; } = [];
		private readonly Dictionary<string, PulseType> _inputs = [];
		public sealed override PulseType SendPulse(PulseType pulseType, string source)
		{
			_inputs[source] = pulseType;
			return Inputs.All(v => _inputs.GetValueOrDefault(v, PulseType.Low) == PulseType.High)
				? PulseType.Low
				: PulseType.High;
		}
	}

	public (string, string) Solve(PuzzleInput input)
	{
		var configuration = input.Lines
			.Select(l =>
			{
				var split = l.Split(" -> ");
				var name = split[0];

				var c = name[0];
				if (!char.IsLetter(name[0]))
					name = name[1..];

				split = split[1].Split(", ");

				return c switch
				{
					'%' => new FlipFlipModule()
					{
						Name = name,
						Destinations = split,
					},
					'&' => new ConjunctionModule()
					{
						Name = name,
						Destinations = split,
					},
					_ => new Module()
					{
						Name = name,
						Destinations = split,
					},
				};
			})
			.Append(new Module { Name = "button", Destinations = ["broadcaster"], })
			.ToDictionary(c => c.Name);

		var inputs = configuration
			.Values
			.SelectMany(x => x.Destinations.Select(d => (x.Name, Dest: d)))
			.ToLookup(x => x.Dest, x => x.Name);

		foreach (var i in inputs)
		{
			if (configuration.GetValueOrDefault(i.Key) is not ConjunctionModule cm)
				continue;

			cm.Inputs = i.ToList();
		}

		var low = 0L;
		var high = 0L;

		void PushButton()
		{
			var pulses = new Queue<(Module module, PulseType pulse, string source)>();
			pulses.Enqueue((configuration["button"], PulseType.Low, string.Empty));

			while (pulses.TryDequeue(out var x))
			{
				var nextPulse = x.module.SendPulse(x.pulse, x.source);
				if (nextPulse == PulseType.None)
					continue;

				if (nextPulse == PulseType.Low)
					low += x.module.Destinations.Count;
				else
					high += x.module.Destinations.Count;

				foreach (var dest in x.module.Destinations)
				{
					if (configuration.TryGetValue(dest, out var nextModule))
						pulses.Enqueue((nextModule, nextPulse, x.module.Name));
				}
			}
		}

		for (var i = 0; i < 1000; i++)
			PushButton();

		var part1 = low * high;

		var part2 = 1L;
		var bases = inputs[inputs["rx"].First()].ToList();
		foreach (var b in bases)
		{
			var counter = inputs[b].First();
			var node = inputs[counter]
				.Intersect(configuration[counter].Destinations)
				.First();

			var num = 0;
			var idx = 0;
			while (!configuration[node].Destinations.SequenceEqual([counter]))
			{
				var flag = configuration[node].Destinations
					.Contains(counter);

				if (flag)
					num |= 1 << idx;
				idx++;
				node = configuration[node].Destinations.First(x => x != counter);
			}

			num |= 1 << idx;

			part2 = Lcm(part2, num);
		}

		return (part1.ToString(), part2.ToString());
	}
}
